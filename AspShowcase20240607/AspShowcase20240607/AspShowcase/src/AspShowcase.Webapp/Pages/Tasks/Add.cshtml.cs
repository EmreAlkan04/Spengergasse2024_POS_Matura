using AspShowcase.Application.Infrastructure;
using AspShowcase.Application.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace AspShowcase.Webapp.Pages.Tasks
{
    public class AddModel : PageModel
    {
        public record NewTaskDto(string Teamname);
        public record NewTaskCmd(
            [StringLength(255, MinimumLength = 1, ErrorMessage = "Fach muss zwischen 1 und 255 Zeichen lang sein")]
            string Subject,
            [StringLength(255, MinimumLength = 1, ErrorMessage = "Titel muss zwischen 1 und 255 Zeichen lang sein")]
            string Title,
            Guid TeacherGuid,
            DateTime ExpirationDate,
            [Range(1,99, ErrorMessage = "Maximale Punktzahl muss zwischen 1 und 99 liegen")]
            int? MaxPoints = null) : IValidatableObject
        {
            public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            {
                if (ExpirationDate < DateTime.Now)
                {
                    yield return new ValidationResult(
                        "Ablaufdatum muss in der Zukunft liegen",
                        new[] { nameof(ExpirationDate) });
                }
            }
        }

        private readonly AspShowcaseContext _db;

        [BindProperty]
        public NewTaskCmd NewTask { get; set; } = default!;
        [FromRoute]
        public Guid TeamGuid { get; set; }
        public NewTaskDto TaskDto { get; set; } = default!;
        public List<SelectListItem> Teams =>
            _db.Teams
                .Select(t => new SelectListItem(t.Name, t.Guid.ToString()))
                .ToList();
        public List<SelectListItem> Teachers =>
            _db.Teachers
                .Select(t => new SelectListItem($"{t.Lastname} {t.Firstname}", t.Guid.ToString()))
                .ToList();
        public AddModel(AspShowcaseContext db)
        {
            _db = db;
        }

        public IActionResult OnGet()
        {
            // Suche nach dem Team, das über die Adresse (Routing) in TeamGuid übergeben wurde.
            var team = _db.Teams.FirstOrDefault(t => t.Guid == TeamGuid);
            if (team is null) return BadRequest();
            TaskDto = new NewTaskDto(team.Name);
            return Page();
        }


        /// <summary>
        /// Wird aufgerufen, wenn der User im Formular auf "Speichern" klickt.
        /// </summary>
        /// <returns></returns>
        public IActionResult OnPost()
        {
            // Damit der Teamname bei einem Eingabefehler nicht zu einer NullReferenceException führt, muss er auch gelesen werden.
            // Suche nach dem Team, das über die Adresse (Routing) in TeamGuid übergeben wurde.
            var team = _db.Teams.FirstOrDefault(t => t.Guid == TeamGuid);
            if (team is null) return BadRequest();
            TaskDto = new NewTaskDto(team.Name);

            if (!ModelState.IsValid)
            {
                return Page();
            }
            // Schritt 1: Lesen aller Fremdschlüssel
            if (team is null)
            {
                ModelState.AddModelError("NewTask.TeamGuid", "Team nicht gefunden");
                return Page();
            }
            var teacher = _db.Teachers.FirstOrDefault(t => t.Guid == NewTask.TeacherGuid);
            if (teacher is null)
            {
                ModelState.AddModelError("NewTask.TeacherGuid", "Lehrer nicht gefunden");
                return Page();
            }

            // Schritt 2: Erstellen des neuen Tasks
            var newTask = new Application.Models.Task(
                NewTask.Subject, NewTask.Title, team, teacher,
                NewTask.ExpirationDate, NewTask.MaxPoints);
            _db.Tasks.Add(newTask);
            try
            {
                _db.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", ex.InnerException?.Message ?? ex.Message);
                return Page();
            }
            return RedirectToPage("/Teams/Details", new { TeamGuid });
        }

    }
}
