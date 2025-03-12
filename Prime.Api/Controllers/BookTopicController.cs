using Dto;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Prime.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "manager,student,teacher,admin")]
    public class BookTopicController : ControllerBase
    {
       
        private readonly IUnitOfWork _unitOfWork;
        public BookTopicController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
       
        [HttpGet("Get")]
        public IActionResult Get(int bookId)
        {
            return Ok(_unitOfWork.BookTopics.FindAll(m=>m.BookId== bookId));
        }

        [HttpGet("GetAsync")]
        public async Task<IActionResult> GetAsync(int bookId)
        {
            return Ok(await _unitOfWork.BookTopics.FindAllAsync(m => m.BookId == bookId));
        }
       
        [HttpGet("SearchByByName")]
        public IEnumerable<BookTopic> GetBookTopics(string name)
        {
            return (_unitOfWork.BookTopics.FindAll(m => m.Topic.Contains(name)));
        }

       
    }
}
