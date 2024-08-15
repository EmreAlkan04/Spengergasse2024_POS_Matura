using Microsoft.AspNetCore.Connections.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SPG_Fachtheorie.Aufgabe2.Infrastructure;
using SPG_Fachtheorie.Aufgabe2.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SPG_Fachtheorie.Aufgabe3.RazorPages.Pages.Stickers
{
    public class IndexModel : PageModel
    {
        private readonly StickerContext _db;
        public CustomerDTO Customer { get; set; } = default!;
        public IndexModel(StickerContext db)
        {
            _db = db;
        }

        //Customer hat mehrere Sticker, deshalb wird ein DTO in einem DTO erstellt
        public record CustomerDTO(string Firstname,
            string Lastname, string Email, List<StickerDTO> Stickers);
        public record StickerDTO(string Numberplate, 
            StickerType StickerType, 
            DateTime PurchaseDate,
            DateTime ValidFrom,
            decimal Price);

        [FromRoute]
        public Guid CustomerGuid { get; set; }

        public IActionResult OnGet()
        {
            //var customer = _db.Customers
            //      .Include(s => s.Stickers)
            //      .ThenInclude(s => s.StickerType)
            //      .Where(s => s.Id == CustomerId)
            //      .FirstOrDefault();

            var customer = _db.Customers
                .Where(c => c.Guid == CustomerGuid)
                .Select(c => new CustomerDTO(c.Firstname, c.Lastname, c.Email,
                    c.Stickers.Select(s => new StickerDTO(s.Numberplate, s.StickerType, s.PurchaseDate, s.ValidFrom, s.Price)).ToList()))
                .FirstOrDefault();
                  

            if (customer is null)
            {
                return NotFound();
            }
            Customer = customer;
            return Page();
                
        }
    }
}