using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SPG_Fachtheorie.Aufgabe1.Model
{
    public class Payment
    {
#pragma warning disable CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.
        protected Payment() { }
#pragma warning restore CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.
        public Payment(int paymentId, DateTime paymentDateTime, PaymentType paymentType, CashDesk cashDesk, Employee employee)
        {
            PaymentId = paymentId;
            PaymentDateTime = paymentDateTime;
            PaymentType = paymentType;
            CashDesk = cashDesk;
            Employee = employee;
        }

        [Key]
        public int PaymentId { get; set; }
        public DateTime PaymentDateTime { get; set; }
        public PaymentType PaymentType { get; set; }

       // [ForeignKey(nameof(CashDesk))]
        public CashDesk CashDesk { get; set; }

       // [ForeignKey(nameof(Employee))]
        public Employee Employee { get; set; }

        [MinLength(1)]
        public List<PaymentItem> PaymentItems { get; set; } = new();
    }
}