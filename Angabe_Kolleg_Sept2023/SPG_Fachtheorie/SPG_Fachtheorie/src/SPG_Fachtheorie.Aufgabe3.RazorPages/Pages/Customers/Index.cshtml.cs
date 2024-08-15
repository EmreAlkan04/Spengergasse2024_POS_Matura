using Bogus.DataSets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SPG_Fachtheorie.Aufgabe2.Infrastructure;
using SPG_Fachtheorie.Aufgabe2.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SPG_Fachtheorie.Aufgabe3.RazorPages.Pages.Customers
{
    public class IndexModel : PageModel
    {
        private readonly StickerContext _db;

        public IndexModel(StickerContext db)
        {
            _db = db;
        }

        public record CustomerDTO(string Firstname, string Lastname, List<Aufgabe2.Model.Vehicle> Vehicles, List<Sticker> Stickers, int Id, Guid Guid);
        public List<CustomerDTO> Customers { get; private set; } = new();

        public void OnGet()
        {
            //Alle Customers werden als CustomerDTOs in einer Liste gespeichert
            var customers = _db.Customers.Select(c => new CustomerDTO(c.Firstname, c.Lastname, c.Vehicles, c.Stickers, c.Id, c.Guid)).ToList();
            Customers = customers;
        }
    }
}