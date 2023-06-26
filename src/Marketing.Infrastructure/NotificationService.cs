using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Marketing.Application.Domain;
using Marketing.Infrastructure.Domain;
using Marketing.Infrastructure.Domain.Entities;

namespace Marketing.Infrastructure
{
    internal class NotificationService : INotificationService
    {
        private readonly Settings _settings;
        private readonly string _failedIntakeTemplate;

        public NotificationService(Settings settings, string failedIntakeTemplate)
        {
            _settings = settings;
            _failedIntakeTemplate = failedIntakeTemplate;
        }

        public async Task SendFailedIntakeEmailAsync(MarketingIntake intake, string payload, Exception? error = null)
        {
            var json = JsonDocument.Parse(payload);
            var pretty = JsonSerializer.Serialize(json, new JsonSerializerOptions { WriteIndented = true });
            var formatted = "<p>" + pretty.Replace(" ", "&nbsp;&nbsp;").ReplaceLineEndings("<br/>") + "</p>";
            var body = _failedIntakeTemplate
                .Replace("{LeadSource}", intake.LeadSource)
                .Replace("{CreatedDate}", intake.CreatedDate.ToString("MM/dd/yyyy hh:mm tt"))
                .Replace("{Database}", Regex.Match(_settings.ConnectionStrings["Orbit"], "Database=(?<Database>[^;]+);").Groups["Database"].Value)
                .Replace("{PayloadHash}", intake.PayloadHash.ToString())
                .Replace("{ContentLength}", payload.Length.ToString())
                .Replace("{Payload}", formatted);
            var email = _settings.Email;
            using var client = GetEmailClient();
            using var message = new MailMessage
            {
                Subject = "Marketing Intake Failure",
                From = new MailAddress(email.From.Address, email.From.DisplayName),
                Priority = MailPriority.High,
                Body = body,
                BodyEncoding = Encoding.UTF8,
                IsBodyHtml = true
            };
            email.To.ForEach(a => message.To.Add(new MailAddress(a.Address, a.DisplayName)));

            await client.SendMailAsync(message);
        }

        private SmtpClient GetEmailClient()
        {
            var email = _settings.Email;
            return new SmtpClient
            {
                Host = email.Client.Host,
                Port = email.Client.Port,
                EnableSsl = true,
                Credentials = new NetworkCredential(email.Client.Username, email.Client.Password)
            };
        }
    }
}