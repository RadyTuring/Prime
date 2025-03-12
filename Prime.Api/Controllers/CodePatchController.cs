using BaseApi;
using Dto;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;
using System.Data;

namespace Prime.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class CodePatchController : ApiBaseController
    {
        public CodePatchController(IUnitOfWork unitOfWork, IAppService appService) : base(unitOfWork, appService)
        {
        }

        [HttpGet("GetAll")]
        [Authorize(Roles = "1,2,3,4")]
        public IActionResult GetAll()
        {
            return Ok(_unitOfWork.CodePatchs.GetAll());
        }
        [HttpGet("GetPatchs")]
        [Authorize(Roles = "1,2,3,4")]
        public IActionResult GetPatchs()
        {
            return Ok(_unitOfWork.CodePatchs.GetDistinct(m=>m.PatchDesc));
        }

        [HttpGet("Getusers")]
        [Authorize(Roles = "1,2,3,4")]
        public IActionResult Getusers()
        {
            return Ok(_unitOfWork.CodePatchs.GetDistinct(m => m.UserName));
        }
        [HttpGet("GetTypes")]
        [Authorize(Roles = "1,2,3,4")]
        public IActionResult GetTypes()
        {
            return Ok(_unitOfWork.CodePatchs.GetDistinct(m => m.PatchType));
        }
        [HttpGet("GetAllAsync")]
        [Authorize(Roles = "1,2,3,4")]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _unitOfWork.CodePatchs.GetAllAsync());
        }
        [HttpGet("GetById")]
        [Authorize(Roles = "1,2,3,4")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _unitOfWork.CodePatchs.FindAsync(m=>m.Id==id,new string[] {"Book"}));
        }

        [HttpGet("GetByIdAsync")]
        [Authorize(Roles = "1,2,3,4")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            
                return Ok(await _unitOfWork.CodePatchs.FindAsync(m => m.Id == id, new string[] { "Book" }));
        }
        [HttpPost("Add")]
        [Authorize(Roles = "4")]
        public IActionResult Add(CodePatchDto dto)
        {
            var _userId = int.Parse(User.Identity.Name);
            var _currentUser = _unitOfWork.Users.Find(m => m.UserId == _userId);
            CodePatch CodePatch = new CodePatch()
            {
                PatchDesc = dto.PatchDesc,
                PatchType = dto.PatchType,
                UserName = _currentUser.UserName
            };
            var m = _unitOfWork.CodePatchs.Add(CodePatch);
            _unitOfWork.Save();
            return Ok(m);
        }

        [HttpPut("Update")]
        [Authorize(Roles = "4")]
        public IActionResult Update(CodePatch CodePatch)
        {
            var m = _unitOfWork.CodePatchs.Update(CodePatch);
            _unitOfWork.Save();
            return Ok(m);
        }

        [HttpDelete("Delete")]
        [Authorize(Roles = "4")]
        public IActionResult Delete(int id)
        {
            var o = _unitOfWork.CodePatchs.GetById(id);
           
            _unitOfWork.CodePatchs.Delete(o);
            _unitOfWork.Save();
            return Ok();
        }

        [HttpGet("SearchByName")]
        [Authorize(Roles = "1,2,3,4")]
        public IActionResult SearchByName(string name)
        {
            return Ok(_unitOfWork.CodePatchs.FindAll(m => m.PatchDesc.Contains(name)));
        }
        [HttpGet("IsExist")]
        [Authorize(Roles = "1,2,3,4")]
        public IActionResult IsExist(string name)
        {
            string[] id_name=name.Split('|');
            string _name = id_name[1];
            int _id = int.Parse(id_name[0]);
            return Ok(_unitOfWork.CodePatchs.IsExist(m => m.PatchDesc == _name && m.Id !=_id));
        }
    }
}
