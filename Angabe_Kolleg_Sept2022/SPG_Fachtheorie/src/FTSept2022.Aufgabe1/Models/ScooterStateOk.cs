using System;

namespace FTSept2022.Aufgabe1.Models
{
    public class ScooterStateOk : ScooterState
    {
        public ScooterStateOk(Scooter scooter, Position position, Customer customer, DateTime timeStamp, string type) : base(scooter : scooter, position: position, customer: customer, timeStamp: timeStamp, type: type)
        {
         
        }

#pragma warning disable CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.
        protected ScooterStateOk()
#pragma warning restore CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.
        {
        }

    }
}
