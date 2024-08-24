using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPG_Fachtheorie.Aufgabe2.Domain
{
    public class Advertisement : Item
    {
        public string ProductName { get; set; } = string.Empty;
        public int? MinPlayTime { get; set; }
        public decimal CostsPerPlay { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = default!;
    }
}
