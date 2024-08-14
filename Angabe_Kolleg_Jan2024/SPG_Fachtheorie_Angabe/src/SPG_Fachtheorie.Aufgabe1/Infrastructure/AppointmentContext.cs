using Microsoft.EntityFrameworkCore;
using SPG_Fachtheorie.Aufgabe1.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace SPG_Fachtheorie.Aufgabe1.Infrastructure
{
    public class AppointmentContext : DbContext
    {
        // TODO: Add your DbSets here
        public DbSet<CashDesk> CashDesks => Set<CashDesk>();
        public DbSet<Cashier> Cashiers => Set<Cashier>();
        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<Manager> Managers => Set<Manager>();
        public DbSet<Payment> Payments => Set<Payment>();
        public DbSet<PaymentItem> PaymentItems => Set<PaymentItem>();
        public DbSet<Address> Addresses => Set<Address>();  

        public AppointmentContext(DbContextOptions options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // TODO: Add your configuration here
            modelBuilder.Entity<CashDesk>().HasKey(c => c.Number);
          
            //ValueObject ist Address und wird in der Klasse Employee verwendet
            modelBuilder.Entity<Employee>()
                .OwnsOne(e => e.Address, p =>
                {
                    p.Property(a => a.Street).HasColumnName("Street");
                    p.Property(a => a.City).HasColumnName("City");
                    p.Property(a => a.Zip).HasColumnName("Zip");
                }
                )
                .HasDiscriminator<string>("Type");
                                           //.HasValue<Manager>("Manager")
                                           //.HasValue<Cashier>("Cashier");
                                           
            modelBuilder.Entity<Payment>().HasKey(p => p.PaymentId);
            modelBuilder.Entity<PaymentItem>().HasKey(p => p.PaymentItemId);

            //modelBuilder.Entity<Cashier>().OwnsOne(c => c.Address);
            //modelBuilder.Entity<Manager>().OwnsOne(c => c.Address);
            //modelBuilder.Entity<Address>().OwnsValue();

            //Erben von Employee
            modelBuilder.Entity<Cashier>().HasBaseType<Employee>();
            modelBuilder.Entity<Manager>().HasBaseType<Employee>();
           
           

        }
    }
}