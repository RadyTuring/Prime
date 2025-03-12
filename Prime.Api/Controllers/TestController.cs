using Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Prime.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : BaseController
    {
        public TestController()
        {
            
        }
        public async Task<IActionResult> Index()
        {
            var res = _unitOfWork.Books.GetAll();

            return Ok(res);
        }
    }
}
