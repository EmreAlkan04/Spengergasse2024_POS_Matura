using AspShowcase.Application.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspShowcase.Webapp.Pages.Teams
{
    public class IndexModel : PageModel
    {
        public record AllTeamDto(
            Guid Guid, string Name, string Schoolclass, int NumberOfTasks);
        private readonly AspShowcaseContext _db;
        public List<AllTeamDto> Teams { get; private set; } = new();

        public IndexModel(AspShowcaseContext context)
        {
            _db = context;
        }

        public void OnGet()
        {
            var teams = _db.Teams
                .OrderBy(t => t.Name)
                .Select(
                t => new AllTeamDto(t.Guid, t.Name, t.Schoolclass, t.Tasks.Count))
                .ToList();
            Teams = teams;
        }
    }
}
