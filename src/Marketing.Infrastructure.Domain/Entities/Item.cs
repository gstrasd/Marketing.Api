using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Marketing.Infrastructure.Domain.Entities
{
    [Keyless]
    [Table("vwExItemData")]
    public class Item
    {
        public int CollectionId { get; set; }
        public int ItemId { get; set; }
        public int? PersonId { get; set; }
        public int ProductId { get; set; }
        [Column("ItemDescription")]
        public string ProductDescription { get; set; } = null!;
        public int DomainId { get; set; }
    }
}