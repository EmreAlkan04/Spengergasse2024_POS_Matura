using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SPG_Fachtheorie.Aufgabe2.Infrastructure;
using SPG_Fachtheorie.Aufgabe2.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SPG_Fachtheorie.Aufgabe3.RazorPages.Pages
{
    /// <summary>
    /// http://localhost:5000/BuyTicket/40
    /// </summary>
    public class BuyTicketModel : PageModel
    {
        public record NewTicketCmd(
        int GuestId,
        [Required(ErrorMessage = "Begleitperson ist Pflichtfeld!")]
        [Range(0,8,ErrorMessage ="Max. 8 Pax")]
        string Pax
     );

        private readonly EventContext _db;
        public Contingent Contingent { get; private set; } = default!;

        public BuyTicketModel(EventContext db)
        {
            _db = db;
        }
        [BindProperty]
        public NewTicketCmd NewTicket { get; set; } = default!;
        public List<SelectListItem> Guests => _db.Guests.OrderBy(g => g.Lastname).ThenBy(g => g.Firstname)
            .Select(g => new SelectListItem($"{g.Lastname} {g.Firstname}", g.Id.ToString()))
            .ToList();

        [FromRoute]
        public int ContingentId { get; set; }

        public IActionResult OnGet()
        {
            var contingent = _db.Contingents
                .Include(s => s.Show)
                .ThenInclude(e => e.Event)
                .Where(c => c.Id == ContingentId)
                .FirstOrDefault();
            if (contingent is null) return NotFound();
            ;
            Contingent = contingent;
            return Page();
        }

        public IActionResult OnPost()
        {
            var contingent = _db.Contingents
              .Include(s => s.Show)
              .ThenInclude(e => e.Event)
              .Where(c => c.Id == ContingentId)
              .FirstOrDefault();
            if (contingent is null) return NotFound();
            Contingent = contingent;
            if (!ModelState.IsValid) return Page();

            var guest = _db.Guests.Where(g => g.Id == NewTicket.GuestId).FirstOrDefault();
            if (guest is null)
            {
                ModelState.AddModelError("NewTicket.GuestId", "Gast nicht gefunden");
                return Page();
            }

                var newTicket = new Ticket(guest, contingent, TicketState.Sold, DateTime.UtcNow, int.Parse(NewTicket.Pax));
                _db.Tickets.Add(newTicket);
            try
            {
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }


            return RedirectToPage("Contingents", new { showId = contingent.Show.Id });
        }
    }
}