using SPG_Fachtheorie.Aufgabe2.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace SPG_Fachtheorie.Aufgabe3.Models
{
    public record IndexViewModel(
        List<Admin> Admins,
        List<SelectListItem> UserItems,
        int CurrentUser,
        int SelectedUser);
}

