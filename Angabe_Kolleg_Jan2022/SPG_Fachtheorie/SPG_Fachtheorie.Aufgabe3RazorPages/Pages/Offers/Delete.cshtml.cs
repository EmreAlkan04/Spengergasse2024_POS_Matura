using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SPG_Fachtheorie.Aufgabe2;
using SPG_Fachtheorie.Aufgabe2.Model;

namespace SPG_Fachtheorie.Aufgabe3RazorPages.Pages.Offers
{
    public class DeleteModel : PageModel
    {
        private readonly AppointmentContext _db;

        public DeleteModel(AppointmentContext db)
        {
            _db = db;
        }
        [FromRoute]
        public Guid OfferId { get; set; }
        public Offer Offer { get; set; }
        

        public IActionResult OnGet()
        {
            var offer = _db.Offers
                .Include(s => s.Appointments)
                .FirstOrDefault(s => s.Id == OfferId);
            if (offer is null) return NotFound();
            Offer = offer;
            return Page();
        }

        public IActionResult OnPost()
        {
            var offer = _db.Offers
                .Include(s => s.Appointments)
                .FirstOrDefault(s => s.Id == OfferId);
            if (offer is null) return NotFound();
            Offer = offer;
            if(offer.Appointments.Count > 0)
            {
                ModelState.AddModelError("", "Offer has appointments, cannot delete");
                return Page();
            }
            _db.Remove(Offer);
            try
            {
                _db.SaveChanges();
            }
            catch(DbUpdateException ex)
            {
                ModelState.AddModelError("Error deleting offer", ex.Message);
                return Page();
            }
            return RedirectToPage("/Offers/Index");
        }
    }
}
