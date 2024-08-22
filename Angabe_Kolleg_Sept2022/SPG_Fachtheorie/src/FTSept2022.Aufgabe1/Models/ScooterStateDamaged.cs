using System;

namespace FTSept2022.Aufgabe1.Models
{
    public class ScooterStateDamaged : ScooterState
    {
        public ScooterStateDamaged(Scooter scooter, Position position, Customer customer, DateTime timeStamp, string type, string link, string schaden)
            : base(scooter:scooter, position:position, customer:customer, timeStamp:timeStamp, type:type)
        {
            Link = link;
            Schaden = schaden;
        }

#pragma warning disable CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.
        protected ScooterStateDamaged()
#pragma warning restore CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.
        {
        }

        public string Link { get; set; }
        public string Schaden { get; set; }
    }
}
