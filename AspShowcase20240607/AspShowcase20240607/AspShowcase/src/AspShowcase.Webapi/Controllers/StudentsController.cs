using AspShowcase.Application.Dtos;
using AspShowcase.Application.Infrastructure;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace AspShowcase.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly AspShowcaseContext _db;
        private readonly IMapper _mapper;

        public StudentsController(AspShowcaseContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public ActionResult<AllStudentDto> GetAllStudents()
        {
            var students = _mapper.ProjectTo<AllStudentDto>(_db.Students
                .OrderBy(s => s.Lastname)
                .ThenBy(s => s.Firstname)).ToList();
            return Ok(students);
        }
    }
}
