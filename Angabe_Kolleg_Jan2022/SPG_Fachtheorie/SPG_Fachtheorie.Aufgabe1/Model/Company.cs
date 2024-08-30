namespace SPG_Fachtheorie.Aufgabe1.Model
{
    public class Company
    {
        protected Company() { }
        public Company(string firmenname, string anschrift, string email, string telefonnummer)
        {
            Firmenname = firmenname;
            Anschrift = anschrift;
            Email = email;
            Telefonnummer = telefonnummer;
        }

        public int Id { get; set; }
        public string Firmenname { get; set; }
        public string Anschrift { get; set; }
        public string Email { get; set; }
        public string Telefonnummer { get; set; }
    }
}
