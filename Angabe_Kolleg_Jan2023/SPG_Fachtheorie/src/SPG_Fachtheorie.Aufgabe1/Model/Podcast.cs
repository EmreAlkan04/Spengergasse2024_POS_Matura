using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SPG_Fachtheorie.Aufgabe1.Model
{
    public class Podcast
    {

#pragma warning disable CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.
        protected Podcast() { }
#pragma warning restore CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.
        public Podcast(string titel, int laengeInSekunden, Category category, RadioStation radioStation)
        {
            Titel = titel;
            this.laengeInSekunden = laengeInSekunden;
            Category = category;
            RadioStation = radioStation;
        }

        public int Id { get; set; }
        public string Titel { get; set; }
        public int laengeInSekunden { get; set; }
        public Category Category { get; set; }
        public RadioStation? RadioStation { get; set; }
        //[ForeignKey("Rating")]
        //public int RatingId { get; set; }
        public List<Rating> RatingList { get; set; } = new();
    }
}
