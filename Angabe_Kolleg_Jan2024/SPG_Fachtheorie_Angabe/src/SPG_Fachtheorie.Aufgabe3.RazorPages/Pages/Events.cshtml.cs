using Microsoft.AspNetCore.Mvc.RazorPages;
using SPG_Fachtheorie.Aufgabe2.Infrastructure;
using SPG_Fachtheorie.Aufgabe2.Model;
using SPG_Fachtheorie.Aufgabe2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;

namespace SPG_Fachtheorie.Aufgabe3.RazorPages.Pages
{
    public class EventsModel : PageModel
    {
        //ToDo für Allgemeinen Sicht:
        //Context für Daten
        //Ein DTO, um das zu Zeigen, was benötigt wird
        //Liste von DTOs, die die Daten enthalten
        public record EventDTO(string Name, List<Show> Shows, int Id );
        private readonly EventContext _db;
        public List<EventDTO> Events { get; private set; } = new();

        public EventsModel(EventContext db)
        {
            _db = db;
        }

        public void OnGet()
        {
            var events = _db.Events.Select(e => new EventDTO(e.Name, e.Shows, e.Id)).ToList();
            Events = events;
        }
    }
}