using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketing.Application.Domain
{
    public class LeadItem
    {
        public string Source { get; set; } = null!;
        public string Product { get; set; } = null!;
        public int ProductId { get; set; }
    }
}
