using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SPG_Fachtheorie.Aufgabe2;
using SPG_Fachtheorie.Aufgabe2.Model;
using SPG_Fachtheorie.Aufgabe3RazorPages.Services;

namespace SPG_Fachtheorie.Aufgabe3RazorPages.Pages.Offers
{
    public class EditModel : PageModel
    {
        private readonly AppointmentContext _db;
        private readonly AuthService _authservice;

        public EditModel(AppointmentContext db, AuthService authservice)
        {
            _db = db;
            _authservice = authservice;
        }
        [FromRoute]
        public Guid OfferId { get; set; }
        public Offer Offer { get; set; } = default!;
        //public record OfferDTO(string EducationType,string Name, int Term,
        //    DateTime From, DateTime To, Guid Id);
        public record EditOfferCmd(DateTime NewTO);
        [BindProperty]
        public EditOfferCmd Command { get; set; } = default!;

        public IActionResult OnGet()
        {
            var offer = _db.Offers
                .Include(s => s.Subject)
                //.Select(s => new OfferDTO(s.Subject.EducationType, s.Subject.Name, s.Subject.Term, s.From, s.To, s.Id))
                .FirstOrDefault(s => s.Id == OfferId);

            if (offer == null) return NotFound();
            Offer = offer;
            return Page();
        }
        public IActionResult OnPost()
        {
            var offer = _db.Offers
            .Include(s => s.Subject)
            .FirstOrDefault(s => s.Id == OfferId);
            if (offer == null) return NotFound();
            Offer = offer;
            if(!ModelState.IsValid)
            {
                return Page();
            }
            if(Offer.To < Command.NewTO) ModelState.AddModelError("Command.NewTO", "New To must be before the old To");
            Offer.To = Command.NewTO;
            _db.Update(Offer);
            try
            {
                _db.SaveChanges();
            }
            catch(DbUpdateException e)
            {
                ModelState.AddModelError("Command.NewTO", e.Message);
                return Page();
            }
            return RedirectToPage("/Offers/Index"); 
        }
    }
}
