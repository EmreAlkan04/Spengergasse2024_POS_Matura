using SPG_Fachtheorie.Aufgabe2.Infrastructure;
using SPG_Fachtheorie.Aufgabe2.Domain;
using SPG_Fachtheorie.Aufgabe3.RazorPages.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace SPG_Fachtheorie.Aufgabe3.RazorPages.Pages
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

        public int? CurrentAdmin => _authService.AdminId;
        public List<Admin> Admins { get; set; } = new();
        public List<SelectListItem> AdminItems { get; set; } = new();
        [BindProperty]
        public int SelectedAdminItem { get; set; }
        public void OnGet()
        {
            Admins = _db.Admins.ToList();
            AdminItems = Admins.Select(a => new SelectListItem
            {
                Text = $"{a.LastName} {a.FirstName} - ({a.Id})",
                Value = a.Id.ToString()
            })
            .ToList();
        }

        public async Task<IActionResult> OnPostLogout()
        {
            await _authService.Logout();
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPost()
        {
            await _authService.TryLogin(SelectedAdminItem);
            return RedirectToPage();
        }
    }
}