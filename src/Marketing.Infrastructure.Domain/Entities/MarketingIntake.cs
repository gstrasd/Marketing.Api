using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Marketing.Infrastructure.Domain.Entities
{
    [Table("MarketingIntake")]
    [PrimaryKey(nameof(Id))]
    public class MarketingIntake
    {
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public string LeadSource { get; set; } = null!;
        public string Payload { get; set; } = null!;
        public Guid PayloadHash { get; set; }
        public bool Success { get; set; }
        public DateTime CreatedDate { get; set; }
        public int SessionId { get; set; }
        public string? Error { get; set; }
    }
}
