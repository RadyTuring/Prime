using ApiCall;
using Entities;
using Microsoft.AspNetCore.Mvc;

namespace Prime.Client.Controllers
{
    public class LogController : BaseController
    {
        public LogController(IAppService api) : base(api)
        {
        }

        public IActionResult GetLog(int id)
        {
            var res = _api.Get<Log>("User/GetLog?userId=" + id);
            return res != null ? View(res) : Problem("Log is null.");
        }
    }
}
