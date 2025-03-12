using ApiCall;
using Dto;
using Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Prime.Client.Controllers
{
    [Authorize(Roles = "4", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme + "," + JwtBearerDefaults.AuthenticationScheme)]
    public class BookServiceController : BaseController
    {
        public BookServiceController(IAppService api) : base(api)
        {
        }

        public async Task<IActionResult> Index()
        {
            var res = _api.Get<BookService>("BookService/GetAllAsync");
            return res != null ? View(res) : Problem("Book Service is null.");
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var res = _api.GetById<BookService>("BookService/GetByIdAsync?id=" + id);

            if (res == null)
            {
                return NotFound();
            }

            return View(res);
        }

        // GET: BookService/Create
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookServiceDto model)
        {
            var res = await _api.GetValAsync($"BookService/IsExist?name={model.ServiceName}");
            if (res == true)
            {
                ModelState.AddModelError("k1", "The Book Service already exists");
            }
            if (ModelState.IsValid)
            {
                _api.Create("BookService/Add", model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var res = _api.GetById<BookService>("BookService/GetByIdAsync?id=" + id);

            if (res == null)
            {
                return NotFound();
            }
            return View(res);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BookService model)
        {
            var res = await _api.GetValAsync($"BookService/IsExist?name={model.ServiceName}");
            if (res == true)
            {
                ModelState.AddModelError("k1", "The Book Service already exists");
            }
            if (ModelState.IsValid)
            {
                _api.Update<BookService>("BookService/Update", model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: BookService/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var res = _api.GetById<BookService>("BookService/GetByIdAsync?id=" + id);

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
            
            var res = _api.GetById<BookService>("BookService/GetByIdAsync?id=" + id);
            if (res != null)
            {
                _api.Delete("BookService/Delete", id);
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
