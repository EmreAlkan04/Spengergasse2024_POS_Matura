using Microsoft.EntityFrameworkCore;
using SPG_Fachtheorie.Aufgabe2.Infrastructure;
using SPG_Fachtheorie.Aufgabe2.Model;
using SPG_Fachtheorie.Aufgabe2.Services;
using System;
using System.Linq;
using Xunit;

namespace SPG_Fachtheorie.Aufgabe2.Test
{
    [Collection("Sequential")]
    public class StickerServiceTests
    {
        /// <summary>
        /// Generates database in C:\Scratch\SPG_Fachtheorie.Aufgabe2.Test\Debug\net6.0\sticker.db
        /// </summary>
        private StickerContext GetEmptyDbContext()
        {
            var options = new DbContextOptionsBuilder()
                .UseSqlite(@"Data Source=sticker.db")
                .Options;

            var db = new StickerContext(options);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            return db;
        }

        private StickerContext GetSeededDbContext()
        {
            var db = GetEmptyDbContext();
            db.Seed();
            return db;
        }

        [Fact]
        public void CreateDatabaseTest()
        {
            using var db = GetSeededDbContext();
            db.ChangeTracker.Clear();
            Assert.True(db.StickerTypes.Any());
        }

        [Fact]
        public void HasPermissionReturnsFalseIfNumberplateIsInvalidTest()
        {
            // Arrange
            using var db = GetEmptyDbContext();
            var service = new StickerService(db);
            var sticker = new Sticker("kennzeichenname", new Customer("John", "Doe", "JohnDoe@Gmail.com", DateTime.Now), new StickerType("stickertypename", VehicleType.Motorcycle, 10, 5.8m), DateTime.Now, DateTime.Now, 5.8m);
            db.Add(sticker);
            db.SaveChanges();

            // Act
            var result = service.HasPermission("invalidnumberplate", DateTime.Now.AddDays(2), VehicleType.Motorcycle);

            // Assert
            Assert.False(result); 
        }

        [Fact]
        public void HasPermissionReturnsFalseIfCarTypeIsInvalidTest()
        {
            // Arrange
            using var db = GetEmptyDbContext();
            var service = new StickerService(db);
            var sticker = new Sticker("kennzeichenname", new Customer("John", "Doe", "JohnDoe@Gmail.com", DateTime.Now), new StickerType("stickertypename", VehicleType.Motorcycle, 10, 5.8m), DateTime.Now, DateTime.Now, 5.8m);
            db.Add(sticker);
            db.SaveChanges();

            // Act
            var result = service.HasPermission("kennzeichenname", DateTime.Now.AddDays(2), VehicleType.PassengerCar);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void HasPermissionReturnsFalseIfDateTimeNotInValidTimespanTest()
        {
            // Arrange
            using var db = GetEmptyDbContext();
            var service = new StickerService(db);
            var sticker = new Sticker("kennzeichenname", new Customer("John", "Doe", "JohnDoe@Gmail.com", DateTime.Now), new StickerType("stickertypename", VehicleType.Motorcycle, 10, 5.8m), DateTime.Now, DateTime.Now, 5.8m);
            db.Add(sticker);
            db.SaveChanges();

            // Act
            var result = service.HasPermission("kennzeichenname", DateTime.Now.AddDays(15), VehicleType.Motorcycle);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void HasPermissionReturnsTrueIfSuccessTest()
        {
            // Arrange
            using var db = GetEmptyDbContext();
            var service = new StickerService(db);
            var sticker = new Sticker("kennzeichenname", new Customer("John", "Doe", "JohnDoe@Gmail.com", DateTime.Now), new StickerType("stickertypename", VehicleType.Motorcycle, 10, 5.8m), DateTime.Now, DateTime.Now, 5.8m);
            db.Add(sticker);
            db.SaveChanges();

            // Act
            var result = service.HasPermission("kennzeichenname", DateTime.Now.AddDays(2), VehicleType.Motorcycle);

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// Vorgegebener Test für die Methode CalcSaleStatistics.
        /// Hier muss nichts geändert werden, sie ist zur Kontrolle deiner Implementierung
        /// von CalcSaleStatistics da.
        /// </summary>
        [Fact]
        public void CalcSaleStatisticsTest()
        {
            // SELECT st.Name AS StickerTypeName, SUM(s.Price) AS TotalRevenue
            // FROM Stickers s INNER JOIN StickerTypes st ON(s.StickerTypeId = st.Id)
            // WHERE strftime('%Y', s.PurchaseDate) == '2023'
            // GROUP BY st.Name;
            using var db = GetSeededDbContext();
            var service = new StickerService(db);
            var saleStatistics = service.CalcSaleStatistics(2023);
            Assert.True(saleStatistics.Count == 6);
            Assert.True(saleStatistics.First(s => s.StickerTypeName == "10-Tages-Vignette Motorrad").TotalRevenue == 11.6M);
            Assert.True(saleStatistics.First(s => s.StickerTypeName == "10-Tages-Vignette PKW").TotalRevenue == 9.9M);
            Assert.True(saleStatistics.First(s => s.StickerTypeName == "2-Monats-Vignette Motorrad").TotalRevenue == 29.0M);
            Assert.True(saleStatistics.First(s => s.StickerTypeName == "2-Monats-Vignette PKW").TotalRevenue == 116.0M);
            Assert.True(saleStatistics.First(s => s.StickerTypeName == "Jahres-Vignette Motorrad").TotalRevenue == 38.2M);
            Assert.True(saleStatistics.First(s => s.StickerTypeName == "Jahres-Vignette PKW").TotalRevenue == 192.8M);
        }
    }
}