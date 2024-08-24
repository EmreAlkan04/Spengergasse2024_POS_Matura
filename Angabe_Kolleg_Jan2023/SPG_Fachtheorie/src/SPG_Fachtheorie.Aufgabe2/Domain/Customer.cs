using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPG_Fachtheorie.Aufgabe2.Domain
{
    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? CompanyName { get; set; }
        public DateTime RegistrationDate { get; set; }
        public decimal? TotalCosts { get; set; }

        public int ResponsibleAdminId { get; set; }
        public Admin ResponsibleAdmin { get; set; } = default!;

        public List<Advertisement> Advertisements { get; set; } = new();
    }
}
