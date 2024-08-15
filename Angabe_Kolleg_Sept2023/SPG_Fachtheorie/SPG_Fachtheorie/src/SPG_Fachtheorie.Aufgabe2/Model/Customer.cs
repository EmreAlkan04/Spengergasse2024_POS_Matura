using System;
using System.Collections.Generic;

namespace SPG_Fachtheorie.Aufgabe2.Model
{
    public class Customer : Entity<int>
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        protected Customer()
        { }

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public Customer(string firstname, string lastname, string email, DateTime registrationDate)
        {
            Firstname = firstname;
            Lastname = lastname;
            Email = email;
            RegistrationDate = registrationDate;
        }

        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public DateTime RegistrationDate { get; set; }
        public List<Vehicle> Vehicles { get; set; } = new();
        public List<Sticker> Stickers { get; set; } = new();
    }
}