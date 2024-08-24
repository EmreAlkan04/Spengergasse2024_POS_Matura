using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SPG_Fachtheorie.Aufgabe1.Model
{
    public class RadioStation
    {

#pragma warning disable CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.
        protected RadioStation() { }
#pragma warning restore CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.
        public RadioStation(string name, string adresse, bool istOnline, string? linkOnline, string? frequenzOffline)
        {
            Name = name;
            Adresse = adresse;
            IstOnline = istOnline;
            LinkOnline = linkOnline;
            FrequenzOffline = frequenzOffline;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Adresse { get; set; }
        public bool IstOnline { get; set; }
        public string? LinkOnline { get; set; }
        public string? FrequenzOffline { get; set; }
        //[ForeignKey("Podcast")]
        //public int PodcastId { get; set; }
        public List<Podcast> Podcasts { get; set; } = new();
    }
}
