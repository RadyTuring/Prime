using Dto;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Prime.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public SubjectController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            return Ok(_unitOfWork.Subjects.GetAll());
        }

        [HttpGet("GetAllAsync")]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _unitOfWork.Subjects.GetAllAsync());
        }
        [HttpGet("GetById")]
        public IActionResult GetById(int id)
        {
            return Ok(_unitOfWork.Subjects.GetById(id));
        }

        [HttpGet("GetByIdAsync")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            if (id == 0)
                return BadRequest();
            else
                return Ok(await _unitOfWork.Subjects.GetByIdAsync(id));
        }
        [HttpPost("Add")]
        public IActionResult Create(SubjectDto dto)
        {
            Subject Subject = new Subject()
            {
                SubjectName = dto.SubjectName
            };
            var m = _unitOfWork.Subjects.Add(Subject);
            _unitOfWork.Save();
            return Ok(m);
        }
        [HttpPost("AddSubjects")]
        public IActionResult Create(IEnumerable<Subject> Subjects)
        {
            var m = _unitOfWork.Subjects.AddRange(Subjects);
            _unitOfWork.Save();
            return Ok(m);
        }
        [HttpPut("Update")]
        public IActionResult Update(Subject Subject)
        {
            var m = _unitOfWork.Subjects.Update(Subject);
            _unitOfWork.Save();
            return Ok(m);
        }
        [HttpDelete("Delete")]
        public IActionResult Delete(int id)
        {
            var o=_unitOfWork.Subjects.GetById(id);
            if (o == null)
            {
                return BadRequest("The Subject is not Exist");
            }
            _unitOfWork.Subjects.Delete(o);
            _unitOfWork.Save();
            return Ok();
        }

        [HttpDelete("DeleteSubjects")]
        public IActionResult Delete(IEnumerable<Subject> Subjects)
        {
            _unitOfWork.Subjects.DeleteRange(Subjects);
            _unitOfWork.Save();
            return Ok();
        }
        [HttpGet("SearchByName")]
        public IEnumerable<Subject> GetSubjects(string name)
        {
            return (_unitOfWork.Subjects.FindAll(m => m.SubjectName.Contains(name)));
        }


    }
}
