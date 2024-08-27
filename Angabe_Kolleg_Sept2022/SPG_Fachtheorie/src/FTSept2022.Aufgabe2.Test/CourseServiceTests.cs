using FTSept2022.Aufgabe2.Domain;
using FTSept2022.Aufgabe2.Infrastructure;
using FTSept2022.Aufgabe2.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
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
            using var db = GetDbContext();
            var service = new CourseService(db);
            string studentRegistrationNumber = "ifeowjfeiowhfiefiowefhioehfwiohfw";
            int courseId = db.Courses.First().Id;

            var result = service.SubscribeCourse(studentRegistrationNumber, courseId);
            Assert.False(result);
        }
        [Fact()]
        public void SubscribeCourse_Invalid_CourseId()
        {
            using var db = GetDbContext();
            var service = new CourseService(db);
            var studentRegistrationNumber = db.Students.First().RegistrationNumber;
            int courseId = -328;

            var result = service.SubscribeCourse(studentRegistrationNumber, courseId);
            Assert.False(result);
        }

        //Der Test SubscribeCourse_Student_Already_Enrolled beweist, dass der Wert false zurückgegeben
        //wird, wenn der Studierende bereits zu diesem Course angemeldet ist
        [Fact()]
        public void SubscribeCourse_Student_Already_Enrolled()
        {
            using var db = GetDbContext();
            var service = new CourseService(db);
            var student = db.Courses.First().Enrollments.First().StudentNavigation;
            var course = student.Enrollments.First().CourseNavigation.Id;
            var studentId = student.RegistrationNumber;

            var result = service.SubscribeCourse(studentId, course);
            Assert.False(result);
        }
        [Fact()]
        public void SubscribeCourse_MaxStudent_Exceeded()
        {
            using var db = GetDbContext();
            var service = new CourseService(db);
            var studentRegistrationNumber = db.Students.First().RegistrationNumber;
            var courseId = db.Courses.Where(s => s.MaxStudents < s.Enrollments.Count()).First().Id;
            var result = service.SubscribeCourse(studentRegistrationNumber, courseId);
            Assert.True(result);
        }
        [Fact()]
        public void SubscribeCourse_Success()
        {
            using var db = GetDbContext();
            var service = new CourseService(db);
            var studentRegistrationNumber = db.Students.First().RegistrationNumber;
            var course = db.Courses.First();
            course.MaxStudents += 5;
            var courseId = course.Id;

            var result = service.SubscribeCourse(studentRegistrationNumber, courseId);
            Assert.True(result);
        }
    }
}