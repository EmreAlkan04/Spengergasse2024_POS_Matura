using System;
using System.ComponentModel.DataAnnotations;

namespace AspShowcase.Application.Commands
{
    //Es dürfen nur die Werte von MaxPoints, Title und Subject bearbeitet werden.
    public record EditTaskCmd(
        Guid Guid,
        [StringLength(16, MinimumLength = 1, ErrorMessage = "Ungültiges Subject")] string Subject,
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Ungültiger Title")] string Title,
        [Range(1, 999, ErrorMessage = "Ungültige maximale Punkteanzahl")] int? MaxPoints);
}