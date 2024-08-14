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
        private AppointmentContext GetEmptyDbContext()
        {
            var options = new DbContextOptionsBuilder()
                .UseSqlite(@"Data Source=cash.db")
                .Options;

            var db = new AppointmentContext(options);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            return db;
        }

        // Creates an empty DB in C:\Scratch\Aufgabe1_Test\Debug\net6.0\cash.db
        [Fact]
        public void CreateDatabaseTest()
        {
            using var db = GetEmptyDbContext();
        }

        [Fact]
        public void AddCashierSuccessTest()
        {
            //Arrange
            using var db = GetEmptyDbContext();
            var cashier = new Cashier(233, "robert", "hoffman", new Address("neusidlersee", "wien", "2332"), "böla");

            //Act
            db.Cashiers.Add(cashier);
            db.SaveChanges();
           

            //Assert
            Assert.NotNull(db.Cashiers);

        }

        [Fact]
        public void AddPaymentSuccessTest()
        {
            //Arrange
            using var db = GetEmptyDbContext();
            //var payment = new Payment(10, DateTime.Now, PaymentType.CreditCard, new CashDesk(2));
            var cashier = new Cashier(233, "robert", "hoffman", new Address("neusidlersee", "wien", "2332"), "blöa");
            var cashDesk = new CashDesk(2);
            var payment = new Payment(10, DateTime.Now, PaymentType.CreditCard, cashDesk, cashier);

            //Act
            db.Payments.Add(payment);
            db.SaveChanges();

            //Assert
            Assert.NotNull(db.Payments);
        }

        [Fact]
        public void EmployeeDiscriminatorSuccessTest()
        {
            //Arrange
            using var db = GetEmptyDbContext();
            var cashier = new Cashier(233, "robert", "hoffman", new Address("neusidlersee", "wien", "2332"), "blöa");

            //Act
            db.Cashiers.Add(cashier);
            db.SaveChanges();

            //Assert
            var discriminatorValue = db.Employees
                 .Where(e => e.RegistrationNumber == cashier.RegistrationNumber)
                 .Select(e => EF.Property<string>(e, "Type"))
                 .FirstOrDefault();

            Assert.NotNull(discriminatorValue);
            Assert.Equal("Cashier", discriminatorValue);

        }
    }
}