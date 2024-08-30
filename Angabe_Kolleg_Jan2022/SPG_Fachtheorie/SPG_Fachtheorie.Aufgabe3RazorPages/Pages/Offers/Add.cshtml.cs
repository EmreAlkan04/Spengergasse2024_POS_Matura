using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using SPG_Fachtheorie.Aufgabe2;
using SPG_Fachtheorie.Aufgabe2.Model;
using SPG_Fachtheorie.Aufgabe3RazorPages.Services;

namespace SPG_Fachtheorie.Aufgabe3RazorPages.Pages.Offers;

public class AddModel : PageModel
{
    private readonly AppointmentContext _db;
    private readonly AuthService _authservice;

    public AddModel(AppointmentContext db, AuthService authservice)
    {
        _db = db;
        _authservice = authservice;
    }

    public record OfferCmd(
        Guid SubjectId,
        DateTime From,
        DateTime To
        //Teacher?
        );
    [BindProperty]
    public OfferCmd Command { get; set; } = default!;
    public Offer NewOffer { get; set; } = default!;
    public List<SelectListItem> Subjects => _db.Subjects
        .Select(s => new SelectListItem($"{s.Term} - {s.Name} - {s.EducationType}", s.Id.ToString())).ToList();

    public IActionResult OnGet()
    {
        var offer = _db.Offers
            .Include(s => s.Teacher)
            .Include(s => s.Subject)
            .FirstOrDefault(s => s.Teacher.Username == _authservice.Username);
        if (offer is null) return NotFound();
        return Page();
    }
    public IActionResult OnPost()
    {
        var offer = _db.Offers
            .Include(s => s.Teacher)
            .Include(s => s.Subject)
            .FirstOrDefault(s => s.Teacher.Username == _authservice.Username);
        if (offer is null) return NotFound();
        if (!ModelState.IsValid) return Page();
        if(Command.From >= Command.To) return Page();
        var subject = _db.Subjects.FirstOrDefault(s => s.Id == Command.SubjectId);
        if (subject is null) return NotFound();
        var newOffer = new Offer
        {
            From = Command.From,
            Subject = subject,
            Teacher = offer.Teacher,
            To = Command.To
        };
        _db.Add(newOffer);
        try
        {
            _db.SaveChanges();
        }
        catch(DbUpdateException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return Page();
        }
        return RedirectToPage("/Offers/Index");
    }
}

