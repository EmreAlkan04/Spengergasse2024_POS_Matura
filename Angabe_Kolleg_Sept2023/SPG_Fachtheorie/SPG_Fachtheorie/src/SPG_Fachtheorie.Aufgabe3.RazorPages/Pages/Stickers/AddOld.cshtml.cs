using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SPG_Fachtheorie.Aufgabe2.Infrastructure;
using SPG_Fachtheorie.Aufgabe2.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using static System.Net.Mime.MediaTypeNames;

namespace SPG_Fachtheorie.Aufgabe3.RazorPages.Pages.Stickers
{
    public class AddModel : PageModel
    {
        private readonly StickerContext _db;
        public AddModel(StickerContext db)
        {
            _db = db;
        }

        public record NewStickerDTO(string CustomerLastname);
        public record NewStickerCmd(
            string Vehicles,
            int StickerTypes,
            DateTime ValidFrom
            ) : IValidatableObject

        {
            public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            {
                if (ValidFrom <= DateTime.Now)
                {
                    yield return new ValidationResult(
                        "Startdatum darf nicht in der Vergangenheit liegen",
                        new[] { nameof(ValidFrom) });
                }
            }
        }


        [BindProperty]
        public NewStickerCmd NewSticker { get; set; } = default!;
        [FromRoute]
        public Guid CustomerGuid { get; set; }

        //public NewStickerDTO StickerDTO { get; set; } = default!;
        public Customer Customer { get; set; } = default!;



        public List<SelectListItem> Customers => _db.Customers
            .Select(s => new SelectListItem(s.Lastname, s.Id.ToString()))
            .ToList();
        public List<SelectListItem> Vehicles => _db.Vehicles
            .Where(s => s.Customer.Guid == CustomerGuid)
             .Select(s => new SelectListItem(s.Numberplate, s.Id.ToString()))
            .ToList();

        public List<SelectListItem> StickerTypes => _db.StickerTypes
            .Select(s => new SelectListItem(s.Name, s.Id.ToString()))
            .ToList();


        public IActionResult OnGet()
        {
            var customer = _db.Customers
                .Include(s => s.Stickers)
                .ThenInclude(s => s.StickerType)
                .FirstOrDefault(s => s.Guid == CustomerGuid);
            if (customer is null)
            {
                return NotFound();
            }
            Customer = customer;

            return Page();
        }

        public IActionResult OnPost()
        {
            var customer = _db.Customers
               .Include(s => s.Stickers)
               .ThenInclude(s => s.StickerType)
               .FirstOrDefault(s => s.Guid == CustomerGuid);

            if (customer is null)
            {
                return NotFound();
            }
            Customer = customer;

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var vehicle = _db.Vehicles
                .Where(s => s.Numberplate == NewSticker.Vehicles)
                .ToList();

            if (vehicle is null)
            {
                ModelState.AddModelError("", "Vehicle not found");
                return Page();
            }

            var stickerType = _db.StickerTypes
                .FirstOrDefault(s => s.Id == NewSticker.StickerTypes);

            if (stickerType is null)
            {
                ModelState.AddModelError("", "StickerType not found");
                return Page();
            }

            var newSticker = new Sticker(
                NewSticker.Vehicles,
                customer,
                stickerType,
                DateTime.Now,
                NewSticker.ValidFrom,
                stickerType.Price
            );

            _db.Stickers.Add(newSticker);

            try
            {
                _db.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return Page();
            }

            return RedirectToPage("/Stickers/Index", new { CustomerGuid = customer.Guid});
        }

    }
}