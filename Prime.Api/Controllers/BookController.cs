using BaseApi;
using CsvHelper.Configuration.Attributes;
using Dto;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;
using System.Data;
using System.Net;

namespace Prime.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "1,2,3,4,5")]
    public class BookController : ApiBaseController
    {
        private string _ekitPath = "files\\ppk\\";
        
        public BookController(IUnitOfWork unitOfWork, IAppService appService) : base(unitOfWork, appService) { }
        [HttpGet("GetEkit")]
        public async Task<IActionResult> GetEkit(int bookId)
        {

            var m = _unitOfWork.Books.GetById(bookId);
            if (m == null)
            {
                return BadRequest("Invalid BookId");
            }
            string _fileName = m.PPKFileName;
            var result = await _appService.DownloadFile(_ekitPath + _fileName);
            return File(result.Item1, result.Item2, result.Item3);
        }
        [HttpGet("GetAllLinks")]
        public async Task<IActionResult> GetAllLinks(int bookId)
        {
            // Find the book by ID
            /*var book = _unitOfWork.Books.GetById(bookId);
            if (book == null)
            {
                return BadRequest("Invalid BookId");
            }*/

            var bookLinks = await _unitOfWork.BookLinks.FindAllAsync(m => m.BookId == bookId);
            return Ok(bookLinks);
        }
        [HttpGet("GetAllLinksByOs")]
        public async Task<IActionResult> GetAllLinksByOs(int bookId, int osId, int serviceId)
        {
            // Find the book by ID
            /*var book = _unitOfWork.Books.GetById(bookId);
            if (book == null)
            {
                return BadRequest("Invalid BookId");
            }*/

            var bookLinks = await _unitOfWork.BookLinks.FindAllAsync(m => m.BookId == bookId && m.BookServiceId == serviceId && m.PlatformOsId == osId);
            return Ok(bookLinks);
        }

        [HttpGet("GetGame")]
        public async Task<IActionResult> GetGame(int bookId)
        {

            var m = _unitOfWork.Books.GetById(bookId);
            if (m == null)
            {
                return BadRequest("Invalid book ID");
            }
            string _fileName = m.GamesFileName;
            var result = await _appService.DownloadFile(_ekitPath + _fileName);
            return File(result.Item1, result.Item2, result.Item2);
        }

        [HttpGet("GetBookImage")]
        public async Task<IActionResult> GetBookImage(int bookId)
        {

            var m = _unitOfWork.Books.GetById(bookId);
            if (m == null)
            {
                return BadRequest("Invalid book ID");
            }
            string _fileName = m.DefaultImage;
            var result = await _appService.DownloadFile(_ekitPath + _fileName);
            return File(result.Item1, result.Item2, result.Item2);
        }
        [HttpGet("GetGamesImage")]
        public async Task<IActionResult> GetGamesImage(int bookId)
        {

            var m = _unitOfWork.Books.GetById(bookId);
            if (m == null)
            {
                return BadRequest("Invalid book ID");
            }
            string _fileName = m.GamesImage;
            var result = await _appService.DownloadFile(_ekitPath + _fileName);
            return File(result.Item1, result.Item2, result.Item2);
        }
        [HttpGet("GetGamesManifest")]
        public async Task<IActionResult> GetGamesManifest(int bookId)
        {

            var m = _unitOfWork.Books.GetById(bookId);
            if (m == null)
            {
                return BadRequest("Invalid book ID");
            }
            string _fileName = m.GamesImage;
            var result = await _appService.DownloadFile(_ekitPath + _fileName);
            return File(result.Item1, result.Item2, result.Item2);
        }
        [HttpGet("GetBooksIds")]
        public IActionResult GetBooksIds()
        {
            var _userId = int.Parse(User.Identity.Name);
            var userBooks = _unitOfWork.UserBooks.FindAll(m => m.UserId == _userId).Select(m => new { BookId = m.BookId });
            return Ok(userBooks);
        }
        [HttpGet("GetBooks")]
        public IActionResult GetBooks()
        {

            var _userId = int.Parse(User.Identity.Name);
            List<int> userBooks = _unitOfWork.UserBooks.FindAll(m => m.UserId == _userId).Select(m => m.BookId).ToList();
            var books = _unitOfWork.Books.FindAll(m => userBooks.Contains(m.BookId));
            return Ok(books);
        }
        [HttpGet("GetBookCodeData")]
        public async Task<IActionResult> GetBookCodeData(string code)
        {
            if (code == null || code.Length != 12)
            {
                return BadRequest("Invalid Code");
            }
            var res = await _unitOfWork.BookPrints.FindAsync(m => m.BkCode == code);
            if (res == null)
            {
                return NotFound("Invalid Code");
            }
            return Ok(res);
        }
        [HttpGet("GetBookServicses")]
        public async Task<IActionResult> GetBookServicses()
        {
            var res = await _unitOfWork.BookServices.GetAllAsync();
            return Ok(res);
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var res = await _unitOfWork.Books.FindAllAsync(m=>m.BookId!=0, includes:new string[] { "UpdateLevel" });
            
            return Ok(res);
        }

        [HttpGet("GetUpdateLevels")]
      
        public async Task<IActionResult> GetUpdateLevels()
        {
            var res = await _unitOfWork.UpdateLevels.GetAllAsync();
            return Ok(res);
        }

        [HttpGet("GetAllAsync")]
      
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _unitOfWork.Books.GetAllAsync());
        }
        [HttpGet("GetById")]
      
        public IActionResult GetById(int id)
        {
            return Ok(_unitOfWork.Books.GetById(id));
        }

        [HttpGet("GetByIdAsync")]
        [Authorize(Roles = "4")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            if (id == 0)
                return BadRequest();
            else
                return Ok(await _unitOfWork.Books.GetByIdAsync(id));
        }


        [HttpPost("Add")]
        public IActionResult Add(BookDto dto)
        {
            Book book = new Book()
            {
                BookName = dto.BookName,
                DefaultImage = dto.DefaultImage,
                PPKFileName = dto.PPKFileName,
                GamesFileName = dto.GamesFileName,
                GamesImage = dto.GamesImage,
                Prefix = dto.Prefix,
                PPkVersion = dto.PPkVersion,
                UpdateLevelId = dto.UpdateLevelId,
                DownloadLink = dto.DownloadLink,
                LinkUrl = dto.LinkUrl,
                Notes = dto.Notes
            };
            var m = _unitOfWork.Books.Add(book);
            _unitOfWork.Save();
            return Ok(m);
        }
        [HttpPut("Update")]
       
        public IActionResult Update(Book book)
        {
            var m = _unitOfWork.Books.Update(book);
            _unitOfWork.Save();
            return Ok(m);
        }

        [HttpDelete("Delete")]
      
        public IActionResult Delete(int id)
        {
            var o = _unitOfWork.Books.GetById(id);

            _unitOfWork.Books.Delete(o);
            _unitOfWork.Save();
            return Ok();
        }

        [HttpGet("SearchByName")]
       
        public IActionResult SearchByName(string name)
        {
            return Ok(_unitOfWork.Books.FindAll(m => m.BookName.Contains(name)));
        }
        [HttpGet("IsExist")]
      
        public IActionResult IsExist(string name_id)
        {
            string[] _arr = name_id.Split('|');
            return Ok(_unitOfWork.Books.IsExist(m => m.BookName == _arr[0] && m.BookId !=Convert.ToInt32( _arr[1])));
        }


    }
}
