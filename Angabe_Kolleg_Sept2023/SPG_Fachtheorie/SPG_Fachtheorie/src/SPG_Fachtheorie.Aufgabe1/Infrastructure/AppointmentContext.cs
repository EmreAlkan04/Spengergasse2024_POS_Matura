using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SPG_Fachtheorie.Aufgabe1.Model;
using System.Globalization;
using System.Threading.Tasks;

namespace SPG_Fachtheorie.Aufgabe1.Infrastructure
{
    public class AppointmentContext : DbContext
    {

        public DbSet<Appointment> Appointments => Set<Appointment>();
        public DbSet<AppointmentState> AppointmentStates => Set<AppointmentState>();
        public DbSet<Address> Addresses => Set<Address>();
        public DbSet<Patient> Patients => Set<Patient>();
        public DbSet<ConfirmedAppointmentState> ConfirmedAppointmentStates => Set<ConfirmedAppointmentState>();
        public DbSet<DeletedAppointmentState> DeletedAppointmentStates => Set<DeletedAppointmentState>();
        public AppointmentContext(DbContextOptions options)
            : base(options)
        { 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Appointment>().HasKey(a => a.AppointmentId);
            modelBuilder.Entity<AppointmentState>().HasKey(a => a.AppointmentStateId);
            modelBuilder.Entity<AppointmentState>().HasDiscriminator<string>("Type");
            
            modelBuilder.Entity<Patient>().OwnsOne(p => p.Address, a =>
            {
                a.Property(p => p.Street).HasColumnName("Street");
                a.Property(p => p.Zip).HasColumnName("Zip");
                a.Property(p => p.City).HasColumnName("City");
            });
            
            modelBuilder.Entity<ConfirmedAppointmentState>().HasBaseType<AppointmentState>();
            modelBuilder.Entity<DeletedAppointmentState>().HasBaseType<AppointmentState>();
        }
    }
}