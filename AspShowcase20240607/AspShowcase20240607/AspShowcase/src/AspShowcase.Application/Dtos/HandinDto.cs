using System;

namespace AspShowcase.Application.Dtos
{
    /*
    "guid": "string",
    "created": "string",
    "description": "string",
    "documentUrl": "string",
    "studentGuid": "string",
    "studentFirstname": "string",
    "studentLastname": "string",
    "studentEmail": "string",
    "taskGuid": "string",
    "taskTitle": "string",
    "taskTeacherFirstname": "string",
    "taskTeacherLastname": "string",
    "taskTeacherEmail": "string"
     */
    public record HandinDto(
        Guid Guid, DateTime Created, string Description, string DocumentUrl,
        Guid StudentGuid, string StudentFirstname, string StudentLastname, string StudentEmail,
        Guid TaskGuid, string TaskTitle, string TaskTeacherFirstname, string TaskTeacherLastname, string TaskTeacherEmail);

}
