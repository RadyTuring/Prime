using BaseApi;
using CSV;
using Dto;
using Entities;
using HelperModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;
using System.Data;

namespace Prime.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "1,2,3,4")]
    public class MesController : ApiBaseController
    {

        public MesController(IUnitOfWork unitOfWork, IAppService appService) : base(unitOfWork, appService)
        {
        }
        [HttpGet("GetServerDate")]
        public IActionResult GetDate()
        {
            return Ok(_unitOfWork.GetServerDate());
        }
        [HttpGet("GetFile")]
        public async Task< IActionResult> GetFile(string fileName)
        {
            var result = await _appService.DownloadFile(fileName);
            return File(result.Item1, result.Item2, result.Item2);
        }

        [HttpGet("GetPages")]
        public IActionResult GetPages()
        {
            return Ok(_unitOfWork.SysPages.GetAll());
        }
        [HttpGet("GetRoles")]
        public IActionResult GetRoles()
        {
            return Ok(_unitOfWork.Roles.FindAll(m=>m.RoleId>2));
        }
        [HttpGet("GetSepartors")]
        public IActionResult GetSepartors()
        {
            return Ok(_unitOfWork.AppSettings.GetAll());
        }
        [HttpPut("UpdateSepartors")]
        public IActionResult UpdateSepartors(AppSettingDto dto)
        {
            var o = _unitOfWork.AppSettings.Find(m => m.Id == 1);
            o.Separator1 = dto.Separator1;
            o.Separator2 = dto.Separator2;
            o.TimeFactor = dto.TimeFactor;
            _unitOfWork.AppSettings.Update(o);
            _unitOfWork.Save();
            return Ok(o);
        }
       

    }
}
