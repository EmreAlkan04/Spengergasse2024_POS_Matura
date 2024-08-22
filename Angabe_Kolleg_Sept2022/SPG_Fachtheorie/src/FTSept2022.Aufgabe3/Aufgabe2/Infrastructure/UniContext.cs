using Bogus;
using FTSept2022.Aufgabe2.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTSept2022.Aufgabe2.Infrastructure
{
    public class UniContext : DbContext
    {
        public DbSet<Professor> Professors => Set<Professor>();
        public DbSet<Student> Students => Set<Student>();
        public DbSet<Exam> Exams => Set<Exam>();
        public DbSet<Enrollment> Enrollments => Set<Enrollment>();
        public DbSet<Course> Courses => Set<Course>();
        public DbSet<AllowedExamDate> AllowedExamDates => Set<AllowedExamDate>();

        public UniContext()
        { }
        public UniContext(DbContextOptions options)
        : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite(@"Data Source=Uni.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { }

        public void Seed()
        {
            var faker = new Faker();
            Randomizer.Seed = new Random(840);
       
            var students = new Faker<Student>("de").Rules((f, s) =>
            {
                s.BirthDate = f.Date.Between(new DateTime(1980, 01, 01), new DateTime(1990, 01, 01));
                s.FirstName = f.Name.FirstName();
                s.LastName = f.Name.LastName();
                s.RegistrationNumber = $"M-{f.Random.Int(111111, 999999)}";
            })
            .Generate(20)
            .ToList();
            Students.AddRange(students);
            SaveChanges();

            var professors = new Faker<Professor>("de").Rules((f, s) =>
            {
                s.FirstName = f.Name.FirstName();
                s.LastName = f.Name.LastName();
            })
            .Generate(5)
            .ToList();
            Professors.AddRange(professors);
            SaveChanges();

            var courses = new Faker<Course>("de").Rules((f, s) =>
            {
                s.Title = f.Commerce.ProductName();
                s.Ects = f.Random.Int(1, 7);
                s.ProfessorNavigation = f.Random.ListItem(professors);
            })
            .Generate(12)
            .ToList();
            Courses.AddRange(courses);
            SaveChanges();

            var allowedExamDates = new Faker<AllowedExamDate>("de").Rules((f, s) =>
            {
                s.From = f.Date.Between(DateTime.Now.AddMonths(1), DateTime.Now.AddMonths(14));
                s.To = f.Date.Between(s.From.AddDays(f.Random.Int(10, 30)), s.To.AddDays(f.Random.Int(10, 30)));
                s.CourseNavigation= f.Random.ListItem(courses);
            })
            .Generate(5)
            .ToList();
            AllowedExamDates.AddRange(allowedExamDates);
            SaveChanges();

            List<Enrollment> enrollments = new List<Enrollment>()
            {
                new Enrollment(){ CourseNavigation=courses[0], StudentNavigation=students[0] },
                new Enrollment(){ CourseNavigation=courses[1], StudentNavigation=students[0] },
                new Enrollment(){ CourseNavigation=courses[2], StudentNavigation=students[0] },
                new Enrollment(){ CourseNavigation=courses[3], StudentNavigation=students[0] },
                new Enrollment(){ CourseNavigation=courses[4], StudentNavigation=students[1] },
                new Enrollment(){ CourseNavigation=courses[5], StudentNavigation=students[1] },
                new Enrollment(){ CourseNavigation=courses[6], StudentNavigation=students[2] },
                new Enrollment(){ CourseNavigation=courses[7], StudentNavigation=students[2] },
                new Enrollment(){ CourseNavigation=courses[8], StudentNavigation=students[3] },
                new Enrollment(){ CourseNavigation=courses[9], StudentNavigation=students[3] },
                new Enrollment(){ CourseNavigation=courses[10], StudentNavigation=students[3] },
                new Enrollment(){ CourseNavigation=courses[8], StudentNavigation=students[3] },
                new Enrollment(){ CourseNavigation=courses[5], StudentNavigation=students[3] },
                new Enrollment(){ CourseNavigation=courses[4], StudentNavigation=students[3] },
                new Enrollment(){ CourseNavigation=courses[3], StudentNavigation=students[3] },
            };
            Enrollments.AddRange(enrollments);
            SaveChanges();
            
            var exams = new Faker<Exam>("de").Rules((f, e) =>
            {
                e.Date = f.Date.Between(new DateTime(1960, 01, 01), new DateTime(1970, 01, 01));
                e.ProfessorNavigation = f.Random.ListItem(professors);
                e.StudentPhoneNumber = "";
                e.EnrollmentNumber = $"L-{f.Random.Int(1000, 9999)}";
                e.Grade = f.Random.Int(1, 5).OrNull(f, 0.5f);
                e.StudentNavigation = f.Random.ListItem(students);
                e.EnrollmentNavigation = f.Random.ListItem(enrollments);
            })
            .Generate(30)
            .ToList();
            Exams.AddRange(exams);
            SaveChanges();
        }
    }
}
