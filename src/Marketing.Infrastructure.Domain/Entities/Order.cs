using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Marketing.Infrastructure.Domain.Entities
{
    [PrimaryKey(nameof(CollectionId))]
    [Table("vwExItemsAgg")]
    public class Order
    {
        public int CollectionId { get; set; }
        public int PersonId { get; set; }
        public string? LeadSource { get; set; }
        [Column("StatusId")]
        public Status Status { get; set; }
        [Column("DomainId")]
        public Domain? Domain { get; set; }
        [Column("FlowId")]
        public Flow Flow { get; set; }
        [Column("Created")]
        public DateTime CreatedDate { get; set; }
        [Column("StatusExpirationTime")]
        public DateTime? ExpirationDate { get; set; }
    }
}
