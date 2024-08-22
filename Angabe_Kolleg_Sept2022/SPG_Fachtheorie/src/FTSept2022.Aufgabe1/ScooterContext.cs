using FTSept2022.Aufgabe1.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTSept2022.Aufgabe1
{
    public class ScooterContext : DbContext
    {

        public DbSet<Scooter> Scooters => Set<Scooter>();
        public DbSet<ScooterState> ScooterStates => Set<ScooterState>();
        public DbSet<ScooterStateDamaged> ScooterStateDamageds => Set<ScooterStateDamaged>();
        public DbSet<ScooterStateOk> ScooterStateOks => Set<ScooterStateOk>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Trip> Trips => Set<Trip>();
        public DbSet<ScooterPosition> ScooterPositions => Set<ScooterPosition>();
        //public DbSet<Position> Positions => Set<Position>();
        //Copilot sagt, value objects gehören nicht hierhin.
        public ScooterContext(DbContextOptions options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Scooter>().HasKey(s => s.ScooterId);
            modelBuilder.Entity<Trip>().HasKey(t => t.Id);
            modelBuilder.Entity<Customer>().HasKey(c => c.Id);

            modelBuilder.Entity<ScooterPosition>()
                .OwnsOne(sp => sp.Position, p =>
                {
                    p.Property(pos => pos.Laengengrad).HasColumnName("Laengengrad");
                    p.Property(pos => pos.Breitengrad).HasColumnName("Breitengrad");
                    p.Property(pos => pos.Hoehe).HasColumnName("Hoehe");
                });

            modelBuilder.Entity<ScooterState>()
               .OwnsOne(sp => sp.Position, p =>
               {
                   p.Property(pos => pos.Laengengrad).HasColumnName("Laengengrad");
                   p.Property(pos => pos.Breitengrad).HasColumnName("Breitengrad");
                   p.Property(pos => pos.Hoehe).HasColumnName("Hoehe");
               });

            modelBuilder.Entity<ScooterStateDamaged>().HasBaseType<ScooterState>();
            modelBuilder.Entity<ScooterStateOk>().HasBaseType<ScooterState>();
        }
    }
}
