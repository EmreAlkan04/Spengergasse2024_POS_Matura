using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SPG_Fachtheorie.Aufgabe2;
using SPG_Fachtheorie.Aufgabe2.Model;
using SPG_Fachtheorie.Aufgabe3RazorPages.Services;

namespace SPG_Fachtheorie.Aufgabe3RazorPages.Pages.Offers;

public class IndexModel : PageModel
{
    private readonly AppointmentContext _db;
    private readonly AuthService _authservice;
    public IndexModel(AppointmentContext db, AuthService authservice)
    {
        _db = db;
        _authservice = authservice;
    }

    public record OfferDTO(
        int Term, string Name, string EducationType,
        DateTime From, DateTime To, int CountofAppointments, Guid Id
        );

    public List<OfferDTO> OffersList { get; set; } = default!; 
    
    public void OnGet()
    {
        var offer = _db.Offers
            .Include(s => s.Subject)
            .Include(s => s.Teacher)
            .Where(s => s.Teacher.Username == _authservice.Username)
            .Select(s => new OfferDTO(
                s.Subject.Term, s.Subject.Name, s.Subject.EducationType,
                s.From, s.To, s.Appointments.Count(), s.Id
            ))
            .ToList();
        OffersList = offer;
    }
}

