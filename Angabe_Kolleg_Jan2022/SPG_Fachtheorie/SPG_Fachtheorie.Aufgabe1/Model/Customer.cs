using System.ComponentModel.DataAnnotations;

namespace SPG_Fachtheorie.Aufgabe1.Model
{
    public class Customer
    {
        protected Customer() { }
        public Customer(string nachname, string vorname, string strasse, string ort, Salutation anrede)
        {
            Nachname = nachname;
            Vorname = vorname;
            Strasse = strasse;
            Ort = ort;
            Anrede = anrede;
        }

        [Key]
        public int Kundennummer { get; set; }
        public string Nachname { get; set; }
        public string Vorname { get; set; }
        public string Strasse { get; set; }
        public string Ort { get; set; }
        public Salutation Anrede { get; set; }

        public enum Salutation
        {
            Mr,
            Ms
        }
    }
}
