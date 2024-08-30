namespace SPG_Fachtheorie.Aufgabe1.Model
{
    public class Employee
    {
        protected Employee() { }
        public Employee(string nachname, string vorname)
        {
            Nachname = nachname;
            Vorname = vorname;
        }

        public int Id { get; set; }
        public string Nachname { get; set; }
        public string Vorname { get; set; }
    }
}
