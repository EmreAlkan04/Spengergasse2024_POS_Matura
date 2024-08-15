using System;

namespace SPG_Fachtheorie.Aufgabe2.Model
{
    public class Sticker : Entity<int>
    {
        public Sticker(string numberplate, Customer customer, StickerType stickerType, DateTime purchaseDate, DateTime validFrom, decimal price)
        {
            Numberplate = numberplate;
            Customer = customer;
            StickerType = stickerType;
            PurchaseDate = purchaseDate;
            ValidFrom = validFrom;
            Price = price;
        }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        protected Sticker()
        { }

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public string Numberplate { get; set; }
        public Customer Customer { get; set; }
        public StickerType StickerType { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime ValidFrom { get; set; }
        public decimal Price { get; set; }
    }
}