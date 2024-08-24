using Bogus;
using SPG_Fachtheorie.Aufgabe2.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace SPG_Fachtheorie.Aufgabe2.Infrastructure
{
    public class PodcastContext : DbContext
    {
        public DbSet<Admin> Admins => Set<Admin>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Item> Items => Set<Item>();
        public DbSet<Advertisement> Advertisements => Set<Advertisement>();
        public DbSet<Podcast> Podcasts => Set<Podcast>();
        public DbSet<ListenedItem> ListenedItems => Set<ListenedItem>();
        public DbSet<Playlist> Playlists => Set<Playlist>();

        public PodcastContext()
        { }
        public PodcastContext(DbContextOptions options)
        : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite(@"Data Source=.\..\..\..\SPG_Fachtheorie.Aufgabe3\Podcast.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>().HasDiscriminator(o => o.ItemType);
            var intArrayValueConverter = new ValueConverter<List<int>, string>(
                i => string.Join(",", i),
                s => string.IsNullOrWhiteSpace(s) ? new List<int>(0) : s.Split(new[] { ',' }).Select(v => int.Parse(v)).ToList());
            modelBuilder.Entity<Podcast>().Property(p => p.PositionForAd).HasConversion(intArrayValueConverter);
        }

        public void Seed()
        {
            var faker = new Faker();
            Randomizer.Seed = new Random(840);
       
            var admins = new Faker<Admin>("de").Rules((f, a) =>
            {
                a.FirstName = f.Name.FirstName();
                a.LastName = f.Name.LastName();
            })
            .Generate(5)
            .ToList();
            Admins.AddRange(admins);
            SaveChanges();

            var customers = new Faker<Customer>("de").Rules((f, c) =>
            {
                c.FirstName = f.Name.FirstName();
                c.LastName = f.Name.LastName();
                c.CompanyName = f.Company.CompanyName().OrDefault(f, 0.2f);
                c.RegistrationDate = f.Date.Between(DateTime.Now.AddDays(-3*52), DateTime.Now.AddDays(-20));
                c.ResponsibleAdmin = f.Random.ListItem(admins);
                c.TotalCosts = f.Random.Decimal(1.0m, 9000.0m).OrDefault(f, 0.3f);
            })
            .Generate(30)
            .ToList();
            Customers.AddRange(customers);
            SaveChanges();

            var advertisements = new Faker<Advertisement>("de").Rules((f, a) =>
            {
                a.Production = f.Date.Between(DateTime.Now.AddDays(-3*52), DateTime.Now.AddDays(-50)); ;
                a.Length = f.Random.Int(2000, 10000);
                a.ProductName = f.Commerce.ProductName();
                a.MinPlayTime = f.Random.Int(1, a.Length/4).OrDefault(f, 0.4f);
                a.CostsPerPlay = f.Random.Decimal(0.1m, 2.5m);
                a.Customer = f.Random.ListItem(customers);
            })
            .Generate(12)
            .ToList();
            Advertisements.AddRange(advertisements);
            SaveChanges();

            var podcasts = new Faker<Podcast>("de").Rules((f, p) =>
            {
                p.Production = f.Date.Between(DateTime.Now.AddDays(-3 * 52), DateTime.Now.AddDays(-50)); ;
                p.Length = f.Random.Int(30000, 60000 * 10);
                p.MaxQuantityAds = f.Random.Int(2, 7);
                for (int i = 0; i < p.MaxQuantityAds; i++)
                {
                    int begin = i > 0 ? p.PositionForAd[i - 1] + 5 : 5;
                    int end = begin + p.Length / p.MaxQuantityAds;
                    p.PositionForAd.Add(f.Random.Int(begin, end));
                }
            })
            .Generate(10)
            .ToList();
            Podcasts.AddRange(podcasts);
            SaveChanges();

            var playlists = new Faker<Playlist>("de").Rules((f, p) =>
            {
                p.Name = f.Name.FirstName();
                p.UserName = p.Name.Substring(0, f.Random.Int(2, p.Name.Length)) + f.Random.Int(1, 9999);
            })
            .Generate(10)
            .ToList();
            Playlists.AddRange(playlists);
            SaveChanges();

            List<ListenedItem> lístenedItems = new List<ListenedItem>()
            {
                new ListenedItem(){ Item=podcasts[0], Playlist=playlists[0], Timestamp = DateTime.Now.AddMinutes(-180*60-1000) },
                new ListenedItem(){ Item=advertisements[0], Playlist=playlists[0], Timestamp = DateTime.Now.AddMinutes(-180*60-1000+1) },
                new ListenedItem(){ Item=advertisements[1], Playlist=playlists[0], Timestamp = DateTime.Now.AddMinutes(-180*60-1000+2) },
                new ListenedItem(){ Item=podcasts[1], Playlist=playlists[0], Timestamp = DateTime.Now.AddMinutes(-180*60-2000) },
                new ListenedItem(){ Item=advertisements[2], Playlist=playlists[0], Timestamp = DateTime.Now.AddMinutes(-180*60-2000+2) },
                new ListenedItem(){ Item=podcasts[2], Playlist=playlists[0], Timestamp = DateTime.Now.AddMinutes(-180*60-3000) },
                new ListenedItem(){ Item=podcasts[3], Playlist=playlists[1], Timestamp = DateTime.Now.AddMinutes(-180*60-4000) },
                new ListenedItem(){ Item=advertisements[0], Playlist=playlists[1], Timestamp = DateTime.Now.AddMinutes(-180*60-4000+1) },
                new ListenedItem(){ Item=advertisements[2], Playlist=playlists[1], Timestamp = DateTime.Now.AddMinutes(-180*60-4000+2) },
                new ListenedItem(){ Item=podcasts[4], Playlist=playlists[2], Timestamp = DateTime.Now.AddMinutes(-180*60-5000) },
                new ListenedItem(){ Item=advertisements[2], Playlist=playlists[2], Timestamp = DateTime.Now.AddMinutes(-180*60-5000+2) },
                new ListenedItem(){ Item=podcasts[5], Playlist=playlists[2], Timestamp = DateTime.Now.AddMinutes(-180*60-6000) },
                new ListenedItem(){ Item=advertisements[1], Playlist=playlists[2], Timestamp = DateTime.Now.AddMinutes(-180*60-6000+2) },
                new ListenedItem(){ Item=podcasts[6], Playlist=playlists[3], Timestamp = DateTime.Now.AddMinutes(-180*60-7000) },
                new ListenedItem(){ Item=podcasts[7], Playlist=playlists[4], Timestamp = DateTime.Now.AddMinutes(-180*60-8000) },
                new ListenedItem(){ Item=advertisements[3], Playlist=playlists[4], Timestamp = DateTime.Now.AddMinutes(-180*60-8000+1) },
                new ListenedItem(){ Item=advertisements[4], Playlist=playlists[4], Timestamp = DateTime.Now.AddMinutes(-180*60-8000+2) },
                new ListenedItem(){ Item=podcasts[8], Playlist=playlists[5], Timestamp = DateTime.Now.AddMinutes(-180*60-9000) },
                new ListenedItem(){ Item=podcasts[9], Playlist=playlists[6], Timestamp = DateTime.Now.AddMinutes(-180*60-1500) },
                new ListenedItem(){ Item=advertisements[5], Playlist=playlists[6], Timestamp = DateTime.Now.AddMinutes(-180*60-1500+1) },
                new ListenedItem(){ Item=podcasts[1], Playlist=playlists[2], Timestamp = DateTime.Now.AddMinutes(-180*60-2500) },
                new ListenedItem(){ Item=podcasts[2], Playlist=playlists[3], Timestamp = DateTime.Now.AddMinutes(-180*60-3500) },
                new ListenedItem(){ Item=podcasts[3], Playlist=playlists[0], Timestamp = DateTime.Now.AddMinutes(-180*60-4500) },
                new ListenedItem(){ Item=advertisements[7], Playlist=playlists[0], Timestamp = DateTime.Now.AddMinutes(-180*60-4500+1) },
                new ListenedItem(){ Item=podcasts[4], Playlist=playlists[4], Timestamp = DateTime.Now.AddMinutes(-180*60-5500) },
                new ListenedItem(){ Item=podcasts[5], Playlist=playlists[5], Timestamp = DateTime.Now.AddMinutes(-180*60-6500) },
                new ListenedItem(){ Item=advertisements[7], Playlist=playlists[5], Timestamp = DateTime.Now.AddMinutes(-180*60-6500+1) },
                new ListenedItem(){ Item=podcasts[6], Playlist=playlists[7], Timestamp = DateTime.Now.AddMinutes(-180*60-7500) },
                new ListenedItem(){ Item=podcasts[7], Playlist=playlists[8], Timestamp = DateTime.Now.AddMinutes(-180*60-8500) },
                new ListenedItem(){ Item=podcasts[8], Playlist=playlists[9], Timestamp = DateTime.Now.AddMinutes(-180*60-9500) }
            };
            ListenedItems.AddRange(lístenedItems);
            SaveChanges();
        }
    }
}
