using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Marketing.Api.Model.Halda;
using Marketing.Api.Model.WordPress;
using Marketing.Application.Domain;

namespace Marketing.Api.Factories
{
    public class DomainFactory
    {
        private static Regex NameExpression = new(@"^(?<FirstName>\w+)\W*(?<LastName>.*)$", RegexOptions.Compiled);

        public Lead CreateLead(SmartForm<WheelchairFormData> smartForm)
        {
            var lead = new Lead
            {
                Source = "Halda",
                SourceId = smartForm.LeadId!,
                FirstName = smartForm.FormData!.FirstName,
                LastName = smartForm.FormData.LastName,
                PhoneNumber = smartForm.FormData.PhoneNumber,
                Interest = smartForm.FormData.Interest,
                EmailAddress = smartForm.FormData.EmailAddress,
                Use = smartForm.FormData.Use,            
                Contact = smartForm.FormData.Contact,     
                Equipment = smartForm.FormData.Equipment, 
                Insurance = smartForm.FormData.Insurance,
                SecondaryInsurance = smartForm.FormData.Secondary
            };

            return lead;
        }

        public Lead CreateLead(PowerChairOffer offer)
        {
            var lead = new Lead
            {
                Source = "GoogleAds",
                SourceId = offer.SerialNumber,
                FirstName = NameExpression.Match(offer.Name).Groups["FirstName"].Value.Trim(),
                LastName = NameExpression.Match(offer.Name).Groups["LastName"].Value.Trim(),
                PhoneNumber = offer.Phone.Replace("(", "").Replace(") ", "-"),
                Interest = "Power Wheelchair",
                EmailAddress = offer.Email,
                Insurance = offer.InsuranceType ?? "",
                SecondaryInsurance = offer.OtherInsurance
            };

            return lead;
        }

        public Lead CreateLead(ScooterOffer offer)
        {
            var lead = new Lead
            {
                Source = "GoogleAds",
                FirstName = NameExpression.Match(offer.Name).Groups["FirstName"].Value.Trim(),
                LastName = NameExpression.Match(offer.Name).Groups["LastName"].Value.Trim(),
                PhoneNumber = offer.Phone.Replace("(", "").Replace(") ", ""),
                Interest = "Scooter",
                EmailAddress = offer.Email,
                Insurance = offer.InsuranceType ?? "",
                SecondaryInsurance = offer.OtherInsurance
            };

            return lead;
        }
    }
}