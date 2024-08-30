using Microsoft.EntityFrameworkCore;
using SPG_Fachtheorie.Aufgabe1.Model;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SPG_Fachtheorie.Aufgabe1
{
    public class InvoiceContext : DbContext
    {
        public InvoiceContext(DbContextOptions opt) : base(opt) { }
        // TOTO: Füge die DbSet<T> Collections hinzu.
        public DbSet<Article> Articles => Set<Article>();
        public DbSet<Company> Companies => Set<Company>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<Invoice> Invoices => Set<Invoice>();
        public DbSet<InvoiceItem> invoiceItems => Set<InvoiceItem>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // TODO: Füge - wenn notwendig - noch Konfigurationen hinzu.
            modelBuilder.Entity<Employee>().HasKey(e => e.Id);
            modelBuilder.Entity<Article>().HasKey(a => a.Id);
            modelBuilder.Entity<Company>().HasKey(c => c.Id);
            modelBuilder.Entity<Customer>().HasKey(c => c.Kundennummer);
            modelBuilder.Entity<Invoice>().HasKey(i => i.Rechnungsnummer);
            modelBuilder.Entity<InvoiceItem>().HasKey(i => i.Id);
        }

    }
}
