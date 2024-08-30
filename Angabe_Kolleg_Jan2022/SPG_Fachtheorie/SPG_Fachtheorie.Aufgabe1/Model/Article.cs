namespace SPG_Fachtheorie.Aufgabe1.Model
{
    public class Article
    {
        protected Article() { }
        public Article(int articleNumber, int preis)
        {
            ArticleNumber = articleNumber;
            Preis = preis;
        }

        public int Id { get; set; }
        public int ArticleNumber { get; set; }
        public int Preis { get; set; }
    }
}
