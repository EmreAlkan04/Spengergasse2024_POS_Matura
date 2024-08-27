using Bogus;
using FTSept2022.Aufgabe2.Domain;
using FTSept2022.Aufgabe2.Dto;
using FTSept2022.Aufgabe2.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace FTSept2022.Aufgabe2.Services
{
    public class CourseService
    {
        public readonly UniContext _db;

        public CourseService(UniContext db)
        {
            _db = db;
        }
        //public record ExamStatisticsDto(int StudensWithoutGradeCount, IEnumerable<StudentDto> StudentsWithNoExam)
        //{ }
        //public record StudentDto(string FirstName, string LastName, DateTime BirthDate)
        //{ }

        public ExamStatisticsDto CalcExamStatistics(int courseId)
        {
            var student = _db.Students
                 .Include(s => s.Enrollments)
                 .ThenInclude(s => s.Exams)
                 .Where(s => s.Enrollments.Any(s => s.CourseNavigation.Id == courseId))
                 .ToList();
            var withoutGrade = student.Where(s => s.Enrollments.Any(s => s.Exams.All(s => s.Grade == null))).Count();
            var studentNames = student
                .Where(s => s.Enrollments == null)
                .Select(s => new StudentDto(s.FirstName, s.LastName, s.BirthDate)).ToList();
            return new ExamStatisticsDto(withoutGrade, studentNames);
        }

        public bool SubscribeCourse(string studentRegistrationNumber, int courseId) //,int professorId)
        {
            var student = _db.Students
                .Include(s => s.Enrollments)
                .ThenInclude(s => s.CourseNavigation)
                .FirstOrDefault(s => s.RegistrationNumber == studentRegistrationNumber) ?? throw new CourseServiceException("student nicht gefunden");
            //if (student is null) return false
            var course = _db.Courses
                .Include(s => s.Enrollments)
                .ThenInclude(s => s.StudentNavigation)
                .FirstOrDefault(s => s.Id == courseId) ?? throw new CourseServiceException("course nicht gefunden");
            //if (course is null) return false;
            if(course.Enrollments.Any(s => s.StudentRegistrationNumber == studentRegistrationNumber))
            {
                throw new CourseServiceException("bereits subscribed");
            }
            if(course.MaxStudents <= course.Enrollments.Count())
            {
                throw new CourseServiceException("Klasse voll");
            }
            return true;
        }

        public bool UnsubscribeCourse(string studentRegistrationNumber, int courseId)
        {
            var student = _db.Students
                .Include(s => s.Enrollments)
                .ThenInclude(s => s.Exams)
                .FirstOrDefault(s => s.RegistrationNumber == studentRegistrationNumber
                && s.Enrollments.Any(s => s.CourseNavigation.Id == courseId)) ?? throw new CourseServiceException("student nicht einmal angemeldet");
            if(student.Enrollments.Where(s => s.Exams is null).Count() > 0)
            {
                throw new CourseServiceException("Student hat tests");
            }
            _db.Remove(student);
            try
            {
                _db.SaveChanges();
            }
            catch(DbUpdateException ex)
            {
                throw new CourseServiceException(ex.Message);
            }
            return true;
        }
    }
}
