using ApiCall;
using Dto;
using Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MySignalR;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Security.AccessControl;
using System.Text;
using ViewModels;

namespace Prime.Client.Controllers
{
    [Authorize(Roles = "4", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme + "," + JwtBearerDefaults.AuthenticationScheme)]
    public class NoteController : BaseController
    {
        public NoteController(IAppService api) : base(api)
        {
        }
        public async Task<IActionResult> Index(string readStatus ="1" )
        {
            ViewBag.readStatus = readStatus;
            
            if (readStatus == "1")
                return View(_api.Get<Notification>("NoteBack/GetUnRead"));
            else if (readStatus == "3")
                return View(_api.Get<Notification>("NoteBack/GetAll"));
            else
                return View(_api.Get<Notification>("NoteBack/GetRead"));
        }

        public  IActionResult SendNote(int id)
        {
            var res = _api.GetById<User>($"user/GetUserDataById?userId={id}");
            ViewBag.username = res.FullName;
            ViewBag.userid = res.UserId;
            return View();
        }
        [HttpPost]
        public IActionResult SendNote(NoteDto model)
        {
            if (ModelState.IsValid)
            {
                _api.Create("NoteBack/SendToUser", model);
                return RedirectToAction(actionName: "FilterUsers", controllerName:"Manager");
            }
            return View(model);
        }
    }
}
