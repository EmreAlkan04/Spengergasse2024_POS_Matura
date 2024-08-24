using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SPG_Fachtheorie.Aufgabe2.Domain;
using SPG_Fachtheorie.Aufgabe2.Infrastructure;
using SPG_Fachtheorie.Aufgabe3.RazorPages.Classes;
using System.Runtime.CompilerServices;

namespace SPG_Fachtheorie.Aufgabe3.RazorPages.Pages.Customers
{
    public class IndexModel : PageModel
    {
        private readonly PodcastContext _db;
        private readonly AuthService _authService;

        public IndexModel(PodcastContext db, AuthService authService)
        {
            _db = db;
            _authService = authService;
        }

            //FirstName, LastName, CompanyName und Links
            //auf die Werbungen des Kunden(Advertisement) ausgegeben werden.
        public record CustomerDTO(string FirstName, string LastName, string? CompanyName,
            List<ADDTO> ADDTOs);
        public List<CustomerDTO> CustomersList { get; set; }
        public record ADDTO(int Id, string ProductName, int Length);

        public void OnGet()
        {
            var customers = _db.Customers
                .Include(s => s.Advertisements)
                //.Include(s => s.ResponsibleAdminId)
                .Where(s => s.ResponsibleAdmin.Id == _authService.AdminId)
                .Select(s => new CustomerDTO(s.FirstName, s.LastName, s.CompanyName, 
                s.Advertisements.Select(a => new ADDTO(a.Id, a.ProductName, a.Length)).ToList()
                ))
                .ToList();
            CustomersList = customers;
        }
    }
}