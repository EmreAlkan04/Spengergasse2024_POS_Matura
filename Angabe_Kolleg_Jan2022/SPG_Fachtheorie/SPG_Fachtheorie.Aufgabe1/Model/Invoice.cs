using System.ComponentModel.DataAnnotations;

namespace SPG_Fachtheorie.Aufgabe1.Model
{
    public class Invoice
    {
        protected Invoice() { }
        public Invoice(Employee clerk, Customer customer, DateTime rechnungsdatum, int rabat)
        {
            Clerk = clerk;
            Customer = customer;
            Rechnungsdatum = rechnungsdatum;
            Rabat = rabat;
        }

        [Key]
        public int Rechnungsnummer { get; set; }
        public Employee Clerk { get; set; }
        public Customer Customer { get; set; }
        public DateTime Rechnungsdatum { get; set; }
        public int Rabat { get; set; }
    }
}
