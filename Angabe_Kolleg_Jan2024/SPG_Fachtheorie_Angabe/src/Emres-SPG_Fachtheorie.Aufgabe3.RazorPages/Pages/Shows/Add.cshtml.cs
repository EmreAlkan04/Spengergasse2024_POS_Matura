using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SPG_Fachtheorie.Aufgabe2.Infrastructure;
using SPG_Fachtheorie.Aufgabe2.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SPG_Fachtheorie.Aufgabe3.RazorPages.Pages.Shows
{
    public class AddModel : PageModel
    {
        private readonly EventContext _db;
        public AddModel(EventContext db)
        {
            _db = db;
        }

        [BindProperty]
        public NewShowCmd NewShow { get; set; } = default!;
        public record NewShowCmd(
            int EventId,
            DateTime Date
            );
        public List<SelectListItem> Events => _db.Events
            .Select(s => new SelectListItem(s.Name, s.Id.ToString())).ToList();
      
        public IActionResult OnGet()
        {
            var events = _db.Events
               .Include(s => s.Shows)
               .ThenInclude(s => s.Contingents)
               .ToList()
                .FirstOrDefault();
            if (events == null) return NotFound();
            return Page();
        }
        public IActionResult OnPost()
        {
            var events = _db.Events
                .Include(s => s.Shows)
                .ThenInclude(s => s.Contingents)
                .Where(s => s.Id == NewShow.EventId)
                .ToList()
                .FirstOrDefault();
            if (events == null) return NotFound();
            if(NewShow.Date < DateTime.Now)
            {
                ModelState.AddModelError("", "Date must be in the future");
                return Page();
            }
            var newShow = new Show(events, NewShow.Date);

            _db.Add(newShow);
            try
            {
                _db.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return Page();
            }
            return RedirectToPage("/Events");
        }
    }
}