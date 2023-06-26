using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketing.Application.Domain
{
    public class Settings
    {
        public List<LeadItem> LeadItems { get; set; } = new();
        public Dictionary<string, int> Outcomes { get; set; } = new();
        public Dictionary<string, string> ConnectionStrings { get; set; } = null!;
        public EmailSettings Email { get; set; } = null!;
    }

    public class EmailSettings
    {
        public EmailClientSettings Client { get; set; } = null!;
        public MailAddressSettings From { get; set; } = null!;
        public List<MailAddressSettings> To { get; set; } = new();
    }

    public class EmailClientSettings
    {
        public string Host { get; set; } = null!;
        public int Port { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class MailAddressSettings
    {
        public string DisplayName { get; set; } = null!;
        public string Address { get; set; } = null!;
    }
}
