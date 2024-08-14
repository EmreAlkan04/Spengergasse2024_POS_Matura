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
            // Arrange
            using var db = GetEmptyDbContext();
            var service = new EventService(db);
            int invalidGuestId = -1; // Beispiel für eine ungültige guestId
            int validContingentId = 1; // Stellen Sie sicher, dass dies ein gültiger Wert ist, der in Ihrer Seed-Methode definiert ist
            int pax = 1;
            DateTime reservationDate = DateTime.Now.AddDays(15); // Ein gültiges Datum in der Zukunft
            

            // Act & Assert
            var exception = Assert.Throws<EventServiceException>(() => service.CreateReservation(invalidGuestId, validContingentId, pax, reservationDate));
            Assert.True(exception.Message == "Invalid guest id");
        }

        [Fact]
        public void ShouldThrowException_WhenNoTickets()
        {
            //Arrange
            using var db = GetEmptyDbContext();
            db.Seed();
            var service = new EventService(db);
            int validGuestId = 1; // Stellen Sie sicher, dass dies ein gültiger Wert ist, der in Ihrer Seed-Methode definiert ist
            int validContingentId = 1; // Stellen Sie sicher, dass dies ein gültiger Wert ist, der in Ihrer Seed-Methode definiert ist
            int pax = 1; // Die Anzahl der angeforderten Tickets
            DateTime reservationDate = DateTime.Now.AddDays(15); // Ein gültiges Datum in der Zukunft
            var contingent = db.Contingents.Where(x => x.Id == validContingentId);
            contingent.First().AvailableTickets = 0;
            



            //Act & Assert
            var exception = Assert.Throws<EventServiceException>(() => service.CreateReservation(validGuestId, validContingentId, pax, reservationDate));
            Assert.True(exception.Message == "Show is sold out"); // Stellen Sie sicher, dass Ihre CreateReservation-Methode diese spezifische Nachricht ausgibt, wenn keine Tickets verfügbar sind.
        }

        [Fact]
        public void ShouldThrowException_WhenGuestHasTicketForShowAndContingentReserved()
        {
            using var db = GetEmptyDbContext();
            db.Seed();
            var service = new EventService(db);
            int validGuestId = 1; // Stellen Sie sicher, dass dies ein gültiger Wert ist, der in Ihrer Seed-Methode definiert ist
            int validContingentId = 1; // Stellen Sie sicher, dass dies ein gültiger Wert ist, der in Ihrer Seed-Methode definiert ist
            int pax = 1; // Die Anzahl der angeforderten Tickets
            DateTime reservationDate = DateTime.Now.AddDays(15); // Ein gültiges Datum in der Zukunft

            //Act
            var addedTicket = service.CreateReservation(validGuestId, validContingentId, pax, reservationDate);
            var addSameTicketAgain = Assert.Throws<EventServiceException>(() =>  service.CreateReservation(validGuestId, validContingentId, pax, reservationDate));

            //Assert
            Assert.True(addSameTicketAgain.Message == "A reservation or purchase has already been made for this contingent");
        }

        [Fact]
        public void ShouldThrowException_WhenShowDateNot14DaysInFuture()
        {
            using var db = GetEmptyDbContext();
            db.Seed();
            var service = new EventService(db);
            int validGuestId = 1; // Stellen Sie sicher, dass dies ein gültiger Wert ist, der in Ihrer Seed-Methode definiert ist
            int validContingentId = 1; // Stellen Sie sicher, dass dies ein gültiger Wert ist, der in Ihrer Seed-Methode definiert ist
            int pax = 1; // Die Anzahl der angeforderten Tickets
            DateTime reservationDate = DateTime.Now.AddDays(9); // Ein ungültiges Datum in der Zukunft

            //Act & Assert
            var exception = Assert.Throws<EventServiceException>(() => service.CreateReservation(validGuestId, validContingentId, pax, reservationDate));
            Assert.True(exception.Message == "The show is too close in time"); // Stellen Sie sicher, dass Ihre CreateReservation-Methode diese spezifische Nachricht ausgibt, wenn die Show weniger als 14 Tage in der Zukunft liegt.
        }

        [Fact]
        public void ShouldReturnTicketId_WhenParametersAreValid()
        {
            using var db = GetEmptyDbContext();
            db.Seed();
            var service = new EventService(db);
            int validGuestId = 1; // Stellen Sie sicher, dass dies ein gültiger Wert ist, der in Ihrer Seed-Methode definiert ist
            int validContingentId = 1; // Stellen Sie sicher, dass dies ein gültiger Wert ist, der in Ihrer Seed-Methode definiert ist
            int pax = 2; // Die Anzahl der angeforderten Tickets
            DateTime reservationDate = DateTime.Now.AddDays(20); // Ein gültiges Datum in der Zukunft

            //Act
            var addTicket = service.CreateReservation(validGuestId, validContingentId, pax, reservationDate);
            var searchTicket = db.Tickets.Where(x => x.Guest.Id == validGuestId);
            //Assert
            Assert.NotNull(searchTicket);
            Assert.Equal(addTicket, validGuestId);
            
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