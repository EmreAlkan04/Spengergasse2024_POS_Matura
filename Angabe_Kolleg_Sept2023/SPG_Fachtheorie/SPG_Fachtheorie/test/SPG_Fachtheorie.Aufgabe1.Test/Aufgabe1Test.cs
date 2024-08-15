using Microsoft.EntityFrameworkCore;
using SPG_Fachtheorie.Aufgabe1.Infrastructure;
using SPG_Fachtheorie.Aufgabe1.Model;
using System;
using System.Linq;
using Xunit;

namespace SPG_Fachtheorie.Aufgabe1.Test
{
    [Collection("Sequential")]
    public class Aufgabe1Test
    {
        /// <summary>
        /// Generates database in C:\Scratch\SPG_Fachtheorie.Aufgabe1.Test\Debug\net6.0\appointments.db
        /// </summary>
        private AppointmentContext GetEmptyDbContext()
        {
            var options = new DbContextOptionsBuilder()
                .UseSqlite(@"Data Source=appointments.db")
                .Options;

            var db = new AppointmentContext(options);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            return db;
        }

        [Fact]
        public void CreateDatabaseTest()
        {
            using var db = GetEmptyDbContext();
        }

        [Fact]
        public void AddPatientSuccessTest()
        {
            //Arrange
            using var db = GetEmptyDbContext();
            var patient = new Patient("boris", "johnson", new Address("10 Downing Street", 2, "SW1A 2AA"), "ja@gmail", "+43500");

            //Act
            db.Patients.Add(patient);
            db.SaveChanges();

            //Assert
            Assert.Contains(patient, db.Patients);
        }

        [Fact]
        public void AddAppointmentSuccessTest()
        {
            //Arrange
            using var db = GetEmptyDbContext();
            var appointment = new Appointment(DateTime.Now, TimeSpan.FromHours(2), new Patient("boris", "johnson",
                new Address("10 Downing Street", 2, "SW1A 2AA"), "ja@gmail", "+43500"), DateTime.Now, new AppointmentState(DateTime.Now, "idk"));

            //Act
            db.Appointments.Add(appointment);
            db.SaveChanges();

            //Assert
            Assert.Contains(appointment, db.Appointments);
        }

        [Fact]
        public void ChangeAppointmentStateToConfirmedSuccessTest()
        {
            //Arrange
            using var db = GetEmptyDbContext();
            var appointment = new Appointment(DateTime.Now, TimeSpan.FromHours(2), new Patient("boris", "johnson",
                new Address("10 Downing Street", 2, "SW1A 2AA"), "ja@gmail", "+43500"), DateTime.Now, new AppointmentState(DateTime.Now, "idk"));

            //Act
            db.Appointments.Add(appointment);
            db.SaveChanges();
            db.ChangeTracker.Clear();


            //Assert
            Assert.True(db.AppointmentStates.Count()== 1);
        }
    }
}