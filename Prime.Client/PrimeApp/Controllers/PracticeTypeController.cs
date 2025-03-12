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
    public class PracticeTypeController : BaseController
    {
        public PracticeTypeController(IAppService api) : base(api)
        {
        }

        public async Task<IActionResult> Index()
        {
            var res = _api.Get<PracticeType>("PracticeType/GetAllAsync");
            return res != null ? View(res) : Problem("Practice Type is null.");
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var res = _api.GetById<PracticeType>("PracticeType/GetByIdAsync?id=" + id);

            if (res == null)
            {
                return NotFound();
            }

            return View(res);
        }

        // GET: PracticeType/Create
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PracticeTypeDto model)
        {
            var res = await _api.GetValAsync($"PracticeType/IsExist?name={model.TypeName}");
            if (res == true)
            {
                ModelState.AddModelError("k1", "The Practice Type already exists");
            }
            if (ModelState.IsValid)
            {
                _api.Create("PracticeType/Add", model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var res = _api.GetById<PracticeType>("PracticeType/GetByIdAsync?id=" + id);

            if (res == null)
            {
                return NotFound();
            }
            return View(res);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PracticeType model)
        {
            var res = await _api.GetValAsync($"PracticeType/IsExist?name={model.TypeName}");
            if (res == true)
            {
                ModelState.AddModelError("k1", "The Practice Type already exists");
            }
            if (ModelState.IsValid)
            {
                _api.Update<PracticeType>("PracticeType/Update", model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: PracticeType/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var res = _api.GetById<PracticeType>("PracticeType/GetByIdAsync?id=" + id);

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
            
            var res = _api.GetById<PracticeType>("PracticeType/GetByIdAsync?id=" + id);
            if (res != null)
            {
                _api.Delete("PracticeType/Delete", id);
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
