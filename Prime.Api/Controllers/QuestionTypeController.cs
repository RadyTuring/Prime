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

    public class QuestionTypeController : ApiBaseController
    {
        public QuestionTypeController(IUnitOfWork unitOfWork, IAppService appService) : base(unitOfWork, appService)
        {
        }

        [HttpGet("GetAll")]
        [Authorize(Roles = "1,2,3,4")]
        public IActionResult GetAll()
        {
            return Ok(_unitOfWork.QuestionTypes.GetAll());
        }

        [HttpGet("GetAllAsync")]
        [Authorize(Roles = "1,2,3,4")]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _unitOfWork.QuestionTypes.GetAllAsync());
        }
        [HttpGet("GetById")]
        [Authorize(Roles = "1,2,3,4")]
        public IActionResult GetById(int id)
        {
            return Ok(_unitOfWork.QuestionTypes.GetById(id));
        }

        [HttpGet("GetByIdAsync")]
        [Authorize(Roles = "1,2,3,4")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            if (id == 0)
                return BadRequest();
            else
                return Ok(await _unitOfWork.QuestionTypes.GetByIdAsync(id));
        }
        [HttpPost("Add")]
        [Authorize(Roles = "4")]
        public IActionResult Add(QuestionTypeDto dto)
        {
            QuestionType QuestionType = new QuestionType()
            {
                TypeName = dto.TypeName,
                QuestHeader = dto.QuestHeader,
                QuestionDesc = dto.QuestionDesc,

            };
            var m = _unitOfWork.QuestionTypes.Add(QuestionType);
            _unitOfWork.Save();
            return Ok(m);
        }

        [HttpPut("Update")]
        [Authorize(Roles = "4")]
        public IActionResult Update(QuestionType QuestionType)
        {
            var m = _unitOfWork.QuestionTypes.Update(QuestionType);
            _unitOfWork.Save();
            return Ok(m);
        }

        [HttpDelete("Delete")]
        [Authorize(Roles = "4")]
        public IActionResult Delete(int id)
        {
            var o = _unitOfWork.QuestionTypes.GetById(id);
            if (o == null)
            {
                return BadRequest("The Practice type is not Exist");
            }
            _unitOfWork.QuestionTypes.Delete(o);
            _unitOfWork.Save();
            return Ok();
        }

        [HttpGet("SearchByName")]
        [Authorize(Roles = "1,2,3,4")]
        public IActionResult SearchByName(string name)
        {
            return Ok(_unitOfWork.QuestionTypes.FindAll(m => m.TypeName.Contains(name)));
        }
        [HttpGet("IsExist")]
        [Authorize(Roles = "1,2,3,4")]
        public IActionResult IsExist(string name)
        {
            return Ok(_unitOfWork.QuestionTypes.IsExist(m => m.TypeName == name));
        }
    }
}
