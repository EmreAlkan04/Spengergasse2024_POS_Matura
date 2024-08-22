using FTSept2022.Aufgabe2.Domain;
using FTSept2022.Aufgabe2.Infrastructure;
using FTSept2022.Aufgabe3.RazorPages.Classes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace FTSept2022.Aufgabe3.RazorPages.Pages.Enrollments
{
    public class IndexModel : PageModel
    {
        private readonly UniContext _db;
        private readonly AuthService _authService;

        public IndexModel(UniContext db, AuthService authService)
        {
            _db = db;
            _authService = authService;
        }

        public record EnrollmentDTO(
            string Titel,
            int Ects,
            string ProfessorLastName,
            string ProfessorFirstName,
            int? Grade,
            DateTime Date,
            int Id,
            string RegistrationNumber
            );
        public List<EnrollmentDTO> EnrollmentsList { get; set; } = new();

        public void OnGet()
        {
       
            var enrolls = _db.Exams
                .Include(s => s.EnrollmentNavigation)
                .ThenInclude(s => s.CourseNavigation)
                .Where(s => s.EnrollmentNavigation.StudentNavigation.RegistrationNumber == _authService.RegistrationNumber)
                .Select(s => new EnrollmentDTO(
                    s.EnrollmentNavigation.CourseNavigation.Title,
                    s.EnrollmentNavigation.CourseNavigation.Ects,
                    s.EnrollmentNavigation.CourseNavigation.ProfessorNavigation.LastName,
                    s.EnrollmentNavigation.CourseNavigation.ProfessorNavigation.FirstName,
                    s.Grade,
                    s.Date, s.Id, s.StudentNavigation.RegistrationNumber
                    ))
                .ToList();

            EnrollmentsList = enrolls;
        }
    }
}