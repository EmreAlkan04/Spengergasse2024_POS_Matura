using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SPG_Fachtheorie.Aufgabe2.Infrastructure;
using SPG_Fachtheorie.Aufgabe2.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SPG_Fachtheorie.Aufgabe3.RazorPages.Pages
{
    public class EventsModel : PageModel
    {
        private readonly EventContext _db;
        public EventsModel(EventContext db)
        {
            _db = db;
        }
        public record EventDTO(int Id, string Name, List<ShowDTO> ShowDTOs);
        public record ShowDTO(int Id, DateTime Date);
        public List<EventDTO> EventsList { get; set; } = default!;
        //public Event Events { get; set; } = default!;
        public void OnGet()
        {
            var events = _db.Events
                .Include(s => s.Shows)
                .Select(e => new EventDTO(e.Id, e.Name, e.Shows.Select(s => new ShowDTO(s.Id, s.Date)).ToList()))
                .ToList();
            EventsList = events;
        }
    }
}