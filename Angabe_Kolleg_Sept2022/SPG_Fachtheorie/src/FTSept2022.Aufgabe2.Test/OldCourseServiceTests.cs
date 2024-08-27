using FTSept2022.Aufgabe2.Domain;
using FTSept2022.Aufgabe2.Infrastructure;
using FTSept2022.Aufgabe2.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Linq;
using Xunit;

namespace FTSept2022.Aufgabe2.Test
{
    public class CourseServiceTests
    {
        private UniContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder()
                .UseSqlite(@"Data Source=Uni.db")
                .Options;

            var db = new UniContext(options);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            db.Seed();
            return db;
        }

        [Fact]
        public void CreateDatabaseTest()
        {
            using var db = GetDbContext();
            Assert.True(db.Students.Count() > 0);
        }
        [Fact()]
        public void SubscribeCourse_Invalid_StudentRegistrationNumber()
        {
            //Arrange
            using var db = GetDbContext();
            var service = new CourseService(db);
            var studentId = "-21421414";
            var courseId = 1;

            //Act
            var result = service.SubscribeCourse(studentId, courseId);

            //Assert
            Assert.False(result);

            //throw new NotImplementedException("Noch keine Implementierung vorhanden");
        }
        [Fact()]
        public void SubscribeCourse_Invalid_CourseId()
        {
            //Arrange
            using var db = GetDbContext();
            var service = new CourseService(db);
            var studentId = db.Students.First().RegistrationNumber;
            var courseId = -1;

            //Act
            var result = service.SubscribeCourse(studentId, courseId);
            Assert.False(result);
            //throw new NotImplementedException("Noch keine Implementierung vorhanden");
        }
        [Fact()]
        public void SubscribeCourse_Student_Already_Enrolled()
        {
            //Arrange
            using var db = GetDbContext();
            var service = new CourseService(db);
            var studentId = db.Students.FirstOrDefault(s => s.Enrollments.Any(s => s.CourseId == 1)).RegistrationNumber;
            var courseId = 1;
            //Act
            var result = service.SubscribeCourse(studentId, courseId);
            Assert.False(result);
            //throw new NotImplementedException("Noch keine Implementierung vorhanden");
        }
        [Fact()]
        public void SubscribeCourse_MaxStudent_Exceeded()
        {
            //Arrange
            using var db = GetDbContext();
            var service = new CourseService(db);
            var studentId = db.Students.First().RegistrationNumber;
            var nextStudentId = db.Students.Skip(1).First().RegistrationNumber;
            var thirdStudentId = db.Students.Skip(2).First().RegistrationNumber;
            var courseId = 1;

            //Act
            var result1 = service.SubscribeCourse(studentId, courseId);
            var result2 = service.SubscribeCourse(nextStudentId, courseId);
            var result3 = service.SubscribeCourse(thirdStudentId, courseId);

            Assert.False(result3);
            //throw new NotImplementedException("Noch keine Implementierung vorhanden");
        }
        [Fact()]
        public void SubscribeCourse_Success()
        {
            //Arrange
            using var db = GetDbContext();
            var service = new CourseService(db);
            var studentId = db.Students.First().RegistrationNumber;
            var courseId = db.Courses.First();
            courseId.MaxStudents += 5;
            var IdofCourse = courseId.Id;

            //Act
            var result = service.SubscribeCourse(studentId, IdofCourse);

            Assert.True(result);
            //throw new NotImplementedException("Noch keine Implementierung vorhanden");
        }
    }
}