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

    public class BookServiceController : ApiBaseController
    {
        public BookServiceController(IUnitOfWork unitOfWork, IAppService appService) : base(unitOfWork, appService)
        {
        }

        [HttpGet("GetAll")]
        [Authorize(Roles = "1,2,3,4")]
        public IActionResult GetAll()
        {
            return Ok(_unitOfWork.BookServices.GetAll());
        }

        [HttpGet("GetAllAsync")]
        [Authorize(Roles = "1,2,3,4")]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _unitOfWork.BookServices.GetAllAsync());
        }
        [HttpGet("GetById")]
        [Authorize(Roles = "1,2,3,4")]
        public IActionResult GetById(int id)
        {
            return Ok(_unitOfWork.BookServices.GetById(id));
        }

        [HttpGet("GetByIdAsync")]
        [Authorize(Roles = "1,2,3,4")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            if (id == 0)
                return BadRequest();
            else
                return Ok(await _unitOfWork.BookServices.GetByIdAsync(id));
        }
        [HttpPost("Add")]
        [Authorize(Roles = "4")]
        public IActionResult Add(BookServiceDto dto)
        {
            BookService BookService = new BookService()
            {
                 ServiceName = dto.ServiceName
            };
            var m = _unitOfWork.BookServices.Add(BookService);
            _unitOfWork.Save();
            return Ok(m);
        }

        [HttpPut("Update")]
        [Authorize(Roles = "4")]
        public IActionResult Update(BookService BookService)
        {
            var m = _unitOfWork.BookServices.Update(BookService);
            _unitOfWork.Save();
            return Ok(m);
        }

        [HttpDelete("Delete")]
        [Authorize(Roles = "4")]
        public IActionResult Delete(int id)
        {
            var o = _unitOfWork.BookServices.GetById(id);
            if (o == null)
            {
                return BadRequest("The book services type does not exist.");
            }
            _unitOfWork.BookServices.Delete(o);
            _unitOfWork.Save();
            return Ok();
        }

        [HttpGet("SearchByName")]
        [Authorize(Roles = "1,2,3,4")]
        public IActionResult SearchByName(string name)
        {
            return Ok (_unitOfWork.BookServices.FindAll(m => m.ServiceName.Contains(name)));
        }
        [HttpGet("IsExist")]
        [Authorize(Roles = "1,2,3,4")]
        public IActionResult IsExist(string name)
        {
            return Ok (_unitOfWork.BookServices.IsExist(m => m.ServiceName==name));
        }

    }
}
