using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketing.Application.Domain.Halda
{
    public class Campaign
    {
        public string VariantId { get; set; } = null!;
        public int ItemId { get; set; }
        public string Description { get; set; } = null!;
    }
}
