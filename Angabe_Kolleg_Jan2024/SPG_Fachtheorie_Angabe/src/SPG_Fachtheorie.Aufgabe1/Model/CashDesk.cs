using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SPG_Fachtheorie.Aufgabe1.Model
{
    public class CashDesk
    {
        protected CashDesk() { }
        public CashDesk(int number)
        {
            Number = number;
        }

        [Key]
        public int Number { get; set; }
        public List<Payment> Payments = new();
    }
}