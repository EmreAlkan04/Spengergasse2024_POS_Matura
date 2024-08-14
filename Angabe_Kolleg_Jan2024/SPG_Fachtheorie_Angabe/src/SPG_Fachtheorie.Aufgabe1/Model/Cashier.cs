using System.ComponentModel.DataAnnotations;

namespace SPG_Fachtheorie.Aufgabe1.Model
{
    public class Cashier : Employee
    {

#pragma warning disable CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.
        protected Cashier() { }
#pragma warning restore CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.
        public Cashier(int registrationNumber, string firstName, string lastName, Address? address, string jobSpezialisation) //int cashierId,
            : base(registrationNumber:registrationNumber, firstName: firstName, lastName: lastName, address:address)
        {
            //CashierId = cashierId;
            JobSpezialisation = jobSpezialisation;
        }

        //[Key]
        //public int CashierId { get; set; }
        [StringLength(255)]
        public string JobSpezialisation { get; set; }
    }
}