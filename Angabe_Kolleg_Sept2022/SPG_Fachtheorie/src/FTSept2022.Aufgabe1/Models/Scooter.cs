using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FTSept2022.Aufgabe1.Models
{
    public class Scooter
    {
        public Scooter(string herstellfirma, string modellbezeichnung, int kmProH)
        {
            Herstellfirma = herstellfirma;
            Modellbezeichnung = modellbezeichnung;
            KmProH = kmProH;
            //ScooterPositions = new List<ScooterPosition>();
        }

#pragma warning disable CS8618
        protected Scooter() { }
#pragma warning restore CS8618

        [Key, MinLength(8)]
        public int ScooterId { get; set; }
        public string Herstellfirma { get; set; }
        public string Modellbezeichnung { get; set; }
        public int KmProH { get; set; }

        [ForeignKey("ScooterState")]
        public int ScooterStateId { get; set; }

        // Navigation property for the 1:n relationship
        public List<ScooterPosition> ScooterPositions { get; set; } = new();
        public List<Trip> Trips { get; set; } = new();
    }
}
