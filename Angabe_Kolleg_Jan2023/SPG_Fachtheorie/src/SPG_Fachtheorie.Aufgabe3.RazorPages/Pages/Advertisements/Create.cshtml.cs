using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SPG_Fachtheorie.Aufgabe2.Domain;
using SPG_Fachtheorie.Aufgabe2.Infrastructure;
using SPG_Fachtheorie.Aufgabe3.RazorPages.Classes;
using System.ComponentModel.DataAnnotations;
using System.Runtime.ConstrainedExecution;

namespace SPG_Fachtheorie.Aufgabe3.RazorPages.Pages.Advertisements
{
    public class CreateModel : PageModel
    {
        private readonly PodcastContext _db;
        private readonly AuthService _service;

        public CreateModel(PodcastContext db, AuthService service)
        {
            _db = db;
            _service = service;
        }

        public record NewAdvertisementCmd(
            int CustomerId,
            [MinLength(3)]
            string ProductName,
            DateTime Production,
            int Length,
            [Required(ErrorMessage ="MinPlayTime ist Pflicht")]
            int? MinPlayTime,
            [Required(ErrorMessage ="CostPerPlay ist Pflicht")]
            decimal CostsPerPlay
            );
        [BindProperty]
        public NewAdvertisementCmd NewAdvertisement { get; set; } = default!;
        public Customer Customers { get; set; } = default!;
        //public Admin Admins { get; set; } = default!;

        public List<SelectListItem> CustomerList => _db.Customers
            .Where(s => s.ResponsibleAdminId == _service.AdminId)
            .Select(s => new SelectListItem(s.FirstName, s.Id.ToString())).ToList();

        //Der ProductName, der Produktionszeitpunkt(Production), die Length, die MinPlayTime und die
        //CostsPerPlay können in den entsprechenden Eingabefeldern angegeben werden.Der ProductName
        //muss zumindest 3 Zeichen umfassen, der Produktionszeitpunkt(Production) muss nach dem
        //RegistrationDate vom Kunden(Customer) liegen, die Length und die CostsPerPlay müssen angegeben
        //werden.Falls einer dieser Bedingungen nicht erfüllt ist, ist eine entsprechende Fehlermeldung nach dem
        //Klick auf Speichern auszugeben. Nach dem erfolgreichen Speichern ist die Seite unter der Route
        ///Customer/Index aufzurufen.


        public IActionResult OnGet()
        {
            var customer = _db.Customers
                .Include(s => s.ResponsibleAdmin)
                .Include(s => s.Advertisements)
                //.Where(s => s.ResponsibleAdminId == _service.AdminId)
                .ToList();
            if (customer is null) return NotFound();
            return Page();
        }

        public IActionResult OnPost()
        {
            var customer = _db.Customers
                  .Include(s => s.ResponsibleAdmin)
                  .Include(s => s.Advertisements)
                  .FirstOrDefault(s => s.Id == NewAdvertisement.CustomerId);
            if (customer is null) return NotFound();
            Customers = customer;
            if (!ModelState.IsValid) return Page();
            if (NewAdvertisement.Production < customer.RegistrationDate)
            {
                ModelState.AddModelError("NewAdvertisement.Production", "Production must be after RegistrationDate");
                return Page();
            }
            var newAdvertisement = new Advertisement
            {
                ProductName = NewAdvertisement.ProductName,
                MinPlayTime = NewAdvertisement.MinPlayTime,
                CostsPerPlay = NewAdvertisement.CostsPerPlay,
                CustomerId = NewAdvertisement.CustomerId,
                Customer = customer,
                Production = NewAdvertisement.Production,
                Length = NewAdvertisement.Length
            };
            _db.Add(newAdvertisement);
            try
            {
                _db.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("NewAdvertisement.ProductName", "Product already exists");
                return Page();
            }
            return RedirectToPage("/Customers/Index");
        }
    }
}