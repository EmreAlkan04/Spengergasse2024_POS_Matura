using Bogus.DataSets;
using System.ComponentModel.DataAnnotations;

namespace SPG_Fachtheorie.Aufgabe1.Model
{
    public class Address
    {

#pragma warning disable CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.
        protected Address()
#pragma warning restore CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.
        {
        }
        public Address(string street, int zip, string city)
        {
            Street = street;
            Zip = zip;
            City = city;
        }



        [StringLength(100), MinLength(1)]
        public string Street { get; set; }
        public int Zip { get; set; }
        public string City { get; set; }
    }
}