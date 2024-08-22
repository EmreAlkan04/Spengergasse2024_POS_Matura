using FTSept2022.Aufgabe2.Domain;
using FTSept2022.Aufgabe2.Infrastructure;
using FTSept2022.Aufgabe3.RazorPages.Classes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FTSept2022.Aufgabe3.RazorPages.Pages
{
    public class IndexModel : PageModel
    {
        private readonly UniContext _db;
        private readonly AuthService _authService;
        public IndexModel(UniContext db, AuthService authService)
        {
            _db = db;
            _authService = authService;
        }

        public string RegistrationNumber => _authService.RegistrationNumber;
        public List<Student> Students { get; set; } = new();
        public List<SelectListItem> UserItems { get; set; } = new();
        [BindProperty]
        public string SelectedUser { get; set; } = string.Empty;
        public void OnGet()
        {
            Students = _db.Students.ToList();
            UserItems = Students.Select(s => new SelectListItem
            {
                Text = $"{s.RegistrationNumber} - {s.LastName} {s.FirstName}",
                Value = s.RegistrationNumber
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
            await _authService.TryLogin(SelectedUser);
            return RedirectToPage();
        }
    }
}