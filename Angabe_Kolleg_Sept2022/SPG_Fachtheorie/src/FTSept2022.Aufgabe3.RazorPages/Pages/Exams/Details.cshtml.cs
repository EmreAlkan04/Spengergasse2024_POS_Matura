using FTSept2022.Aufgabe2.Domain;
using FTSept2022.Aufgabe2.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FTSept2022.Aufgabe3.RazorPages.Pages.Exams
{
    public class DetailsModel : PageModel
    {

        private readonly UniContext _db;

        public DetailsModel(UniContext db)
        {
            _db = db;
        }

        [FromRoute]
        public int Id { get; set; }
        public Exam Exams { get; set; } = default!;

        public IActionResult OnGet()
        {
            var zeugnis = _db.Exams
                .Include(s => s.EnrollmentNavigation)
                .ThenInclude(s => s.CourseNavigation)
                .ThenInclude(s => s.ProfessorNavigation)
                .Where(s => s.Id == Id)
                .ToList()
                .FirstOrDefault();
            if (zeugnis is null || zeugnis.Grade is null) return NotFound();
            Exams = zeugnis;
            return Page();
        }
    }
}