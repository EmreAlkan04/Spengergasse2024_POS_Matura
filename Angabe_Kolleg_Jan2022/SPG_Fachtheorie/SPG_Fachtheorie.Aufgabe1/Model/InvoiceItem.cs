namespace SPG_Fachtheorie.Aufgabe1.Model
{
    public class InvoiceItem
    {
        protected InvoiceItem() { }
        public InvoiceItem(Invoice invoice, Article article)
        {
            Invoice = invoice;
            Article = article;
            NewPrice = Article.Preis * Invoice.Rabat;
        }

        public int Id { get; set; }
        public Invoice Invoice { get; set; }
        public Article Article { get; set; }
        public double NewPrice { get; set; }
    }
}
