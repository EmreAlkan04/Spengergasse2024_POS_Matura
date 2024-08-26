using Microsoft.AspNetCore.Connections.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using SPG_Fachtheorie.Aufgabe2.Infrastructure;
using SPG_Fachtheorie.Aufgabe2.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SPG_Fachtheorie.Aufgabe3.RazorPages.Pages.Stickers
{
    public class AddModel : PageModel
    {
        private readonly StickerContext _db;

        public AddModel(StickerContext db)
        {
            _db = db;
        }
        [FromRoute]
        public Guid CustomerGuid { get; set; }
        public record NewStickerCmd(
            int VehicleId,
            int StickerTypeId,
            DateTime ValidFrom
            );
        [BindProperty]
        public NewStickerCmd Command { get; set; } = default!;
        public Customer Customer { get; set; } = default!;
        public List<SelectListItem> Vehichles => _db.Vehicles
            //.Include(s => s.VehicleType)
            .Where(s => s.Customer.Guid == CustomerGuid)
            .Select(s => new SelectListItem(s.VehicleInfo, s.Id.ToString()))
            .ToList();
        public List<SelectListItem> StickerTypes => _db.StickerTypes
            //.Include(s => s.VehicleType)
            .Select(s => new SelectListItem(s.Name, s.Id.ToString()))
            .ToList();

        public IActionResult OnGet()
        {
            var customer = _db.Customers
                .Include(s => s.Vehicles)
                .FirstOrDefault(s => s.Guid == CustomerGuid);
            if (customer == null) return NotFound();
            Customer = customer;
            return Page();
        }
        public IActionResult OnPost()
        {
            var customer = _db.Customers
                .Include(s => s.Vehicles)
                .Include(s => s.Stickers)
                .ThenInclude(s => s.StickerType)
                .FirstOrDefault(s => s.Guid == CustomerGuid);
            if (customer == null) return NotFound();
            Customer = customer;

            //if (!ModelState.IsValid) return Page();
            var vehicle = _db.Vehicles
                .Include(s => s.Customer)
                .ThenInclude(s => s.Stickers)
                .ThenInclude(s => s.StickerType)
                .FirstOrDefault(s => s.Id == Command.VehicleId);
            if (vehicle == null) return NotFound();
            
            var stickerType = _db.StickerTypes
                //.Include(s => s.VehicleType)
                .FirstOrDefault(s => s.Id == Command.StickerTypeId);
            if (stickerType == null) return NotFound();
            
            if (vehicle.VehicleType != stickerType.VehicleType) 
            { 
                ModelState.AddModelError("Command.StickerTypeId", "StickerType does not match VehicleType");
                return Page();
            }
            if(Command.ValidFrom < DateTime.Now.Date)
            {
                ModelState.AddModelError("Command.ValidFrom", "ValidFrom must be in the future");
                return Page();
            
            }
            var newSticker = new Sticker(
                vehicle.Numberplate,
                Customer, 
                stickerType,
                DateTime.Now.Date, 
                Command.ValidFrom, 
                stickerType.Price
                );
            if (newSticker is null) return NotFound();
            
            _db.Add(newSticker);
            try
            {
                _db.SaveChanges();
            }
            catch(DbUpdateException ex)
            {
                ModelState.AddModelError("A Fehler", ex.Message);
                return Page();
            }
            return RedirectToPage("/Stickers/Index", new { CustomerGuid });
        }
    }
}