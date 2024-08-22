namespace FTSept2022.Aufgabe1.Models
{
    public class Position
    {
        public Position(decimal laengengrad, decimal breitengrad, decimal hoehe)
        {
            Laengengrad = laengengrad;
            Breitengrad = breitengrad;
            Hoehe = hoehe;
        }

        public decimal Laengengrad { get; private set; }
        public decimal Breitengrad { get; private set; }
        public decimal Hoehe { get; private set; }
    }
}
