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
    [Authorize(Roles = "1,2,3,4")]
    public class MenuController : ApiBaseController
    {
        public MenuController(IUnitOfWork unitOfWork, IAppService appService) : base(unitOfWork, appService)
        {
        }

        [HttpGet("GetPages")]
        public async Task<IActionResult> GetPages(int roleId)
        {
            return Ok( await _unitOfWork.PagesV.FindAllAsync(m => m.RoleId == roleId));
        }
       

        [HttpGet("GetById")]
        [Authorize(Roles = "manager,student,teacher,admin")]
        public IActionResult GetBookById(int id)
        {

            GetInfo();
            return Ok(_unitOfWork.SysPages.GetById(id));
        }

        [HttpGet("GetByIdAsync")]
        [Authorize(Roles = "manager,student,teacher,admin")]
        public async Task<IActionResult> GetBookAsync(int id)
        {
            GetInfo();
            if (id == 0)
                return BadRequest();
            else
                return Ok(await _unitOfWork.SysPages.GetByIdAsync(id));
        }

        [HttpPost("Add")]
        [Authorize(Roles = "manager")]
        public IActionResult Add([FromForm] BookDto dto)
        {
            GetInfo();
            //if (dto.DefaultImage != null && Path.GetExtension(dto.DefaultImage.FileName).ToLower() != ".jpg")
            //{
            //    return BadRequest("Invalid Image File");
            //}
          
            Book book = new Book()
            {
                BookName = dto.BookName,
              
               
               
                PPkVersion = dto.PPkVersion,
            };
            //var m = _unitOfWork.SysPages.Add(book);
            _unitOfWork.Save();
            return Ok();
        }


        [HttpDelete("Delete")]
        [Authorize(Roles = "manager")]
        public IActionResult Delete(int id)
        {
            GetInfo();
            var o = _unitOfWork.SysPages.Find(m => m.Id == id);
            if (o == null)
            {
                return BadRequest("The Book is not Exist");
            }
            _unitOfWork.SysPages.Delete(o);
            _unitOfWork.Save();
            //_appService.Delete(@"Files\" + o.PgImage);
            return Ok();
        }
        [HttpGet("SearchByName")]
        [Authorize(Roles = "manager,student,teacher,admin")]
        public IEnumerable<SysPage> GetSysPages(string name)
        {
            GetInfo();
            return (_unitOfWork.SysPages.FindAll(m => m.PgTitle.Contains(name)));
        }
        private void GetInfo()
        {
            //_userCode = HttpContext.User.Identity.Name;
          //  _user = _unitOfWork.Users.Find(m => m.UserCode == _userCode);
        //    _schoolCode = _user.SchoolCode;
            //_sep = _unitOfWork.AppSettings.GetById(1).Separator1;
        }

    }
}
