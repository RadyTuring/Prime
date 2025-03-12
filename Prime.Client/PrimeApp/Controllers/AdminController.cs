using ApiCall;
using Datatable;
using DataTablesFilters;
using DataTablesHelper;
using Dto;
using Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text;
using ViewModels;

namespace Prime.Client.Controllers
{
    [Authorize(Roles = "3,4", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme + "," + JwtBearerDefaults.AuthenticationScheme)]
    public class AdminController : BaseController
    {
        public AdminController(IAppService api) : base(api)
        {
        }
        public IActionResult index()
        {
             
            var res = _api.Get<AdminBooksV>("Admin/GetAdminBooks");
            string[] images = new string[res.Count()];
            int i = 0;  
            foreach (var o in res)
            {
                images[i]= (GetBookImage((int)o.BookId));
                i++;
            }
            ViewBag.images = images;    
            return res != null ? View(res) : Problem("No Books Added.");
        }
        public IActionResult GetTeachers()
        {
            var res = _api.Get<AdminTeachersV>("Admin/GetTeachers");
            return res != null ? View(res) : Problem("No Teachers Added.");
        }

        [HttpPost]
        public  IActionResult UnAssignAdminFromTeacherCode(string tCode)
        {
            if (tCode==null)
            {
                return View();
            }
            StringUpdateDto dto = new StringUpdateDto()
            {
                 StringValue = tCode
            };
            var res =  _api.Execute<FeedBackReturn>("Admin/UnAssignAdminFromTeacherCode", dto).Result;
            if (!res.IsSuccess)
            {
                TempData["error"] = res.Message;
                return Json(new { success = false, message = res.Message });
            }

            TempData["success"] = "Teacher code unassigned successfully.";

            return Json(new { success = true, message = "Teacher code unassigned successfully." });
        }
       
       

        #region Add_Import_Codes
        public IActionResult AddTeacherCode()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddTeacherCode(TeacherCodeAdminDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var res = await _api.Execute<FeedBackReturn>("Admin/AddTeacherCode", model);
            if (!res.IsSuccess)
            {
                ModelState.AddModelError("TeacherCode", res.Message);
                TempData["error"] = res.Message;
                return View(model);
            }

            TempData["success"] = "Data Saved";
            return RedirectToAction(nameof(AddTeacherCode));

        }

        public IActionResult Import()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Import([FromForm] TeacherCodeAdminDtoCsv model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var res = _api.Create2<FeedBackReturn>("Admin/AddTeacherCodesFromCSV", model);
            if (!res.IsSuccess)
            {
                ModelState.AddModelError("TeacherCodeFile", res.Message);
                TempData["error"] = res.Message;
                return View(model);
            }
            TempData["success"] = "Data imported";
            return RedirectToAction(actionName: "TeachersCodes", controllerName:"Filter");
        }
        #endregion

       
        #region Private_Methods
        private string GetBookImage(int bookId)
        {
            // Get the base64 image data from the API
            var imageData = _api.GetImage($"Book/GetBookImage?bookId={bookId}");

            if (string.IsNullOrEmpty(imageData))
            {
                return "data:image/jpeg;base64,default-base64-placeholder-image";
            }

            return $"data:image/jpeg;base64,{imageData}";
        }
        #endregion
    }
}
