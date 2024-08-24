using System.ComponentModel.DataAnnotations;

namespace SPG_Fachtheorie.Aufgabe1.Model
{
    public class Rating
    {

#pragma warning disable CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.
        protected Rating () {         }
#pragma warning restore CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.
        public Rating(int zahl, string? bewertung, Podcast podcast, User user)
        {
            Zahl = zahl;
            Bewertung = bewertung;
            Podcast = podcast;
            User = user;
        }

        public int Id { get; set; }

        [MinLength(1), MaxLength(5)]
        public int Zahl { get; set; }
        public string? Bewertung { get; set; }
        public Podcast Podcast { get; set; }
        public User User { get; set; }
    }
}
