using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SPG_Fachtheorie.Aufgabe1.Model
{
    public class PaymentItem
    {
#pragma warning disable CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.
        protected PaymentItem() { }
#pragma warning restore CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.
        public PaymentItem(int paymentItemId, string articleName, int amount, decimal price, Payment payment)
        {
            PaymentItemId = paymentItemId;
            ArticleName = articleName;
            Amount = amount;
            Price = price;
            Payment = payment;
        }

        [Key]
        public int PaymentItemId { get; set; }
        [StringLength(255)]
        public string ArticleName { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }

        //[ForeignKey(nameof(Payment))]
        public Payment Payment { get; set; }
    }
}