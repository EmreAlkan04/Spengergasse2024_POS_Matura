using AspShowcase.Application.Dtos;
using AspShowcase.Application.Infrastructure;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AspShowcase.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly AspShowcaseContext _db;
        private readonly IMapper _mapper;
        public TeamsController(AspShowcaseContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<List<AllTeamDto>> GetAllTeams()
        {
            var teams = _mapper.ProjectTo<AllTeamDto>(_db
                    .Teams
                    .OrderBy(t => t.Schoolclass).ThenBy(t => t.Name))
                .ToList();
            return Ok(teams);
        }

        [HttpGet("{guid}")]
        public ActionResult<TeamDetailsDto> GetTeamById(Guid guid)
        {
            var team = _mapper
                .ProjectTo<TeamDetailsDto>(_db.Teams.Where(t => t.Guid == guid))
                .FirstOrDefault();
            if (team == null)
                return NotFound();
            return Ok(team);
        }
    }
}
