using AspShowcase.Application.Commands;
using AspShowcase.Application.Dtos;
using AspShowcase.Application.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace AspShowcase.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly TaskService _service;
        private readonly IMapper _mapper;  // braucht builder.Services.AddAutoMapper(typeof(MappingProfile));

        public TasksController(IMapper mapper, TaskService service)
        {
            _mapper = mapper;
            _service = service;
        }

        /// <summary>
        /// Default GET Route, also /api/tasks?expiresAfter=2023-01-02
        /// </summary>
        [HttpGet]
        public IActionResult GetAllTasks([FromQuery] DateTime? expiresAfter)
        {
            expiresAfter ??= DateTime.MinValue;  // Zuweisung nur wenn der Wert NULL ist.

            // SELECT * FROM Tasks INNER JOIN Teacher ON (TeacherId = Id)
            // ORDER BY ExpirationDate

            var tasks = _mapper.ProjectTo<TaskDto>(
                _service.Tasks
                    .Where(t => t.ExpirationDate >= expiresAfter)
                    .OrderBy(t => t.ExpirationDate))
                .ToList();
            return Ok(tasks);  // HTTP 200 + Payload
        }

        /// <summary>
        /// Reagiert auf /api/tasks/82AA8096-4CC6-B320-0A34-FECE6C209F5B
        /// </summary>
        [HttpGet("{guid}")]
        public IActionResult GetTaskById(Guid guid)
        {
            var task = _mapper.ProjectTo<TaskDto>(_service.Tasks.Where(t => t.Guid == guid))
                .FirstOrDefault();
            if (task is null) { return NotFound(); }
            return Ok(task);
        }

        /// <summary>
        /// INSERT
        /// Reagiert auf POST /api/tasks
        /// </summary>
        [HttpPost]
        public IActionResult AddTask([FromBody] NewTaskCmd cmd)
        {
            try
            {
                var guid = _service.AddTask(cmd);
                return CreatedAtAction(nameof(AddTask), new { Guid = guid });
            }
            catch (ServiceException e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// UPDATE
        /// Reagiert auf PUT /api/tasks/1234-56-78900
        /// </summary>
        [HttpPut("{guid}")]
        public IActionResult UpdateTask(Guid guid, [FromBody] EditTaskCmd cmd)
        {
            if (guid != cmd.Guid) { return BadRequest(); }
            try
            {
                _service.EditTask(cmd);
                return NoContent();
            }
            catch (ServiceException e) when (e.NotFound)
            {
                return NotFound();
            }
            catch (ServiceException e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// DELETE
        /// Reagiert auf DELETE /api/tasks/1234-56-78900
        /// </summary>
        [HttpDelete("{guid}")]
        public IActionResult DeleteTask(Guid guid)
        {
            try
            {
                _service.DeleteTask(guid, force: false);
                return NoContent();
            }
            catch (ServiceException e) when (e.NotFound)
            {
                return NotFound();
            }
            catch (ServiceException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}