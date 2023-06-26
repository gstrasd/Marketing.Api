using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marketing.Application.Domain
{
    public class Lead
    {
        public string? SourceId { get; set; }
        public string Source { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Interest { get; set; } = null!;
        public string? EmailAddress { get; set; }
        public string? Use { get; set; }
        public string? Contact { get; set; }
        public string? Equipment { get; set; }
        public string? Insurance { get; set; }
        public string? SecondaryInsurance { get; set; }
    }
}