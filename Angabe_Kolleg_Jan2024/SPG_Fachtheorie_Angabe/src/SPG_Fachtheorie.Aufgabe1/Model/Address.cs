using System.ComponentModel.DataAnnotations;

namespace SPG_Fachtheorie.Aufgabe1.Model
{
    public class Address
    {
        //Value Object haben keine Id
#pragma warning disable CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.
        protected Address() { }
#pragma warning restore CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.

        public Address(string street, string city, string zip)
        {
            Street = street;
            City = city;
            Zip = zip;
        }

        //        [Key] public int AddressId { get; set; }

        [StringLength(255)]
        public string Street { get; set; }
        [StringLength(255)]
        public string City { get; set; }
        [StringLength(255)]
        public string Zip { get; set; }
    }
}