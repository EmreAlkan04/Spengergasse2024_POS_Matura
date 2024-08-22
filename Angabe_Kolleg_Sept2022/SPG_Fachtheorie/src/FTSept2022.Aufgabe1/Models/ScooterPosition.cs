using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FTSept2022.Aufgabe1.Models
{
    public class ScooterPosition
    {
        public ScooterPosition(Scooter scooter, Position position, DateTime meldung)
        {
            Scooter = scooter;
            Position = position;
            Meldung = meldung;
        }

#pragma warning disable CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.
        protected ScooterPosition()
#pragma warning restore CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.
        {
        }

        public int Id { get; set; }
        public Scooter Scooter { get; set; }
        public Position Position { get; set; }
        public DateTime Meldung { get; set; }
        
        //[ForeignKey("Scooter")]
        //public int ScooterId { get; set; }
    }
}
