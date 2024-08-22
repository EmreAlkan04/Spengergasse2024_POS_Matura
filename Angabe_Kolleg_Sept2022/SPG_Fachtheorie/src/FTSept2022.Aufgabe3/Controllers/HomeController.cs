using FTSept2022.Aufgabe2.Infrastructure;
using FTSept2022.Aufgabe3.Classes;
using FTSept2022.Aufgabe3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace FTSept2022.Aufgabe3.Controllers
{
    public class HomeController : Controller
    {
        private readonly UniContext _db;
        private readonly AuthService _authService;

        public HomeController(UniContext db, AuthService authService)
        {
            _db = db;
            _authService = authService;
        }

        public IActionResult Index()
        {
            var students = _db.Students.ToList();
            var userItems = students.Select(s => new SelectListItem
            {
                Text = $"{s.LastName} {s.FirstName} - ({s.RegistrationNumber})",
                Value = s.RegistrationNumber,
            })
            .ToList();

            return View(new IndexViewModel(
                Students: students,
                UserItems: userItems,
                CurrentUser: _authService.RegistrationNumber,
                SelectedUser: _authService.RegistrationNumber));
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _authService.Logout();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Index(string selectedUser)
        {
            await _authService.TryLogin(selectedUser);
            return RedirectToAction();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}