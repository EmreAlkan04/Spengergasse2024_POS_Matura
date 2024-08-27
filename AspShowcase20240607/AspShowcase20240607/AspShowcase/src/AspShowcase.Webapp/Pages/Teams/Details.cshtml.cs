using AspShowcase.Application.Infrastructure;
using AspShowcase.Application.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspShowcase.Webapp.Pages.Teams
{
    public class DetailsModel : PageModel
    {
        public record TeamDetailsDto(string Name, string Schoolclass, List<TaskDto> Tasks);
        public record TaskDto(
            Guid Guid,
            string Subject, string Title,
            string TeacherFirstname, string TeacherLastname,
            DateTime ExpirationDate,
            bool IsExpired,
            int? MaxPoints = null);
        public TeamDetailsDto Team { get; set; } = default!;
        private readonly AspShowcaseContext _db;

        public DetailsModel(AspShowcaseContext db)
        {
            _db = db;
        }
        [FromRoute]
        public Guid TeamGuid { get; set; }
        public IActionResult OnGet()
        {
            // Liest die Details eines Teams aus der Datenbank.
            var team = _db.Teams
                .Where(t => t.Guid == TeamGuid)
                .Select(t => new TeamDetailsDto(
                    t.Name, t.Schoolclass,
                    t.Tasks.Select(task => new TaskDto(
                        task.Guid, task.Subject, task.Title, task.Teacher.Firstname, task.Teacher.Lastname,
                        task.ExpirationDate, task.ExpirationDate < DateTime.Now, task.MaxPoints
                     ))
                    .ToList()))
                .FirstOrDefault();
            if (team is null) return NotFound();
            Team = team;
            return Page();
        }
    }
}
