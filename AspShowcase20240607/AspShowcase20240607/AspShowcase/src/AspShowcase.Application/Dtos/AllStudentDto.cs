using System;

namespace AspShowcase.Application.Dtos
{
    public record AllStudentDto(Guid Guid, string Firstname,
        string Lastname, string Email);
}
