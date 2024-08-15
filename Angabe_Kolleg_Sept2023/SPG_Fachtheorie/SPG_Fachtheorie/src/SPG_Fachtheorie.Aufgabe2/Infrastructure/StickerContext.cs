using Bogus;
using Microsoft.EntityFrameworkCore;
using SPG_Fachtheorie.Aufgabe2.Model;
using System;
using System.Linq;

namespace SPG_Fachtheorie.Aufgabe2.Infrastructure
{
    public class StickerContext : DbContext
    {
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Sticker> Stickers => Set<Sticker>();
        public DbSet<StickerType> StickerTypes => Set<StickerType>();
        public DbSet<Vehicle> Vehicles => Set<Vehicle>();

        public StickerContext()
        { }

        public StickerContext(DbContextOptions options) : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite(@"Data Source=sticker.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sticker>().Property(s => s.Price).HasPrecision(9, 4);
            modelBuilder.Entity<StickerType>().Property(s => s.Price).HasPrecision(9, 4);
            modelBuilder.Entity<Vehicle>().Property(v => v.VehicleType).HasConversion<string>();

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // Generic config for all entities
                // ON DELETE RESTRICT instead of ON DELETE CASCADE
                foreach (var key in entityType.GetForeignKeys())
                    key.DeleteBehavior = DeleteBehavior.Restrict;

                foreach (var prop in entityType.GetDeclaredProperties())
                {
                    // Define Guid as alternate key. The database can create a guid fou you.
                    if (prop.Name == "Guid")
                    {
                        modelBuilder.Entity(entityType.ClrType).HasAlternateKey("Guid");
                        prop.ValueGenerated = Microsoft.EntityFrameworkCore.Metadata.ValueGenerated.OnAdd;
                    }
                    // Default MaxLength of string Properties is 255.
                    if (prop.ClrType == typeof(string) && prop.GetMaxLength() is null) prop.SetMaxLength(255);
                    // Seconds with 3 fractional digits.
                    if (prop.ClrType == typeof(DateTime)) prop.SetPrecision(3);
                    if (prop.ClrType == typeof(DateTime?)) prop.SetPrecision(3);
                }
            }
        }

        public void Seed()
        {
            Randomizer.Seed = new Random(1028);
            var minDate = new DateTime(2021, 1, 1);
            var maxDate = new DateTime(2023, 10, 1);
            // https://www.asfinag.at/maut-vignette/vignette/
            var stickerTypes = new StickerType[]
            {
                new StickerType(name: "10-Tages-Vignette PKW", vehicleType: VehicleType.PassengerCar, daysValid: 10, price: 9.9M),
                new StickerType(name: "10-Tages-Vignette Motorrad", vehicleType: VehicleType.Motorcycle, daysValid: 10, price: 5.8M),
                new StickerType(name: "2-Monats-Vignette PKW", vehicleType: VehicleType.PassengerCar, daysValid: 60, price: 29M),
                new StickerType(name: "2-Monats-Vignette Motorrad", vehicleType: VehicleType.Motorcycle, daysValid: 60, price: 14.5M),
                new StickerType(name: "Jahres-Vignette PKW", vehicleType: VehicleType.PassengerCar, daysValid: 365, price: 96.40M),
                new StickerType(name: "Jahres-Vignette Motorrad", vehicleType: VehicleType.Motorcycle, daysValid: 365, price: 38.20M),
            };
            StickerTypes.AddRange(stickerTypes);
            SaveChanges();

            var customers = new Faker<Customer>("de").CustomInstantiator(f =>
            {
                var lastname = f.Name.LastName();
                var email = $"{lastname.ToLower()}@{f.Internet.DomainName()}";
                // Make second accurate
                var registrationDate = new DateTime(f.Date.Between(minDate, maxDate).Ticks / TimeSpan.TicksPerSecond * TimeSpan.TicksPerSecond);
                var customer = new Customer(f.Name.FirstName(), lastname, email, registrationDate);
                var country = new string[] { "BN", "NK", "OW", "W" };
                var numberplatesWithTypes = Enumerable.Range(1, f.Random.Int(1, 3))
                    .Select(i => new
                    {
                        Numberplate = $"{f.Random.ListItem(country)} {f.Random.String2(5, "0123456789")}{f.Random.String2(1, "ABCDEFGHIJKLMNOPRSTUVWXYZ")}",
                        VehicleType = f.Random.Enum<VehicleType>()
                    })
                    .ToList();

                var vehicles = numberplatesWithTypes.Select(n => new Vehicle(n.Numberplate, customer, n.VehicleType)).ToList();

                var stickers = new Faker<Sticker>("de").CustomInstantiator(f =>
                {
                    var numberplateWithType = f.Random.ListItem(numberplatesWithTypes);
                    var stickerType = f.Random.ListItem(stickerTypes.Where(s => s.VehicleType == numberplateWithType.VehicleType).ToList());
                    var purchaseDate = new DateTime(f.Date.Between(customer.RegistrationDate, maxDate).Ticks / TimeSpan.TicksPerSecond * TimeSpan.TicksPerSecond);
                    var validFrom = purchaseDate.Date.AddDays(f.Random.Int(0, 60));
                    var price = purchaseDate.Year switch
                    {
                        >= 2023 => stickerType.Price,
                        2022 => stickerType.Price * 0.9M,
                        _ => stickerType.Price * 0.8M,
                    };
                    return new Sticker(
                        numberplate: numberplateWithType.Numberplate, customer: customer,
                        stickerType: stickerType, purchaseDate: purchaseDate, validFrom: validFrom,
                        price: price);
                })
                .Generate(f.Random.Int(0, 4))
                .ToList();
                customer.Vehicles.AddRange(vehicles);
                customer.Stickers.AddRange(stickers);
                return customer;
            })
            .Generate(10)
            .DistinctBy(c => c.Email)
            .ToList();
            Customers.AddRange(customers);
            SaveChanges();
        }
    }
}