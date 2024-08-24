using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SPG_Fachtheorie.Aufgabe2.Domain;
using SPG_Fachtheorie.Aufgabe2.Infrastructure;
using SPG_Fachtheorie.Aufgabe3.RazorPages.Classes;

namespace SPG_Fachtheorie.Aufgabe3.RazorPages.Pages.Playlists
{
    public class IndexModel : PageModel
    {
        private readonly PodcastContext _db;
        //private readonly AuthService _authService;

        public IndexModel(PodcastContext db)
        {
            _db = db;
        }

        public record PlayListDTO(string Name, string UserName, int Items);
        public List<PlayListDTO> playListDTOs { get; set; }

        public void OnGet()
        {
            var playlist = _db.Playlists
                .Include(s => s.ListenedItems)
                .ThenInclude(s => s.Item)
                //.Where(s => s.ListenedItems.Any(a => a.Item.))
                .Select(s => new PlayListDTO(s.Name, s.UserName, s.ListenedItems.Count(s => s.Item is Podcast)))
                .ToList();
            playListDTOs = playlist;
        }
    }
}