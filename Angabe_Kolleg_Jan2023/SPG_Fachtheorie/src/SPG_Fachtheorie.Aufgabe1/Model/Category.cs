
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SPG_Fachtheorie.Aufgabe1.Model
{
    public class Category
    {

#pragma warning disable CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.
        protected Category() { }
#pragma warning restore CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.
        public Category(string name, bool top, bool onlyPremium)
        {
            Name = name;
            Top = top;
            OnlyPremium = onlyPremium;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool Top { get; set; }
        public bool OnlyPremium { get; set; }
        //[ForeignKey("Podcast")]
        //public int PodcastId { get; set; }
        //[ForeignKey("Favorite")]
        //public int FavoriteId { get; set; }
        public List<Podcast> Podcasts { get; set; } = new();
        public List<Favorite> Favorites { get; set; } = new();
    }
}

