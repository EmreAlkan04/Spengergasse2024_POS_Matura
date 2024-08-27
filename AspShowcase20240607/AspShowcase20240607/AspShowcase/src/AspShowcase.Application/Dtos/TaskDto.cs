using System;

namespace AspShowcase.Application.Dtos
{
    public record TaskDto(
        Guid Guid,
        string Title,
        string Subject,
        DateTime ExpirationDate,
        int? MaxPoints,
        string TeacherFirstname,
        string TeacherLastname,
        string TeacherEmail,
        string TeacherInitials,
        string TeamName);
}