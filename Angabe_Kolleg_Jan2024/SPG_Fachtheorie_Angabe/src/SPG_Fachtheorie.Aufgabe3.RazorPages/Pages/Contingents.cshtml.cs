using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SPG_Fachtheorie.Aufgabe2.Infrastructure;
using SPG_Fachtheorie.Aufgabe2.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SPG_Fachtheorie.Aufgabe3.RazorPages.Pages
{
    public class ContingentsModel : PageModel
    {
        //Eigentlich wird dies mit DTOs gemacht, aber so geht es auch
        private readonly EventContext _db;
        public Show Show { get; private set; } = default!;

        public ContingentsModel(EventContext db)
        {
            _db = db;
        }

        [FromRoute]
        public int ShowId { get; set; }
        public IActionResult OnGet()
        {

            var show = _db.Shows
                .Include(s => s.Event)
                .Include(c => c.Contingents)
                    .ThenInclude(t => t.Tickets)
                    .ThenInclude(e => e.Guest)
                .Where(s => s.Id == ShowId)
                .ToList()
                .FirstOrDefault();

            if (show is null)
            {
                return NotFound();
            }

            Show = show;
            return Page();
        }

    }
}