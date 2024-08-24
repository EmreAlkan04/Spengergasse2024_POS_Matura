using SPG_Fachtheorie.Aufgabe2.Infrastructure;
using SPG_Fachtheorie.Aufgabe3.Classes;
using SPG_Fachtheorie.Aufgabe3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;

namespace SPG_Fachtheorie.Aufgabe3.Controllers
{
    public class HomeController : Controller
    {
        private readonly PodcastContext _db;
        private readonly AuthService _authService;

        public HomeController(PodcastContext db, AuthService authService)
        {
            _db = db;
            _authService = authService;
        }

        public IActionResult Index()
        {
            var admins = _db.Admins.ToList();
            var userItems = admins.Select(a => new SelectListItem
            {
                Text = $"{a.LastName} {a.FirstName} - ({a.Id})",
                Value = a.Id.ToString(),
            })
            .ToList();

            return View(new IndexViewModel(
                Admins: admins,
                UserItems: userItems,
                CurrentUser: _authService.AdminId,
                SelectedUser: _authService.AdminId));
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