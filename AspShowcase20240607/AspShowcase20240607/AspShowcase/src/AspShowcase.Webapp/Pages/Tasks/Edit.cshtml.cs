using AspShowcase.Application.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace AspShowcase.Webapp.Pages.Tasks
{
    public class EditModel : PageModel
    {
        public record EditTaskDto(string Teachername, int? MaxPoints);
        public record EditTaskCmd(
            [StringLength(255, MinimumLength = 1, ErrorMessage = "Fach muss zwischen 1 und 255 Zeichen lang sein")]
            string Subject,
            [StringLength(255, MinimumLength = 1, ErrorMessage = "Titel muss zwischen 1 und 255 Zeichen lang sein")]
            string Title,
            DateTime ExpirationDate) : IValidatableObject
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

        [FromRoute]
        public Guid TaskGuid { get; set; }

        [BindProperty]
        public EditTaskCmd EditTask { get; set; } = default!;

        public EditTaskDto TaskDto { get; set; } = default!;
        public EditModel(AspShowcaseContext db)
        {
            _db = db;
        }

        public IActionResult OnGet()
        {
            // Suche nach dem Task, das ¸ber die Adresse (Routing) in TeamGuid ¸bergeben wurde.
            // Dies ist nˆtig, damit wir Teachername und MaxPoints f¸r die Anzeige im Formular haben.
            var taskDto = _db.Tasks
                .Where(t => t.Guid == TaskGuid)
                .Select(t => new EditTaskDto($"{t.Teacher.Lastname} {t.Teacher.Firstname}", t.MaxPoints))
                .FirstOrDefault();
            if (taskDto is null) return BadRequest();
            TaskDto = taskDto;

            // Damit das Formular auch beim Betreten der Seite ausge¸llt ist, holen wir uns die Daten des Tasks.
            var editTask = _db.Tasks
                .Where(t => t.Guid == TaskGuid)
                .Select(t => new EditTaskCmd(t.Subject, t.Title, t.ExpirationDate))
                .FirstOrDefault();
            if (editTask is null) return BadRequest();
            EditTask = editTask;
            return Page();
        }

        public IActionResult OnPost()
        {
            // Suche nach dem Task, das ¸ber die Adresse (Routing) in TeamGuid ¸bergeben wurde.
            // Dies wird benˆtigt, da sonst TaskDto null ist wenn die Validierung fehlschl‰gt.
            var taskDto = _db.Tasks
                .Where(t => t.Guid == TaskGuid)
                .Select(t => new EditTaskDto($"{t.Teacher.Lastname} {t.Teacher.Firstname}", t.MaxPoints))
                .FirstOrDefault();
            if (taskDto is null) return BadRequest();
            TaskDto = taskDto;

            if (!ModelState.IsValid) return Page();
            // Da wir f¸r den Redirect die TeamGuid benˆtigen, m¸ssen wir mit Include einen Join machen
            // und auch die Daten des Teams lesen.
            var task = _db.Tasks.Include(t => t.Team).FirstOrDefault(t => t.Guid == TaskGuid);
            if (task is null)
            {
                ModelState.AddModelError(string.Empty, "Task nicht gefunden");
                return Page();
            }
            task.Subject = EditTask.Subject;
            task.Title = EditTask.Title;
            task.ExpirationDate = EditTask.ExpirationDate;
            try
            {
                _db.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError(string.Empty, ex.InnerException?.Message ?? ex.Message);
                return Page();
            }
            // Wichtig: Der Routingparameter heiﬂt TeamGuid, daher muss er auch so benannt werden.
            return RedirectToPage("/Teams/Details", new { TeamGuid = task.Team.Guid });
        }
    }
}
