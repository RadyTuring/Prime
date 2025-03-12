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
    [Authorize(Roles = "4,5", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme + "," + JwtBearerDefaults.AuthenticationScheme)]
    public class BookController : BaseController
    {
        #region Crud
        public BookController(IAppService api) : base(api)
        {
        }
        public async Task<IActionResult> Index()
        {
            var res = _api.Get<Book>("Book/GetAll");
            return View(res);
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
            var bookimage = _api.GetImage($"Book/GetBookImage?bookId={res.BookId}");
            if (bookimage.ToLower() == "notfound")
                ViewBag.bookimage = null;
            else
                ViewBag.bookimage = $"data:image/jpeg;base64,{bookimage}";
            var gamesimage = _api.GetImage($"Book/GetGamesImage?bookId={res.BookId}");
            if (gamesimage.ToLower() == "notfound")
                ViewBag.gamesimage = null;
            else
                ViewBag.gamesimage = $"data:image/jpeg;base64,{gamesimage}";
            return View(res);
        }

        // GET: Book/Create
        public IActionResult Create()
        {
            ViewBag.updatelevels = _api.GetList<UpdateLevel>("Book/GetUpdateLevels");

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookDto model)
        {
            ViewBag.updatelevels = _api.GetList<UpdateLevel>("Book/GetUpdateLevels");
            var res = await _api.GetValAsync($"Book/IsExist?name={model.BookName}");
            if (res == true)
            {
                ModelState.AddModelError("BookName", "The Book Name already exists");
            }

            if (ModelState.IsValid)
            {
                _api.Create("Book/Add", model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.updatelevels = _api.GetList<UpdateLevel>("Book/GetUpdateLevels");
            var res = _api.GetById<Book>("Book/GetByIdAsync?id=" + id);

            if (res == null)
            {
                return NotFound();
            }
            return View(res);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Book model)
        {
            ViewBag.updatelevels = _api.GetList<UpdateLevel>("Book/GetUpdateLevels");
            var res = await _api.GetValAsync($"Book/IsExist?name_id={model.BookName}|{model.BookId}");
            if (res == true)
            {
                ModelState.AddModelError("BookName", "The Book Name already exists");
            }
            if (ModelState.IsValid)
            {
                _api.Update<Book>("Book/Update", model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int? id)
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


        #endregion

        #region UploadFiles
        public IActionResult UploadFiles(int id)
        {
            var res = _api.GetById<Book>("Book/GetByIdAsync?id=" + id);
            BookFilesVM model = new BookFilesVM()
            {
                BookId = id,
                BookName = res.BookName

            };
            return View(model);
        }
        // Controller action
        [HttpPost]
        public async Task<IActionResult> UploadFiles([FromForm] BookFilesVM model, [FromServices] IHubContext<UploadProgressHub> hubContext)
        {
            bool flag = false;
            string connectionId = HttpContext.Request.Query["connectionId"];
            List<string> _fileNames = new List<string>();

            // Flag to track if there's a duplicate name
            bool isDuplicateName = false;

            // Check PPKFile for duplication
            if (model.PPKFile != null)
            {
                _fileNames.Add(model.PPKFile.FileName.ToLower());
            }

            // Check DefaultImage for duplication
            if (model.DefaultImage != null)
            {
                if (!_fileNames.Contains(model.DefaultImage.FileName.ToLower()))
                {
                    _fileNames.Add(model.DefaultImage.FileName.ToLower());
                }
                else
                {
                    isDuplicateName = true;
                }
            }

            // Check GamesFileName for duplication
            if (model.GamesFileName != null)
            {
                if (!_fileNames.Contains(model.GamesFileName.FileName.ToLower()))
                {
                    _fileNames.Add(model.GamesFileName.FileName.ToLower());
                }
                else
                {
                    isDuplicateName = true;
                }
            }

            // Check GamesImage for duplication
            if (model.GamesImage != null)
            {
                if (!_fileNames.Contains(model.GamesImage.FileName.ToLower()))
                {
                    _fileNames.Add(model.GamesImage.FileName.ToLower());
                }
                else
                {
                    isDuplicateName = true;
                }
            }


            if (isDuplicateName)
            {
                ModelState.AddModelError("", "Duplicate file names detected. Each file must have a unique name.");
                return View(model);
            }

            if (ModelState.IsValid)
            {
                var _currentBook = _api.GetById<Book>("Book/GetByIdAsync?id=" + model.BookId);

                if (model.PPKFile != null)
                {
                    _currentBook.PPKFileName = model.PPKFile.FileName;
                    await _api.FtpUpload(model.PPKFile, connectionId, hubContext);
                    flag = true;
                }
                if (model.DefaultImage != null)
                {
                    _currentBook.DefaultImage = model.DefaultImage.FileName;
                    await _api.FtpUpload(model.DefaultImage, connectionId, hubContext);
                    flag = true;
                }

                if (model.GamesFileName != null)
                {
                    _currentBook.GamesFileName = model.GamesFileName.FileName;
                    await _api.FtpUpload(model.GamesFileName, connectionId, hubContext);
                    flag = true;
                }
                if (model.GamesImage != null)
                {
                    _currentBook.GamesImage = model.GamesImage.FileName;
                    await _api.FtpUpload(model.GamesImage, connectionId, hubContext);
                    flag = true;
                }
                if (flag)
                {
                    _api.Update<Book>("Book/Update", _currentBook);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }


        #endregion
    }
}
