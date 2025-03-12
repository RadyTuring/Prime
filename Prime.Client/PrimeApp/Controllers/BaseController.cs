using ApiCall;
using Microsoft.AspNetCore.Mvc;

namespace Prime.Client.Controllers
{
    public class BaseController : Controller
    {
        protected readonly IAppService _api;
        public BaseController(IAppService api)
        {
            _api = api;
           

        }
    }
}
