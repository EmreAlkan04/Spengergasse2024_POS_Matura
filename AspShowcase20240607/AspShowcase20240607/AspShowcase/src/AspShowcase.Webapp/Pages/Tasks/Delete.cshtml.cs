using AspShowcase.Application.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AspShowcase.Webapp.Pages.Tasks
{
    public class DeleteModel : PageModel
    {
        public record DeleteTaskDto(string Subject, string Title, Guid TeamGuid);

        private readonly AspShowcaseContext _db;

        [FromRoute]
        public Guid TaskGuid { get; set; }

        public DeleteTaskDto TaskDto { get; set; } = default!;
        public DeleteModel(AspShowcaseContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Im Formular wird asp-page-handler="No" gesetzt.
        /// Das bedeutet, dass dieser Handler aufgerufen wird, wenn der Button "Nein" geklickt wird.
        /// Die Namenskonvention ist On + Get/Post + Handlername.
        /// </summary>
        /// <returns></returns>
        public IActionResult OnPostNo()
        {
            return RedirectToPage("/Teams/Details", new { TaskDto.TeamGuid });
        }
        public IActionResult OnPostYes()
        {
            var task = _db.Tasks.FirstOrDefault(t => t.Guid == TaskGuid);
            if (task is null)
                return RedirectToPage("/Teams/Details", new { TaskDto.TeamGuid });
            // Optional: Check von constraints

            _db.Tasks.Remove(task);
            try
            {
                _db.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                ModelState.AddModelError(string.Empty, e.InnerException?.Message ?? e.Message);
                return Page();
            }
            return RedirectToPage("/Teams/Details", new { TaskDto.TeamGuid });
        }

        /// <summary>
        /// Diese Methode wird vor allen Page Handlern ausgeführt.
        /// Hier können Teile hineingeschrieben werden, die für alle Handler benötigt werden.
        /// </summary>
        public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            var taskDto = _db.Tasks
                .Where(t => t.Guid == TaskGuid)
                .Select(t => new DeleteTaskDto(t.Subject, t.Title, t.Team.Guid))
                .FirstOrDefault();
            if (taskDto is null)
            {
                context.Result = BadRequest();
                return;
            }
            TaskDto = taskDto;
        }
    }
}
