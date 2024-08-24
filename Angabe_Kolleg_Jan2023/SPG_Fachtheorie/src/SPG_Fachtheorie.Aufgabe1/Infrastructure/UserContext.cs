using Microsoft.EntityFrameworkCore;
using SPG_Fachtheorie.Aufgabe1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPG_Fachtheorie.Aufgabe1.Infrastructure
{
    public class UserContext : DbContext
    {

        public DbSet<User> Users => Set<User>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Favorite> Favorites => Set<Favorite>();
        public DbSet<Podcast> Podcasts => Set<Podcast>();
        public DbSet<RadioStation> RadioStations => Set<RadioStation>();
        public DbSet<Rating> Ratings => Set<Rating>();
        public DbSet<UserStandard> UserStandards => Set<UserStandard>();
        public DbSet<UserPremium> UserPremiums => Set<UserPremium>();
        
        public UserContext(DbContextOptions options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => u.Id);

            modelBuilder.Entity<UserStandard>().HasBaseType<User>();    
            modelBuilder.Entity<UserPremium>().HasBaseType<User>();

            modelBuilder.Entity<Podcast>().HasKey(p => p.Id);
            modelBuilder.Entity<RadioStation>().HasKey(r => r.Id);
            modelBuilder.Entity<Category>().HasKey(c => c.Id);

            modelBuilder.Entity<Favorite>().HasKey(f => f.Id);

        }
    }
}
