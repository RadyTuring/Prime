using ApiCall;
using CsvHelper;
using Dto;
using Entities;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Prime.Client.Controllers
{
    public class CodeController : BaseController
    {
        private readonly IMemoryCache _cache;
        public CodeController(IAppService api, IMemoryCache cache) : base(api)
        {
            _cache = cache;
        }
       
        #region Search_Edit
        public async Task<IActionResult> SearchCode()
        {
            var PatchDescs = (List<string>)_api.Get<string>(" CodePatch/GetPatchs");
            PatchDescs.Insert(0, "--Choose--");
            ViewBag.Patchdescs = new SelectList(PatchDescs);

            var users = (List<string>)_api.Get<string>("CodePatch/Getusers");
            users.Insert(0, "--Choose--");
            ViewBag.users = new SelectList(users);

            var PatchTypes = (List<string>)_api.Get<string>("CodePatch/GetTypes");
            ViewBag.Patchtypes = new SelectList(PatchTypes);
            ViewBag.books = _api.GetList<Book>("Book/GetAllAsync");


            return View();
        }
        [HttpPost]
        public IActionResult SearchCode(CodeSearchDto model)
        {
            var PatchDescs = (List<string>)_api.Get<string>("CodePatch/GetPatchs");
            PatchDescs.Insert(0, "--Choose--");
            ViewBag.Patchdescs = new SelectList(PatchDescs);

            var users = (List<string>)_api.Get<string>("CodePatch/Getusers");
            users.Insert(0, "--Choose--");
            ViewBag.users = new SelectList(users);

            var PatchTypes = (List<string>)_api.Get<string>("CodePatch/GetTypes");
            ViewBag.Patchtypes = new SelectList(PatchTypes);
            ViewBag.books = _api.GetList<Book>("Book/GetAllAsync");
            string jsonDto = JsonConvert.SerializeObject(model);
            string cacheKey = Guid.NewGuid().ToString();
            _cache.Set(cacheKey, jsonDto, TimeSpan.FromMinutes(30));
            Response.Cookies.Append("cacheKey", cacheKey, new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddMinutes(30),
                HttpOnly = true,
               // Secure = true
            });
            return RedirectToAction(nameof(CodePatchSearchResult), new { id = cacheKey });
        }
        public IActionResult CodePatchSearchResult(string? id)
        {
            string _id = "";
            if (id != null)
            {
                _id = id;
            }
            else if (Request.Cookies.TryGetValue("cacheKey", out string cacheKey))
            {
                _id = cacheKey;
            }

            if (_cache.TryGetValue(_id, out string jsonDto))
            {
                string queryString = $"CodeGen/GetPrimeCodes?dto={Uri.EscapeDataString(jsonDto)}";
                var content = new StringContent(jsonDto, Encoding.UTF8, "application/json");
                var res = _api.Get2(queryString, content);
                var CodePatches = JsonConvert.DeserializeObject<IEnumerable<CodePatch>>(res.ToString());
                return View(CodePatches);
            }
            return RedirectToAction(nameof(SearchCode));
        }


        public async Task<IActionResult> DeleteCodes(int id)
        {
            _api.Delete("CodeGen/DeleteCodes", id.ToString());
            return RedirectToAction(nameof(CodePatchSearchResult));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var res = _api.GetById<CodePatch>("CodePatch/GetByIdAsync?id=" + id);
            return View(res);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CodePatch model)
        {
            var res = await _api.GetValAsync($"CodePatch/IsExist?name={model.Id}|{model.PatchDesc}");
            if (res == true)
            {
                ModelState.AddModelError("PatchDesc", "The Patch Description   already exists");
                return View(model);
            }
            var _CodePatch = _api.GetById<CodePatch>("CodePatch/GetByIdAsync?id=" + model.Id);
            _CodePatch.PatchDesc = model.PatchDesc;
            _CodePatch.IsPrinted = model.IsPrinted;
            _CodePatch.InventoryLocation = model.InventoryLocation;
            _CodePatch.Notes = model.Notes;
            _api.Update<CodePatch>("CodePatch/Update", _CodePatch);
            return RedirectToAction(nameof(CodePatchSearchResult));


        }
        #endregion

        #region GenerateCodes

        #region Books
        public async Task<IActionResult> GenBookPrints()
        {
            ViewBag.books = _api.GetList<Book>("Book/GetAllAsync");
            ViewBag.bookservices = _api.GetList<BookService>("BookService/GetAllAsync");

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> GenBookPrints(BookPrintDto model)
        {
            var _books = _api.Get<Book>("Book/GetAllAsync");
            ViewBag.books = _api.GetList<Book>("Book/GetAllAsync");
            ViewBag.bookservices = _api.GetList<BookService>("BookService/GetAllAsync");
            model.PatchType = "Book";
            var _exist = await _api.GetValAsync($"CodeGen/IsExist?name={model.PatchDesc}");
            if (_exist)
            {
                ModelState.AddModelError("PatchDesc", $"The Patch {model.PatchDesc} already exists");
                return View();

            }

            if (!ModelState.IsValid)
            {
                return View();
            }

            var res = _api.Create<BookPrintCodeCsv>("CodeGen/GenBookPrintCodes", model, true);
            var selectedBook = _books.FirstOrDefault(m => m.BookId == model.BookId);

            string fileName = "Book_" + selectedBook.BookName + "_" + model.PatchDesc;
            return GetCSV<BookPrintCodeCsv>(res, fileName);
        } 
        #endregion

        #region Teachers
        public async Task<IActionResult> GenTeacherCodes()
        {
            ViewBag.books = _api.GetList<Book>("Book/GetAllAsync");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> GenTeacherCodes(TeacherCodeDto model)
        {
            TempData.Remove("Patchexist");
            var _books = _api.Get<Book>("Book/GetAllAsync");
            ViewBag.books = _api.GetList<Book>("Book/GetAllAsync");
            model.PatchType = "Teacher";
            var _exist = await _api.GetValAsync($"CodeGen/IsExist?name={model.PatchDesc}");
            string uniqueKey = null;
            if (_exist)
            {
                ModelState.AddModelError("PatchDesc", $"The Patch {model.PatchDesc} already exists");
                return View();

            }

            if (!ModelState.IsValid)
            {
                return View();
            }
            var res = _api.Create<TeacherCodeCsv>("CodeGen/GenTeacherCodes", model, true);
            var selectedBook = _books.FirstOrDefault(m => m.BookId == model.BookId);

            string fileName = "Teacher_" + selectedBook.BookName + "_" + model.PatchDesc;
            return GetCSV<TeacherCodeCsv>(res, fileName);
        } 
        #endregion

        #region Admin
        public async Task<IActionResult> GenAdminCodes()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> GenAdminCodes(AdminUserDto model)
        {
            model.PatchType = "Admin";
            var _exist = await _api.GetValAsync($"CodeGen/IsExist?name={model.PatchDesc}");
            if (_exist)
            {
                ModelState.AddModelError("PatchDesc", $"The Patch {model.PatchDesc} already exists");
                return View();
            }

            if (!ModelState.IsValid)
            {
                return View();
            }


            var res = _api.Create<AdminCodesCsv>("CodeGen/GenAdminCodes", model, true);
          
            string fileName = "Admin_"  + "_" + model.PatchDesc;
            return GetCSV<AdminCodesCsv>(res, fileName);
        } 
        #endregion

        #endregion

        #region CSV
        public async Task<IActionResult> GetCSV(int id)
        {
            var _CodePatch = _api.GetById<CodePatch>("CodePatch/GetByIdAsync?id=" + id);
            string fileName = "";
            switch (_CodePatch.PatchType.ToLower())
            {
                case "book":
                    fileName = "Book_" + _CodePatch.Book.BookName + "_" + _CodePatch.PatchDesc;
                    var bookPrintCodeCsv = _api.Get<BookPrintCodeCsv>("CodeGen/GetBookPrintCodes?id=" + id);
                    return GetCSV<BookPrintCodeCsv>(bookPrintCodeCsv, fileName);
                case "teacher":
                    fileName = "Teacher_" + _CodePatch.Book.BookName + "_" + _CodePatch.PatchDesc;
                    var teacherCodeCsves = _api.Get<TeacherCodeCsv>("CodeGen/GetTeacherCodes?id=" + id);
                    return GetCSV<TeacherCodeCsv>(teacherCodeCsves, fileName);
                case "admin":
                    fileName = "Admin_" +  "_" + _CodePatch.PatchDesc;
                    var _adminCodesCsv = _api.Get<AdminCodesCsv>("CodeGen/GetAdminCodes?id=" + id);
                    return GetCSV<AdminCodesCsv>(_adminCodesCsv, fileName);
            }
            return View();
        }
        private IActionResult GetCSV<T>(IEnumerable<T> res, string fileName)
        {
            StringBuilder csvContent = new StringBuilder();

            if (res.Any())
            {
                csvContent.AppendLine(string.Join(",", typeof(T).GetProperties().Select(p => p.Name)));
            }
            foreach (var item in res)
            {
                List<string> rowValues = new List<string>();
                foreach (var property in typeof(T).GetProperties())
                {
                    object value = property.GetValue(item);
                    string formattedValue = value?.ToString() ?? ""; // Handle null values
                    rowValues.Add(formattedValue.Replace(",", "\",\"")); // Escape commas within values
                }
                csvContent.AppendLine(string.Join(",", rowValues));
            }
            byte[] csvData = Encoding.UTF8.GetBytes(csvContent.ToString());
            return File(csvData, "text/csv", fileName + ".csv");
        }
        #endregion
    }

}


