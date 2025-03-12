using ApiCall;
using Dto;
using Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Prime.Client.Controllers
{
   
    [Authorize(Roles = "manager", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme + "," + JwtBearerDefaults.AuthenticationScheme)]
    public class CountryController : BaseController
    {
        public CountryController(IAppService api) : base(api)
        {
        }
        public async Task<IActionResult> Index()
        {
            
            var res = _api.Get<Country>("Country/GetAllAsync");
            return res != null ? View(res) : Problem("Question Type is null.");
        }
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            

            var res = _api.GetById<Country>("Country/GetByIdAsync?id=" + id);

            if (res == null)
            {
                return NotFound();
            }

            return View(res);
        }

        // GET: Country/Create
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CountryDto model)
        {
            
            if (ModelState.IsValid)
            {
                _api.Create("Country/Add", model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            
            var res = _api.GetById<Country>("Country/GetByIdAsync?id=" + id);

            if (res == null)
            {
                return NotFound();
            }
            return View(res);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Country model)
        {
            
            if (ModelState.IsValid)
            {
              _api.Update<Country>("Country/Update", model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Country/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            
            if (id == null)
            {
                return NotFound();
            }

            var res = _api.GetById<Country>("Country/GetByIdAsync?id=" + id.ToString());

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
            
            var res = _api.GetById<Country>("Country/GetByIdAsync?id=" + id);
            if (res != null)
            {
                _api.Delete("Country/Delete", id);
            }
            return RedirectToAction(nameof(Index));
        }

        //private bool CountryExists(int id)
        //{
        //    return (_context.Country?.Any(e => e.CountryId == id)).GetValueOrDefault();
        //}
        private void refToken()
        {
            var token = Request.Cookies["prmtoken"];
           // _api.SetToken(token);
        }
    }
}
