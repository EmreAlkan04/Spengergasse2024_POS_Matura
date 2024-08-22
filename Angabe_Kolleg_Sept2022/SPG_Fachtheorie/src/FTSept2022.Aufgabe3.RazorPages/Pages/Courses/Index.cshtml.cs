using FTSept2022.Aufgabe2.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FTSept2022.Aufgabe3.RazorPages.Pages.Courses
{
    public class IndexModel : PageModel
    {
        private readonly UniContext _db;

        public IndexModel(UniContext db)
        {
            _db = db;
        }

        public record CourseDTO(
            string Title,
            int Ects,
            string ProfessorFirstName,
            string ProfessorLastName
            );
        public List<CourseDTO> Courses { get; set; } = new();

        public void OnGet()
        {
            var courses = _db.Courses
                .Include(s => s.ProfessorNavigation)
                .Select(s => new CourseDTO(
                    s.Title, s.Ects, s.ProfessorNavigation.FirstName, s.ProfessorNavigation.LastName))
                .ToList();

            Courses = courses;
        }
    }
}