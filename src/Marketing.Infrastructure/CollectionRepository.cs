using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using Marketing.Application.Domain;
using Marketing.Infrastructure.Domain;
using Marketing.Infrastructure.Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Marketing.Infrastructure
{
    internal class CollectionRepository : ICollectionRepository
    {
        private static readonly TimeSpan OneYear = new(365, 0, 0, 0);
        private static readonly string[] Sources = { "GoogleAds", "Google Lead", "Halda", "Halda Lead" };
        private static readonly Regex PhoneNumberExpression = new(@"^(?<AreaCode>\d{3})-?(?<Prefix>\d{3})-?(?<Suffix>\d{4})$", RegexOptions.Compiled);
        private static readonly Regex QuotesExpression = new(@"'|""", RegexOptions.Compiled);
        private readonly OrbitDbContext _dbContext;
        private readonly IIntakeRepository _intakeRepository;
        private readonly Settings _settings;
        private readonly AsyncLocal<int> _sessionId;
        private readonly AsyncLocal<int?> _intakeId;

        public CollectionRepository(OrbitDbContext dbContext, IIntakeRepository intakeRepository, Settings settings, AsyncLocal<int> sessionId, AsyncLocal<int?> intakeId)
        {
            _dbContext = dbContext;
            _intakeRepository = intakeRepository;
            _settings = settings;
            _sessionId = sessionId;
            _intakeId = intakeId;
        }

        public async Task<(Order? Order, Item? Item)> FindLeadAsync(int personId)
        {
            // change to six months
            var sixMonthsAgo = DateTime.Today - new TimeSpan(180, 0, 0, 0);
            
            // Look for the most recent Halda or Google lead for the given patient in the past six months
            var lead = await (
                    from o in _dbContext.Orders
                    join i in _dbContext.Items on o.CollectionId equals i.CollectionId
                    where
                        o.PersonId == personId
                        && Sources.Contains(o.LeadSource)
                        && o.Domain == Domain.Domain.Marketing
                        && o.CreatedDate > sixMonthsAgo
                    orderby o.CollectionId descending
                    select new { o, i })
                .FirstOrDefaultAsync();

            return (lead?.o, lead?.i);
        }

        public async Task<int> CreateLeadAsync(Lead lead, Patient? patient = null)
        {
            var item = _settings.LeadItems.FirstOrDefault(i => i.Source == lead.Source && lead.Interest.Contains(i.Product, StringComparison.OrdinalIgnoreCase));
            if (item == null) throw new ApplicationException($"Could not find {lead.Source} {lead.Interest} product.");

            // Ensure these values can be passed in an XML attribute and passed as an SQL parameter
            var scrubbedFirstName = QuotesExpression.Replace(lead.FirstName, "").Trim();
            var scrubbedLastName = QuotesExpression.Replace(lead.LastName, "").Trim();
            var scrubbedEmail = QuotesExpression.Replace(lead.EmailAddress ?? "", "").Trim();
            var scrubbedPrimaryInsurance = QuotesExpression.Replace(lead.Insurance ?? "", "").Trim();
            var scrubbedSecondaryInsurance = QuotesExpression.Replace(lead.SecondaryInsurance ?? "", "").Trim();

            // Parse phone number
            var match = PhoneNumberExpression.Match(lead.PhoneNumber);
            
            // Build notes
            var note = new StringBuilder();
            note.Append($"A new lead has come in from {lead.Source}. ");
            note.Append($"{scrubbedFirstName} {scrubbedLastName} is interested in a {lead.Interest!.ToLower()}");
            if (!String.IsNullOrWhiteSpace(lead.Use))
            {
                note.Append($" which will be used {(lead.Use!.Contains("Outdoor") ? "outdoors" : "inside their residence")}");
                if (lead.Equipment!.Contains("None")) note.AppendLine($". {scrubbedFirstName} is currently walking on his/her own");
                else if (lead.Equipment!.Contains("Other")) note.AppendLine($". {scrubbedFirstName} is currently using something else");
                else note.Append($". {scrubbedFirstName} is currently using a {lead.Equipment!.ToLower()}");
            }
            note.AppendLine(".");

            
            var order = new StringBuilder()
                .Append(
                    new XElement("Patient",
                        new XAttribute("Id", patient?.PersonId ?? 0),
                        new XAttribute("FirstName", scrubbedFirstName.Length <= 64 ? scrubbedFirstName : scrubbedFirstName[..64].Trim()),
                        new XAttribute("LastName", scrubbedLastName.Length <= 64 ? scrubbedLastName : scrubbedLastName[..64].Trim()),
                        new XAttribute("DOB", patient?.DateOfBirth?.ToShortDateString() ?? "01/01/1894"),
                        new XAttribute("Gender", patient?.Gender ?? "U"),
                        new XAttribute("AreaCode", match.Success ? match.Groups["AreaCode"].Value : ""),
                        new XAttribute("Prefix", match.Success ? match.Groups["Prefix"].Value : ""),
                        new XAttribute("Suffix", match.Success ? match.Groups["Suffix"].Value : ""),
                        new XAttribute("Email", scrubbedEmail.Length <= 64 ? scrubbedEmail : scrubbedEmail[..64].Trim()),
                        new XAttribute("PrimaryInsurance", scrubbedPrimaryInsurance.Length <= 64 ? scrubbedPrimaryInsurance : scrubbedPrimaryInsurance[..64].Trim()),
                        new XAttribute("SecondaryInsurance", scrubbedSecondaryInsurance.Length <= 64 ? scrubbedSecondaryInsurance : scrubbedSecondaryInsurance[..64].Trim())
                    ))
                .Append(
                    new XElement("Order",
                        new XAttribute("Id", 0),
                        new XAttribute("Subject", "New Order Notes"),
                        new XAttribute("SalesRep", ""),
                        new XAttribute("ReferralId", ""),
                        new XAttribute("DischargeId", ""),
                        new XAttribute("Discharge", ""),
                        new XAttribute("LeadSource", lead.Source),
                        new XAttribute("SessionId", _sessionId.Value),
                        new XAttribute("IntakeOrderId", ""),
                        new XAttribute("DocType", "Initial Documentation"))
                ).Append(
                    new XElement("Items",
                        new XElement("Item",
                            new XAttribute("ProductId", item.ProductId),
                            new XAttribute("DomainId", (int)Domain.Domain.Marketing),
                            new XAttribute("Name", "")
                        )
                    )
                ).ToString();

            var collectionId = new SqlParameter("@result", SqlDbType.Int) { Direction = ParameterDirection.Output };
            await _dbContext.Database.ExecuteSqlRawAsync($"exec @result = dbo.spNewOrder @order='{order}', @note='{note}'; select @result", collectionId);

            return (int)collectionId.Value;
        }

        public async Task<int> AddNoteAsync(int collectionId, string subject, string note)
        {
            var scrubbedSubject = subject.Replace("'", "").Trim();
            var scrubbedNote = note.Replace("'", "").Trim();
            var noteId = new SqlParameter("@result", SqlDbType.Int) { Direction = ParameterDirection.Output };
            await _dbContext.Database.ExecuteSqlRawAsync($"exec @result = dbo.spAddNote @objectid='{collectionId}', @ctext='C', @subject='{scrubbedSubject}', @note='{scrubbedNote}', @sessionId={_sessionId.Value}; select @result", noteId);

            return (int)noteId.Value;
        }

        public async Task<bool> CheckoutAsync(int collectionId, Flow flow)
        {
            var result = new SqlParameter("@result", SqlDbType.Int) { Direction = ParameterDirection.Output };
            await _dbContext.Database.ExecuteSqlRawAsync($"exec @result = [dbo].[spCheckoutCollection] @cid={collectionId}, @flowId={(int)flow}; select @result", result);

            return collectionId == (int)result.Value;
        }

        public Task CheckInAsync(int collectionId, Flow flow, int outcome = 0)
        {
            return _dbContext.Database.ExecuteSqlRawAsync($"exec [dbo].[spCheckInCollection] @cid={collectionId}, @flowid={(int)flow}, @outcomeId={outcome}, @sessionId={_sessionId.Value}");
        }
    }
}