using SPG_Fachtheorie.Aufgabe2.Infrastructure;
using SPG_Fachtheorie.Aufgabe2.Services;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;
using SPG_Fachtheorie.Aufgabe2.Domain;
using System.Security.Cryptography.X509Certificates;
using System.Linq;

namespace SPG_Fachtheorie.Aufgabe2.Test
{
    public class DatabaseContextTest
    {
        private PodcastContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder()
                .UseSqlite(@"Data Source=Podcast.db")
                .Options;

            var db = new PodcastContext(options);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            db.Seed();
            return db;
        }


        [Fact()]
        public void CalcTotalCosts_Invalid_CustomerId()
        {
            //Arrange
            using var db = GetDbContext();
            var service = new PodcastService(db);
            //var CustomerId = new Customer("josh", "josh", "josh", DateTime.Now, 1, 1, new Admin(), new System.Collections.Generic.List<Advertisement>()) { Id = -1 };
            var CustomerId = new Customer() { Id = -1 };
            var begin = new DateTime(2021, 1, 1);
            var end = new DateTime(2021, 1, 2);

            //Act
            var failed = service.CalcTotalCosts(CustomerId.Id, begin, end);

            //Assert
            Assert.False(failed);

            //throw new NotImplementedException("Noch keine Implementierung vorhanden");
        }
        [Fact()]
        public void CalcTotalCosts_TotalCosts_Already_Calculated()
        {
            using var db = GetDbContext();
            var service = new PodcastService(db);
            //var CustomerId = new Customer();
            var CustomerId = new Customer() { Id = 1, TotalCosts = 3.2m };
            var begin = new DateTime(2021, 1, 1);
            var end = new DateTime(2021, 1, 2);

            //Act
            var failed = service.CalcTotalCosts(CustomerId.Id, begin, end);

            //Assert
            Assert.False(failed);
            //throw new NotImplementedException("Noch keine Implementierung vorhanden");
        }
        [Fact()]
        public void CalcTotalCosts_Invalid_TimePeriod()
        {

            using var db = GetDbContext();
            var service = new PodcastService(db);
            //var CustomerId = new Customer();
            var CustomerId = new Customer() { Id = 1 };
            var begin = new DateTime(2021, 3, 6);
            var end = new DateTime(2021, 1, 1);

            //Act
            var failed = service.CalcTotalCosts(CustomerId.Id, begin, end);

            //Assert
            Assert.False(failed);
            //throw new NotImplementedException("Noch keine Implementierung vorhanden");
        }
        [Fact()]
        public void CalcTotalCosts_No_Advertisements()
        {
            using var db = GetDbContext();
            var service = new PodcastService(db);
            //var CustomerId = new Customer();
            var CustomerId = new Customer() { Id = 1, TotalCosts = null};
            var Adverb = new Advertisement() { Id = 1, CustomerId = 1, CostsPerPlay = 0};
            var begin = new DateTime(2021, 1, 1);
            var end = new DateTime(2021, 1, 2);

            //Act
            var failed = service.CalcTotalCosts(CustomerId.Id, begin, end);

            //Assert
            Assert.False(failed);
            //throw new NotImplementedException("Noch keine Implementierung vorhanden");
        }
        [Fact()]
        public void CalcTotalCosts_Success()
        {
            // Arrange
            using var db = GetDbContext();
            var service = new PodcastService(db);
            var customer = db.Customers.First();
            customer.TotalCosts = null;
            int cutomerId = customer.Id;
            var begin = new DateTime(2021, 1, 1);
            var end = new DateTime(2022, 6, 2);

            // Act
            var result = service.CalcTotalCosts(cutomerId, begin, end);

            // Assert
            Assert.True(result);
            //throw new NotImplementedException("Noch keine Implementierung vorhanden");
        }

        [Fact()]
        public void CalcQuantityAdditionalAds_Success()
        {
            // Arrange
            using var db = GetDbContext();
            var service = new PodcastService(db);
            var playlist = db.Playlists.First(s => s.ListenedItems.Count() < 10).Id;

            // Act
            var result = service.CalcQuantityAdditionalAds(playlist);

            // Assert
            Assert.True(result > 0);
            //throw new NotImplementedException("Noch keine Implementierung vorhanden");
        }

        [Fact()]
        public void AddPostionForAd_Success()
        {
            // Arrange
            using var db = GetDbContext();
            var service = new PodcastService(db);
            var podcast = 13;
            var position = 1;

            // Act
            var result = service.AddPostionForAd(podcast, position);

            // Assert
            Assert.True(result);
            //throw new NotImplementedException("Noch keine Implementierung vorhanden");
        }
    }
}