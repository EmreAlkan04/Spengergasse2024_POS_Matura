using Bogus.DataSets;
using System.ComponentModel.DataAnnotations;

namespace SPG_Fachtheorie.Aufgabe1.Model
{
    public class Manager : Employee
    {

#pragma warning disable CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.
        protected Manager() { }
#pragma warning restore CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.
        public Manager(int registrationNumber, string firstName, string lastName, Address? address,   string carType) //int managerId,
            : base(registrationNumber: registrationNumber, firstName: firstName, lastName: lastName, address: address)
        {
            //ManagerId = managerId;
            CarType = carType;
        }

        //[Key]
        //public int ManagerId { get; set; }
        [StringLength(255)]
        public string CarType { get; set; }
    }
}