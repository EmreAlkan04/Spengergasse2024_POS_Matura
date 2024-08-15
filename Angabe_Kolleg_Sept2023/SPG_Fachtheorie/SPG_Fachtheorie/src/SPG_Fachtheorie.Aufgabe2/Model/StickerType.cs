using Microsoft.EntityFrameworkCore;
using System;

namespace SPG_Fachtheorie.Aufgabe2.Model
{
    [Index(nameof(Name), IsUnique = true)]
    public class StickerType : Entity<int>
    {
        public StickerType(string name, VehicleType vehicleType, int daysValid, decimal price)
        {
            Name = name;
            VehicleType = vehicleType;
            DaysValid = daysValid;
            Price = price;
        }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        protected StickerType()
        { }

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public string Name { get; set; }
        public VehicleType VehicleType { get; set; }
        public int DaysValid { get; set; }
        public decimal Price { get; set; }
    }
}