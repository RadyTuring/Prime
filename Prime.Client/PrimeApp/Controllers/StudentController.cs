using ApiCall;
using Microsoft.AspNetCore.Mvc;

namespace Prime.Client.Controllers
{
    public class StudentController : BaseController
    {
        public StudentController(IAppService api) : base(api)
        {
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
