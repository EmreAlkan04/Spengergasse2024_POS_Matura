using System.Collections.Generic;

namespace FTSept2022.Aufgabe1.Models
{
    public class Customer
    {
        public Customer(string vornamen, string nachname, string email, string handynummer, bool confirmed)
        {
            Vornamen = vornamen;
            Nachname = nachname;
            Email = email;
            Handynummer = handynummer;
            Confirmed = confirmed;
        }

#pragma warning disable CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.
        protected Customer()
#pragma warning restore CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.
        {
        }

        public int Id { get; set; }
        public string Vornamen { get; set; }
        public string Nachname { get; set; }
        public string Email { get; set; }
        public string Handynummer { get; set; }
        public bool Confirmed { get; set; }
        public List<Trip> trips { get; set; } = new();
        public List<ScooterState> scooterStates { get; set; } = new();  
    }
}
