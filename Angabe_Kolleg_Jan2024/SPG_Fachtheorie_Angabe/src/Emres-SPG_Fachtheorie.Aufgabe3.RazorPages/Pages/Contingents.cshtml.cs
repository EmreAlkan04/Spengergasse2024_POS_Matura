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
        private readonly EventContext _db;

        public ContingentsModel(EventContext db)
        {
            _db = db;
        }
        [FromRoute]
        public int showId { get; set; }
        //public Contingent Conts { get; set; } = default!;
        public Show Show { get; set; } = default!;
        //public record ContigentDTO(ContingentType ContigentType, int AvaibleTickets, List<TicketDTO> GuestDTOs);
        //public record TicketDTO(DateTime ReservationDateTime, TicketState TicketState, int Pax, string Nachname, string Vorname, DateTime Geburtsdatum);
        public IActionResult OnGet()
        {
            var show = _db.Shows
                .Include(s => s.Event)
                .Include(c => c.Contingents)
                .ThenInclude(s => s.Tickets)
                .ThenInclude(s => s.Guest)
                //.Select(s => new ContigentDTO(s.Contingents., s.AvailableTickets,
                //s.Tickets.Select(a => new TicketDTO(a.ReservationDateTime, a.TicketState, a.Pax ,a.Guest.Lastname, a.Guest.Firstname, a.Guest.BirthDate)).ToList())).ToList()
                .Where(s => s.Id == showId)
                .ToList()
                .FirstOrDefault();

            if (show is null) return NotFound();
            Show = show;
            return Page();
        }
    }
}