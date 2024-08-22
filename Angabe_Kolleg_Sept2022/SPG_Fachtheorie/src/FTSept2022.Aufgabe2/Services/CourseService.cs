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

        public ExamStatisticsDto CalcExamStatistics(int courseId)
        {
            var all = _db.Courses.Include(s => s.Enrollments).ThenInclude(s => s.Exams)
                .FirstOrDefault(s => s.Id == courseId);

            int studentsWithoutGradeCount = all.Enrollments.Where(s => s.Exams.Any(e => e.Grade == null)).Count();
            //throw new NotImplementedException("Noch keine Implementierung vorhanden");

            var studentsWithNoExam = all.Enrollments.Where(s => s.Exams.Count == 0).Select(s => new StudentDto(s.StudentNavigation.FirstName, s.StudentNavigation.LastName, s.StudentNavigation.BirthDate));
            //studentsWithNoExam.Select(s => new StudentDto(s.StudentNavigation.FirstName, s.StudentNavigation.LastName, s.StudentNavigation.BirthDate));

            return new ExamStatisticsDto(studentsWithoutGradeCount, studentsWithNoExam);
        }

        public bool SubscribeCourse(string studentId, int courseId) //,int professorId)
        {
            var student = _db.Students.Include(s => s.Enrollments).FirstOrDefault(s => s.RegistrationNumber == studentId);
            var course = _db.Courses.Include(s => s.Enrollments).FirstOrDefault(s => s.Id == courseId);

            if(student == null )
            {
                return false;
                //throw new ServiceException("Student nicht gefunden");
            }

            if(course == null)
            {
                return false;
                //throw new ServiceException("Kurs nicht gefunden");
            }

            if (student.Enrollments.Any(s => s.CourseId == courseId))
            {
                return false;
                //throw new ServiceException("Student ist bereits eingeschrieben");
            }

            if (course.MaxStudents <= course.Enrollments.Count())
            {
                return false;
                //throw new ServiceException("Klasse voll");
            }

            //var enroll = new Enrollment() { CourseId = course, StudentRegistrationNumber = student};
            var addEnrollment = new Enrollment() { CourseId = courseId, StudentRegistrationNumber = studentId };
            _db.Enrollments.Add(addEnrollment);
            return true; 
            //throw new NotImplementedException("Noch keine Implementierung vorhanden");
        }

        public bool UnsubscribeCourse(string studentId, int courseId)
        {
            var student = _db.Students.Include(s => s.Enrollments).FirstOrDefault(s => s.RegistrationNumber == studentId);
            var course = _db.Courses.Include(s => s.Enrollments).FirstOrDefault(s => s.Id == courseId);

            if(student == null)
            {
                return false;
                //throw new ServiceException("Student nicht gefunden");
            }
            if(course == null)
            {
                return false;
                //throw new ServiceException("Kurs nicht gefunden");
            }

            if(student.Enrollments.FirstOrDefault(s => s.CourseId == courseId) == null)
            {
                return false;
                //throw new ServiceException("Student ist nicht eingeschrieben");
            }

            if( student.Enrollments.FirstOrDefault(s => s.CourseId == courseId).Exams.Any())
            {
                return false;
                //throw new ServiceException("Student hat bereits Prüfungen abgelegt");
            }

            var entroll = _db.Enrollments.FirstOrDefault(s => s.CourseId == courseId && s.StudentRegistrationNumber == studentId);
            if(entroll == null)
            {
                return false;
                //throw new ServiceException("Student ist nicht eingeschrieben");
            }

            _db.Enrollments.Remove(entroll);
            return true;
            //throw new NotImplementedException("Noch keine Implementierung vorhanden");
        }
    }
}
