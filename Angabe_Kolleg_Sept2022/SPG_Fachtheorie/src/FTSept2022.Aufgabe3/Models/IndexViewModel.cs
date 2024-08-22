using FTSept2022.Aufgabe2.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FTSept2022.Aufgabe3.Models
{
    public record IndexViewModel(
        List<Student> Students,
        List<SelectListItem> UserItems,
        string CurrentUser,
        string SelectedUser);
}
