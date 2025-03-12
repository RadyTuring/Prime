using BaseApi;
using Dto;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;
using System.Data;

namespace Prime.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class PracticeTypeController : ApiBaseController
    {
        public PracticeTypeController(IUnitOfWork unitOfWork, IAppService appService) : base(unitOfWork, appService)
        {
        }

        [HttpGet("GetAll")]
        [Authorize(Roles = "1,2,3,4")]
        public IActionResult GetAll()
        {
            return Ok(_unitOfWork.PracticeTypes.GetAll());
        }

        [HttpGet("GetAllAsync")]
        [Authorize(Roles = "1,2,3,4")]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _unitOfWork.PracticeTypes.GetAllAsync());
        }
        [HttpGet("GetById")]
        [Authorize(Roles = "1,2,3,4")]
        public IActionResult GetById(int id)
        {
            return Ok(_unitOfWork.PracticeTypes.GetById(id));
        }

        [HttpGet("GetByIdAsync")]
        [Authorize(Roles = "1,2,3,4")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            if (id == 0)
                return BadRequest();
            else
                return Ok(await _unitOfWork.PracticeTypes.GetByIdAsync(id));
        }
        [HttpPost("Add")]
        [Authorize(Roles = "4")]
        public IActionResult Add(PracticeTypeDto dto)
        {
            PracticeType PracticeType = new PracticeType()
            {
                TypeName = dto.TypeName
            };
            var m = _unitOfWork.PracticeTypes.Add(PracticeType);
            _unitOfWork.Save();
            return Ok(m);
        }

        [HttpPut("Update")]
        [Authorize(Roles = "4")]
        public IActionResult Update(PracticeType PracticeType)
        {
            var m = _unitOfWork.PracticeTypes.Update(PracticeType);
            _unitOfWork.Save();
            return Ok(m);
        }

        [HttpDelete("Delete")]
        [Authorize(Roles = "4")]
        public IActionResult Delete(int id)
        {
            var o = _unitOfWork.PracticeTypes.GetById(id);
            if (o == null)
            {
                return BadRequest("The practice type does not exist.");
            }
            _unitOfWork.PracticeTypes.Delete(o);
            _unitOfWork.Save();
            return Ok();
        }

        [HttpGet("SearchByName")]
        [Authorize(Roles = "1,2,3,4")]
        public IActionResult SearchByName(string name)
        {
            return Ok(_unitOfWork.PracticeTypes.FindAll(m => m.TypeName.Contains(name)));
        }
        [HttpGet("IsExist")]
        [Authorize(Roles = "1,2,3,4")]
        public IActionResult IsExist(string name)
        {
            return Ok(_unitOfWork.PracticeTypes.IsExist(m => m.TypeName == name));
        }
    }
}
