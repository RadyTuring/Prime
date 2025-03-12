using ApiCall;
using Dto;
using Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Text;

namespace Prime.Client.Controllers
{
    [Authorize(Roles = "manager", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme + "," + JwtBearerDefaults.AuthenticationScheme)]
    public class Order : BaseController
    {
        public Order(IConsumeApi api) : base(api)
        {
        }
        public async Task<IActionResult> Index()
        {
            var res = _api.Get<Book>("Book/GetAllAsync");
            return res != null ? View(res) : Problem("Book is null.");
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var res = _api.GetById<Book>("Book/GetByIdAsync?id=" + id);

            if (res == null)
            {
                return NotFound();
            }

            return View(res);
        }

        // GET: Book/Create
        public IActionResult Create()
        {
           
          
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( BookDto model)
        {
 
            if (ModelState.IsValid)
            {
                 _api.CreateWithFile("Book/Add", model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
 
           
            var res = _api.GetById<Book>("Book/GetByIdAsync?id=" + id);

            if (res == null)
            {
                return NotFound();
            }
            string imageurl =  "Mes/GetFile?fileName=" + res.DefaultImage;
             var img=_api.GetImage(imageurl);
            ViewBag.image= img;
            return View(res);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Book model)
        {
 
            if (ModelState.IsValid)
            {
                _api.Update<Book>("Book/Update", model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Book/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
 
            if (id == null)
            {
                return NotFound();
            }

            var res = _api.GetById<Book>("Book/GetByIdAsync?id=" + id.ToString());

            if (res == null)
            {
                return NotFound();
            }

            return View(res);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {

            var res = _api.GetById<Book>("Book/GetByIdAsync?id=" + id);
            if (res != null)
            {
                _api.Delete("Book/Delete", id);
            }
            return RedirectToAction(nameof(Index));
        }

        //private bool BookExists(int id)
        //{
        //    return (_context.Book?.Any(e => e.BookId == id)).GetValueOrDefault();
        //}
      
    }
}
