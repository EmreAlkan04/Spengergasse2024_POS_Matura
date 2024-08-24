using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SPG_Fachtheorie.Aufgabe2.Domain;
using SPG_Fachtheorie.Aufgabe2.Infrastructure;
using SPG_Fachtheorie.Aufgabe3.RazorPages.Classes;
using System.Runtime.ConstrainedExecution;

namespace SPG_Fachtheorie.Aufgabe3.RazorPages.Pages.Advertisements
{
    public class DetailsModel : PageModel
    {
        private readonly PodcastContext _db;
        private readonly AuthService _authService;

        public DetailsModel(PodcastContext db, AuthService authService)
        {
            _db = db;
            _authService = authService;
        }
        [FromRoute]
        public int Id { get; set; }
        public Advertisement DAdvertisement { get; set; } = default!;

    //Zu jeder Werbung sollen die Id, der ProductName, der Kunde(Customer) im Format
    //„LastName FirstName“, der Produktionszeitpunkt(Production), die Length, die MinPlayTime, die
    //CostsPerPlay und die Anzahl der Aufrufe(Anzahl der ListenedItems) angezeigt werden.
    //Wird die angegebene ID nicht gefunden, so wird 404 not found geliefert.Falls die ID zwar existiert, die
    //Werbung aber nicht von einem Kunden des angemeldeten Administrators stammt, wird ebenfalls
    //404 not found geliefert.
        public IActionResult OnGet()
        {
            var exAd = _db.Advertisements
                .Include(s => s.Customer)
                .Include(s => s.ListenedItems)
                .Where(s => s.Id == Id )
                //.Select(s => new AdvertisementDTO(s.Id, s.ProductName, s.Customer.LastName, s.Customer.FirstName,
                //s.Production, s.Length, s.MinPlayTime, s.CostsPerPlay, s.ListenedItems.Count()))
                .ToList()
                .FirstOrDefault();

            if (exAd is null ) return NotFound();

            DAdvertisement = exAd;
            return Page();
        }
    }
}