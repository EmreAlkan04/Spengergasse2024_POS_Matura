using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using SPG_Fachtheorie.Aufgabe2.Infrastructure;
using SPG_Fachtheorie.Aufgabe2.Model;
using SPG_Fachtheorie.Aufgabe2.Services;
using System;
using System.Linq;
using Xunit;

namespace SPG_Fachtheorie.Aufgabe2.Test
{
    [Collection("Sequential")]
    public class EventServiceTests 
    {
        /// <summary>
        /// Generates database in C:\Scratch\Aufgabe2_Test\Debug\net6.0\event.db
        /// </summary>
        private EventContext GetEmptyDbContext()
        {
            var options = new DbContextOptionsBuilder()
                .UseSqlite(@"Data Source=event.db")
                .Options;

            var db = new EventContext(options);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            return db;
        }

        [Fact]
        public void CreateDatabaseTest()
        {
            using var db = GetEmptyDbContext();
            db.Seed();
            db.ChangeTracker.Clear();
            Assert.True(db.Tickets.Any());
        }

        [Fact]
        public void ShouldThrowException_WhenInvalidGuestId()
        {
            //Arrange
            using var db = GetEmptyDbContext();
            var service = new EventService(db);
            var guest = new Guest("emre", "alkan", DateTime.Now.AddYears(-20));
            var contigent = new Contingent(new Show(new Event("eventname"), DateTime.Now.AddDays(90)), ContingentType.Floor, 100);
            db.Contingents.Add(contigent);
            db.Guests.Add(guest);
            db.SaveChanges();

            //Act
            //var result = service.CreateReservation(-3, contigent.Id, 2, DateTime.Now);

            //Assert
            var ex = Assert.Throws<EventServiceException>(() => service.CreateReservation(-3, contigent.Id, 2, DateTime.Now));
            Assert.True(ex.Message == "Invalid guest id");
            db.ChangeTracker.Clear();

        }

        [Fact]
        public void ShouldThrowException_WhenNoTickets()
        {
            using var db = GetEmptyDbContext();
            var service = new EventService(db);
            var guest = new Guest("emre", "alkan", DateTime.Now.AddYears(-20));
            var contigent = new Contingent(new Show(new Event("eventname"), DateTime.Now.AddDays(90)), ContingentType.Floor, 5);
            db.Contingents.Add(contigent);
            db.Guests.Add(guest);
            db.SaveChanges();

            //Act
            //var result = service.CreateReservation(-3, contigent.Id, 2, DateTime.Now);

            //Assert
            var ex = Assert.Throws<EventServiceException>(() => service.CreateReservation(guest.Id, contigent.Id, 6, DateTime.Now));
            Assert.True(ex.Message == "Show is sold out");
            db.ChangeTracker.Clear();
        }

        [Fact]
        public void ShouldThrowException_WhenGuestHasTicketForShowAndContingentReserved()
        {
            using var db = GetEmptyDbContext();
            var service = new EventService(db);
            var guest = new Guest("emre", "alkan", DateTime.Now.AddYears(-20));
            var contigent = new Contingent(new Show(new Event("eventname"), DateTime.Now.AddDays(90)), ContingentType.Floor, 100);
            db.Contingents.Add(contigent);
            db.Guests.Add(guest);
            db.SaveChanges();

            //Act
            var result = service.CreateReservation(guest.Id, contigent.Id, 6, DateTime.Now);

            //Assert
            var ex = Assert.Throws<EventServiceException>(() => service.CreateReservation(guest.Id, contigent.Id, 6, DateTime.Now));
            Assert.True(ex.Message == "A reservation or purchase has already been made for this contingent");
            db.ChangeTracker.Clear();
        }

        [Fact]
        public void ShouldThrowException_WhenShowDateNot14DaysInFuture()
        {
            using var db = GetEmptyDbContext();
            var service = new EventService(db);
            var guest = new Guest("emre", "alkan", DateTime.Now.AddYears(-20));
            var contigent = new Contingent(new Show(new Event("eventname"), DateTime.Now.AddDays(3)), ContingentType.Floor, 100);
            db.Contingents.Add(contigent);
            db.Guests.Add(guest);
            db.SaveChanges();

            //Act
            //var result = service.CreateReservation(guest.Id, contigent.Id, 6, DateTime.Now);

            //Assert
            var ex = Assert.Throws<EventServiceException>(() => service.CreateReservation(guest.Id, contigent.Id, 6, DateTime.Now));
            Assert.True(ex.Message == "The show is too close in time");
            db.ChangeTracker.Clear();
        }

        [Fact]
        public void ShouldReturnTicketId_WhenParametersAreValid()
        {
            using var db = GetEmptyDbContext();
            var service = new EventService(db);
            var guest = new Guest("emre", "alkan", DateTime.Now.AddYears(-20));
            var contigent = new Contingent(new Show(new Event("eventname"), DateTime.Now.AddDays(20)), ContingentType.Floor, 100);
            db.Contingents.Add(contigent);
            db.Guests.Add(guest);
            db.SaveChanges();

            //Act
            var result = service.CreateReservation(guest.Id, contigent.Id, 6, DateTime.Now);
            var ticket = db.Tickets.Find(result) ?? throw new EventServiceException();


            //Assert
            //var ex = Assert.Throws<EventServiceException>(() => service.CreateReservation(guest.Id, contigent.Id, 6, DateTime.Now));
            //Assert.True(ex.Message == "The show is too close in time");
            Assert.NotNull(ticket);
            db.ChangeTracker.Clear();
        }

        /// <summary>
        /// SELECT t.TicketState, SUM(t.Pax+1) AS Count
        /// FROM Tickets t
        /// WHERE t.ContingentId = 68
        /// GROUP BY t.TicketState;
        /// </summary>
        [Fact]
        public void CalcContingentStatisticsTest()
        {
            using var db = GetEmptyDbContext();
            db.Seed();
            var service = new EventService(db);
            var statistics = service.CalcContingentStatistics(68);
            Assert.True(statistics.ReservedTickets == 23);
            Assert.True(statistics.SoldTickets == 17);
        }
    }
}