using System;
using System.ComponentModel.DataAnnotations;

namespace AspShowcase.Application.Commands
{
    public record EditHandinCmd(
        Guid Guid,
        [StringLength(255, MinimumLength = 1)] string description,
        [StringLength(255, MinimumLength = 1)] string documentUrl);
}
