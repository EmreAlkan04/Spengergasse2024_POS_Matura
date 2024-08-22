using FTSept2022.Aufgabe2.Domain;
using FTSept2022.Aufgabe2.Infrastructure;
using FTSept2022.Aufgabe3.RazorPages.Classes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace FTSept2022.Aufgabe3.RazorPages.Pages.Exams
{
    public class CreateModel : PageModel
    {
        private readonly UniContext _db;
        private readonly AuthService _authService;

        public CreateModel(UniContext db, AuthService authService)
        {
            _db = db;
            _authService = authService;
        }

        public record NewExamCmd( //referenzen als Id, um den richtigen Course zu finden
            int CourseId,
            DateTime Pruefungsdatum,
            [Required(ErrorMessage ="angeben")]
            string Telefonnummer
            );
        [BindProperty]
        public NewExamCmd Command { get; set; } = default!; //BindProperty ist immer type Cmd

        public List<SelectListItem> Titels => _db.Courses
            .Select(c => new SelectListItem(c.Title, c.Id.ToString()))
            .ToList();

        public Enrollment Enrollments { get; set; } = default!;

        
        //Kein FromRoute, weil wir keinen Id haben
        //das neue object wird nicht gesucht, weil es dies noch nicht gibt
        //Statdessen wird das vorhandene object gesucht
        public IActionResult OnGet()
        {
            var enrollment = _db.Enrollments
                .Include(s => s.CourseNavigation)
                .ThenInclude(s => s.AllowedExamDates)
                //.Where(s => s.StudentNavigation.RegistrationNumber == RegistrationNumber)
                .FirstOrDefault();
            if (enrollment is null) return NotFound();
            Enrollments = enrollment;

            return Page();
        }

        public IActionResult OnPost()
        {
            //mit .Include auf mehrere Tabellen zugreifen
            var enrollment = _db.Enrollments
                .Include(s => s.StudentNavigation)
                .Include(s => s.CourseNavigation)
                .ThenInclude(s => s.AllowedExamDates)
                .Where(s => s.StudentNavigation.RegistrationNumber == _authService.RegistrationNumber)
                .FirstOrDefault();
            if (enrollment is null) return NotFound();
            Enrollments = enrollment;
            
            if(!ModelState.IsValid)
            {
                return Page();
            }
            if (enrollment.CourseNavigation.AllowedExamDates.Any(a => a.From > Command.Pruefungsdatum) || enrollment.CourseNavigation.AllowedExamDates.Any(a => a.To < Command.Pruefungsdatum))
            {
                ModelState.AddModelError("Command.Pruefungsdatum", "Prüfungsdatum nicht erlaubt");
                return Page();
            }

            var newExam = new Exam
            {
                Date = Command.Pruefungsdatum,
                StudentPhoneNumber = Command.Telefonnummer,
                EnrollmentId = enrollment.Id,
                EnrollmentNavigation = enrollment,
                ProfessorId = enrollment.CourseNavigation.ProfessorId,
                ProfessorNavigation = enrollment.CourseNavigation.ProfessorNavigation,
                StudentNavigation = enrollment.StudentNavigation
            };

            if (newExam is null) return NotFound();

            _db.Add(newExam);
            try
            {
                _db.SaveChanges();
            }
           catch (DbUpdateException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
            return RedirectToPage("/Enrollments/Index");
        }
    }
}