using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace SPG_Fachtheorie.Aufgabe2.Model
{
    [Index(nameof(Numberplate), IsUnique = true)]
    public class Vehicle : Entity<int>
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        protected Vehicle()
        { }

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public Vehicle(string numberplate, Customer customer, VehicleType vehicleType)
        {
            Numberplate = numberplate;
            Customer = customer;
            VehicleType = vehicleType;
        }

        public string Numberplate { get; set; }
        public Customer Customer { get; set; }
        public VehicleType VehicleType { get; set; }
        public string VehicleInfo => $"{Numberplate} ({VehicleType})";
    }
}