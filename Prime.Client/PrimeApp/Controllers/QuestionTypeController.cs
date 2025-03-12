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
    public class QuestionTypeController : BaseController
    {
        public QuestionTypeController(IAppService api) : base(api)
        {
        }

        public async Task<IActionResult> Index()
        {
            var res = _api.Get<QuestionType>("QuestionType/GetAllAsync");
            return res != null ? View(res) : Problem("Question Type Type is null.");
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var res = _api.GetById<QuestionType>("QuestionType/GetByIdAsync?id=" + id);

            if (res == null)
            {
                return NotFound();
            }

            return View(res);
        }

        // GET: QuestionType/Create
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(QuestionTypeDto model)
        {
            var res = await _api.GetValAsync($"QuestionType/IsExist?name={model.TypeName}");
            if (res == true)
            {
                ModelState.AddModelError("k1", "The Question Type already exists");
            }

            if (ModelState.IsValid)
            {
                _api.Create("QuestionType/Add", model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var res = _api.GetById<QuestionType>("QuestionType/GetByIdAsync?id=" + id);

            if (res == null)
            {
                return NotFound();
            }
            return View(res);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(QuestionType model)
        {
            var res = await _api.GetValAsync($"QuestionType/IsExist?name={model.TypeName}");
            if (res == true)
            {
                ModelState.AddModelError("k1", "The Question Type already exists");
            }

            if (ModelState.IsValid)
            {
                _api.Update<QuestionType>("QuestionType/Update", model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: QuestionType/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var res = _api.GetById<QuestionType>("QuestionType/GetByIdAsync?id=" + id);

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

            var res = _api.GetById<QuestionType>("QuestionType/GetByIdAsync?id=" + id);
            if (res != null)
            {
                _api.Delete("QuestionType/Delete", id);
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
