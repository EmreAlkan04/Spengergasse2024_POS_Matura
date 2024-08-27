using AspShowcase.Application.Commands;
using AspShowcase.Application.Dtos;
using AspShowcase.Application.Infrastructure;
using AspShowcase.Application.Models;
using AspShowcase.Application.Services;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace AspShowcase.Tests
{
    // Run tests sequentially, not in parallel because they share the same database.
    [Collection("Sequential")]
    public class TaskServiceTests
    {
        private AspShowcaseContext GetDbContext()
        {
            // Do not test against the real database!!
            var opt = new DbContextOptionsBuilder<AspShowcaseContext>()
                .UseSqlite("DataSource=test.db")
                .Options;
            var db = new AspShowcaseContext(opt);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            return db;
        }

        public IMapper GetMapper()
        {
            var configuration = new MapperConfiguration(cfg => cfg.AddMaps(typeof(MappingProfile)));
            return new Mapper(configuration);
        }

        private void GenerateFixtures(AspShowcaseContext db)
        {
            // Arrange
            // using means that the db connection is closed after the block.
            var team = new Team(name: "Name", schoolclass: "6AAIF")
            { Guid = new Guid("00000000-0000-0000-0000-000000000001") };
            db.Teams.Add(team);
            db.SaveChanges();

            var teacher = new Teacher(
                firstname: "Max", lastname: "Mustermann", email: "mail")
            { Guid = new Guid("00000000-0000-0000-0001-000000000001") };
            db.Teachers.Add(teacher);
            db.SaveChanges();
        }
        [Fact]
        public void AddTaskSuccessTest()
        {
            // Arrange
            using var db = GetDbContext();
            GenerateFixtures(db);
            var cmd = new NewTaskCmd(
                Subject: "POS", Title: "Title",
                TeamGuid: new Guid("00000000-0000-0000-0000-000000000001"),     // GUID from fixtures
                TeacherGuid: new Guid("00000000-0000-0000-0001-000000000001"),  // GUID from fixtures
                ExpirationDate: new DateTime(2024, 4, 1),
                MaxPoints: 24);
            var clock = new FakeClock(new DateTime(2024, 3, 1));
            var mapper = GetMapper();
            var service = new TaskService(db, clock, mapper);

            // Act
            var guid = service.AddTask(cmd);

            // Assert
            db.ChangeTracker.Clear();
            Assert.True(db.Tasks.Any(t => t.Guid == guid));
        }

        [Fact]
        public void AddTaskThrowsServiceExceptionIfTaskIsExpiredTest()
        {
            // Arrange
            using var db = GetDbContext();
            GenerateFixtures(db);

            var cmd = new NewTaskCmd(
                Subject: "POS", Title: "Title",
                TeamGuid: new Guid("00000000-0000-0000-0000-000000000001"),
                TeacherGuid: new Guid("00000000-0000-0000-0001-000000000001"),
                ExpirationDate: new DateTime(2024, 2, 1),
                MaxPoints: 24);
            var clock = new FakeClock(new DateTime(2024, 3, 1));
            var mapper = GetMapper();
            var service = new TaskService(db, clock, mapper);

            // Act & Assert
            var ex = Assert.Throws<ServiceException>(() => service.AddTask(cmd));
            Assert.Equal("Expiration date must be in the future.", ex.Message);
        }

        [Fact]
        public void AddTaskThrowsServiceExceptionIfTaskAlreadyExistsTest()
        {
            // Arrange
            using var db = GetDbContext();
            GenerateFixtures(db);
            var team = db.Teams.First(t => t.Guid == new Guid("00000000-0000-0000-0000-000000000001"));
            var teacher = db.Teachers.First(t => t.Guid == new Guid("00000000-0000-0000-0001-000000000001"));

            var task = new Task(
                subject: "POS", title: "Title",
                team: team,
                teacher: teacher,
                expirationDate: new DateTime(2024, 4, 1), maxPoints: 24);
            db.Tasks.Add(task);
            db.SaveChanges();

            var cmd = new NewTaskCmd(
                Subject: "POS", Title: "Title",
                TeamGuid: team.Guid,
                TeacherGuid: teacher.Guid,
                ExpirationDate: new DateTime(2024, 5, 1),
                MaxPoints: 24);
            var clock = new FakeClock(new DateTime(2024, 3, 1));
            var mapper = GetMapper();
            var service = new TaskService(db, clock, mapper);

            // Act & Assert
            var ex = Assert.Throws<ServiceException>(() => service.AddTask(cmd));
            Assert.Equal("Task title must be unique per team", ex.Message);
        }
    }
}
