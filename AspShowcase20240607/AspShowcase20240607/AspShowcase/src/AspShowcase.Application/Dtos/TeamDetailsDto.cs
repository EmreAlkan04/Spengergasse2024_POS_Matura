using System;
using System.Collections.Generic;

namespace AspShowcase.Application.Dtos
{
    public record TeamDetailsDto(
        Guid Guid, string Name, string Schoolclass,
        List<TaskDto> Tasks);
}
