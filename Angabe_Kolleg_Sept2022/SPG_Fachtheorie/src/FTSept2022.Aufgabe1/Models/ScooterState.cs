using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FTSept2022.Aufgabe1.Models
{
    public class ScooterState
    {
        public ScooterState(Scooter scooter, Position position, Customer customer, DateTime timeStamp, string type)
        {
            Scooter = scooter;
            Position = position;
            Customer = customer;
            TimeStamp = timeStamp;
            Type = type;
        }

#pragma warning disable CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.
        protected ScooterState()
#pragma warning restore CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.
        {
        }

        public int Id { get; set; }
        public Scooter Scooter { get; set; }
        public Position Position { get; set; }
        public Customer Customer { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Type { get; set; }
        

        //[ForeignKey("Scooter")]
        //public int ScooterId { get; set; }
    }
}
