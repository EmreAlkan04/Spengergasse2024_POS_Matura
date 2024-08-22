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
        private readonly EventContext _db;

        public BuyTicketModel(EventContext db)
        {
            _db = db;
        }
        [FromRoute]
        public int contingentId { get; set; }
        public record NewTicketCmd(
            //int ContingentId,
            int GuestId,
            [Range(0,8, ErrorMessage ="zwischen 0-8")]
            int Pax
            );
        [BindProperty]
        public NewTicketCmd Command { get; set; } = default!;
        public Contingent Contingent { get; set; } = default!;
        public List<SelectListItem> Guests => _db.Guests
            .OrderBy(s => s.Lastname).ThenBy(s => s.Firstname)
            .Select(s => new SelectListItem($"{s.Firstname} {s.Lastname}", s.Id.ToString())).ToList();

        public IActionResult OnGet()
        {
            var contingent = _db.Contingents
                .Include(s => s.Show)
                .ThenInclude(s => s.Event)
                .Where(s => s.Id == contingentId)
                .ToList()
                .FirstOrDefault();
            if (contingent is null) return NotFound();
            Contingent = contingent;
            return Page();
        }

        public IActionResult OnPost()
        {
            var contingent = _db.Contingents
                .Include(s => s.Show)
                .ThenInclude(s => s.Event)
            .Where(s => s.Id == contingentId)
            .ToList()
            .FirstOrDefault();
            if (contingent is null) return NotFound();
            Contingent = contingent;

            if (!ModelState.IsValid) return Page();

            var guest = _db.Guests
                .Where(s => s.Id == Command.GuestId)
                .ToList()
                .FirstOrDefault();
            if (guest is null) return NotFound();
            if (contingent.AvailableTickets < Command.Pax + 1)
            {
                ModelState.AddModelError(string.Empty, "Nicht genügend Tickets verfügbar");
                return Page();
            }

            var newTicket = new Ticket(guest, contingent, TicketState.Sold, DateTime.UtcNow, Command.Pax);
            Contingent.AvailableTickets -= Command.Pax + 1;

            _db.Add(newTicket);
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