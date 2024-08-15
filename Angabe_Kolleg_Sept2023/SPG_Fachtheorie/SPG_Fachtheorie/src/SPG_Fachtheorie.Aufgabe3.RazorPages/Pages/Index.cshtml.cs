using Microsoft.AspNetCore.Mvc.RazorPages;
using SPG_Fachtheorie.Aufgabe2.Infrastructure;

namespace SPG_Fachtheorie.Aufgabe3.RazorPages.Pages
{
    public class IndexModel : PageModel
    {
        private readonly StickerContext _db;

        public IndexModel(StickerContext db)
        {
            _db = db;
        }

        public void OnGet()
        {
        }
    }
}