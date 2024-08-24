using System;

namespace SPG_Fachtheorie.Aufgabe1.Model
{
    public class Favorite
    {

#pragma warning disable CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.
        protected Favorite() { }
#pragma warning restore CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.
        public Favorite(Category category, User user, DateTime beginnDatum, DateTime endDatum)
        {
            Category = category;
            User = user;
            BeginnDatum = beginnDatum;
            EndDatum = endDatum;
        }

        public int Id { get; set; }
        public Category Category { get; set; }
        public User User { get; set; }
        public DateTime BeginnDatum { get; set; }
        public DateTime EndDatum { get; set; }

    }
}
