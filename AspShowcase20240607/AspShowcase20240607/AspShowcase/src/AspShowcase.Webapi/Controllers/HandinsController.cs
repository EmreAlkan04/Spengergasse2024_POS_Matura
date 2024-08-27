using AspShowcase.Application.Commands;
using AspShowcase.Application.Dtos;
using AspShowcase.Application.Models;
using AspShowcase.Application.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace AspShowcase.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HandinsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly HandinService _service;
        public HandinsController(IMapper mapper, HandinService service)
        {
            _mapper = mapper;
            _service = service;
        }

        /// <summary>
        /// /api/handins
        /// /api/handins?studentGuid=(guid)
        /// </summary>
        [HttpGet]
        public IActionResult GetAll([FromQuery] Guid? studentGuid)
        {
            IQueryable<Handin> query = _service.Handins;
            if (studentGuid is not null) { query = query.Where(h => h.Student.Guid == studentGuid); }
            var result = _mapper.ProjectTo<HandinDto>(query).ToList();
            return Ok(result);
        }

        /// <summary>
        /// GET /api/handins/(guid)
        /// </summary>
        [HttpGet("{guid}")]
        public IActionResult GetById(Guid guid)
        {
            var result = _mapper.ProjectTo<HandinDto>(
                _service.Handins.Where(h => h.Guid == guid))
                .FirstOrDefault();
            if (result is null) { return NotFound(); }
            return Ok(result);
        }

        [HttpPost]
        public IActionResult AddHandin(NewHandinCmd handinCmd)
        {
            try
            {
                var guid = _service.AddHandin(handinCmd);
                return CreatedAtAction(nameof(AddHandin), new { Guid = guid });
            }
            catch (ServiceException e) { return BadRequest(e.Message); }
        }

        /// <summary>
        /// PUT /api/handins/(guid)
        /// </summary>
        [HttpPut("{guid}")]
        public IActionResult EditHandin(Guid guid, [FromBody] EditHandinCmd handinCmd)
        {
            if (guid != handinCmd.Guid) { return BadRequest("Invalid GUID."); }
            try
            {
                _service.EditHandin(handinCmd);
                return CreatedAtAction(nameof(AddHandin), new { Guid = guid });
            }
            catch (ServiceException e) { return BadRequest(e.Message); }
        }

        [HttpDelete("{guid}")]
        public IActionResult DeleteHandin(Guid guid)
        {
            var result = _service.DeleteHandin(guid);
            return result switch
            {
                (true, _, _) => NoContent(),
                (_, false, _) => NotFound(),
                (_, _, _) => BadRequest(result.message)
            };
        }
    }
}
