using AspShowcase.Application.Commands;
using AspShowcase.Application.Models;
using AutoMapper;

namespace AspShowcase.Application.Dtos
{
    public class MappingProfile : Profile  // using AutoMapper;
    {
        public MappingProfile()
        {
            // Ich kann aus einer Instanz einer Klasse Task automatisch
            // eine Instanz der Klasse TaskDto erzeugen lassen.
            CreateMap<Task, TaskDto>();
            CreateMap<Handin, HandinDto>();
            CreateMap<NewHandinCmd, Handin>();
            CreateMap<EditHandinCmd, Handin>();
            CreateMap<Team, AllTeamDto>();
            CreateMap<Student, AllStudentDto>();
            CreateMap<Team, TeamDetailsDto>();
        }
    }
}