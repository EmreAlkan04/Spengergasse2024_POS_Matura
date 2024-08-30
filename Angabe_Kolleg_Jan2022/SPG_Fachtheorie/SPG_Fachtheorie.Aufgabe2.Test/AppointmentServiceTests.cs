using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using SPG_Fachtheorie.Aufgabe2;
using Microsoft.EntityFrameworkCore;
using SPG_Fachtheorie.Aufgabe2.Model;
using System.Security.Cryptography;

namespace SPG_Fachtheorie.Aufgabe2.Test
{
    [Collection("Sequential")]
    public class AppointmentServiceTests
    {
        /// <summary>
        /// Legt die Datenbank an und befüllt sie mit Musterdaten. Die Datenbank ist
        /// nach Ausführen des Tests ServiceClassSuccessTest in
        /// SPG_Fachtheorie\SPG_Fachtheorie.Aufgabe2.Test\bin\Debug\net6.0\Appointment.db
        /// und kann mit SQLite Manager, DBeaver, ... betrachtet werden.
        /// </summary>
        private AppointmentContext GetAppointmentContext()
        {
            var options = new DbContextOptionsBuilder()
                .UseSqlite("Data Source=Appointment.db")
                .Options;

            var db = new AppointmentContext(options);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            db.Seed();
            return db;
        }
        [Fact]
        public void ServiceClassSuccessTest()
        {
            using var db = GetAppointmentContext();
            Assert.True(db.Students.Count() > 0);
            var service = new AppointmentService(db);
        }
        [Fact]
        public void AskForAppointmentSuccessTest()
        {
            // Arrange
            using var db = GetAppointmentContext();
            var service = new AppointmentService(db);

            var student = db.Students.First(); 
            var appointment = db.Appointments.First();

            //Act
            var result = service.AskForAppointment(appointment.Id, student.Id, DateTime.Now);

            //Assert
            Assert.False(result);
        }

        [Fact]
        public void AskForAppointmentReturnsFalseIfNoOfferExists()
        {
            //Arrange
            using var db = GetAppointmentContext();
            var service = new AppointmentService(db);

            var offer = new Offer();
            var student = db.Students.First();

            //Act
            var askingappointment = service.AskForAppointment(offer.Id, student.Id, DateTime.Now);

            //Assert
            Assert.False(askingappointment);
        }

        [Fact]
        public void AskForAppointmentReturnsFalseIfOutOfDate()
        {
            //Arrange
            using var db = GetAppointmentContext();
            var service = new AppointmentService(db);

            var offer = db.Students.First();
            var student = db.Students.First();

            //Act
            var askingappointment = service.AskForAppointment(offer.Id, student.Id, DateTime.Now.AddYears(1000));

            //Assert
            Assert.False(askingappointment);
        }

        [Fact]
        public void ConfirmAppointmentSuccessTest()
        {
            //Arrange
            using var db = GetAppointmentContext();
            var service = new AppointmentService(db);
            var appointment = db.Appointments.First();

            //Act
            var result = service.ConfirmAppointment(appointment.Id);

            //Assert
            Assert.True(result);
        }
        [Fact]
        public void ConfirmAppointmentReturnsFalseIfStateIsInvalid()
        {
            using var db = GetAppointmentContext();
            var service = new AppointmentService(db);
            var appointment = db.Appointments.First();
            appointment.State = Model.AppointmentState.Confirmed;

            //Act
            var result = service.ConfirmAppointment(appointment.Id);

            //Assert
            Assert.False(result);
        }

        [Fact]
        public void CancelAppointmentStudentSuccessTest()
        {
            using var db = GetAppointmentContext();
            var service = new AppointmentService(db);
            var appointment = db.Appointments.FirstOrDefault(s => s.State == AppointmentState.AskedFor );
            var student = appointment.Student.Id;


            //Act
            var result = service.CancelAppointment(appointment.Id, student);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public void CancelAppointmentCoachSuccessTest()
        {
            using var db = GetAppointmentContext();
            var service = new AppointmentService(db);
            var appointment = db.Appointments
                .First(s =>  s.Student is Coach);
            appointment.State = AppointmentState.Confirmed;
            var student = appointment.Student.Id;


            //Act
            var result = service.CancelAppointment(appointment.Id, student);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public void ConfirmAppointmentStudentReturnsFalseIfStateIsInvalid()
        {
            using var db = GetAppointmentContext();
            var service = new AppointmentService(db);
            var appointment = db.Appointments.FirstOrDefault(s => s.State == AppointmentState.Cancelled );
            var student = appointment.Student.Id;


            //Act
            var result = service.CancelAppointment(appointment.Id, student);

            //Assert
            Assert.False(result);
        }

        [Fact]
        public void ConfirmAppointmentCoachReturnsFalseIfStateIsInvalid()
        {
            using var db = GetAppointmentContext();
            var service = new AppointmentService(db);
            var appointment = db.Appointments.FirstOrDefault(s => s.State == AppointmentState.TookPlace);
            var student = appointment.Student.Id;


            //Act
            var result = service.CancelAppointment(appointment.Id, student);

            //Assert
            Assert.False(result);
        }
    }
}
