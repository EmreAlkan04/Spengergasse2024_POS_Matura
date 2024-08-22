using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTSept2022.Aufgabe1.Models
{
    public class Trip
    {
        public Trip(Customer customer, Scooter scooter, DateTime fahrtbeginn, DateTime? fahrtende)
        {
            Customer = customer;
            Scooter = scooter;
            Fahrtbeginn = fahrtbeginn;
            Fahrtende = fahrtende;
        }

#pragma warning disable CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.
        protected Trip()
#pragma warning restore CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.
        {
        }

        public int Id { get; set; }
        public Customer Customer { get; set; }
        public Scooter Scooter { get; set; }
        public DateTime Fahrtbeginn { get; set; }
        public DateTime? Fahrtende { get; set; }

        //[ForeignKey("Customer")]
        //public int CustomerId { get; set; }

        //[ForeignKey("Scooter")]
        //public int ScooterId { get; set; }
    }
}
