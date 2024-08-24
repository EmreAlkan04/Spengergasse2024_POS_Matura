using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SPG_Fachtheorie.Aufgabe1.Model
{
    public class User
    {

#pragma warning disable CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.
        protected User () {         }
#pragma warning restore CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.
        public User(string firstName, string lastName, string email, DateTime startDate, DateTime? endDate)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            StartDate = startDate;
            EndDate = endDate;
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        //[ForeignKey("Favorite")]
        //public int FavoriteId { get; set; }
        public List<Favorite> Favorites { get; set; } = new();
    }
}
