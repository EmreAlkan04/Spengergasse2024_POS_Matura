using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SPG_Fachtheorie.Aufgabe2;
using SPG_Fachtheorie.Aufgabe2.Model;

namespace SPG_Fachtheorie.Aufgabe3RazorPages.Pages.Offers
{
    public class DetailsModel : PageModel
    {
        private readonly AppointmentContext _db;

        public DetailsModel(AppointmentContext db)
        {
            _db = db;
        }
        [FromRoute]
        public Guid OfferId { get; set; }
        public record AppointmentDTO(
            string Vorname, string Nachname,
            DateTime Date, string Ort, AppointmentState Status
            );
        public List<AppointmentDTO> AppointmentsList { get; set; } = default!;
        public IActionResult OnGet()
        {
            var appointments = _db.Appointments
                .Include(s => s.Offer)
                .Include(s => s.Student)
                .Where(s => s.OfferId == OfferId)
                .Select(s => new AppointmentDTO(s.Student.Firstname, s.Student.Lastname,
                s.Date, s.Location, s.State))
                .ToList();
            if (appointments is null) return NotFound();
            AppointmentsList = appointments;
            return Page();
        }
    }
}
