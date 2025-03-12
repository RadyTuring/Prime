using BaseApi;
using CSV;
using Dto;
using Entities;
using HelperModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Renci.SshNet.Messages;
using Services;
using System.Data;
using Token;

namespace Prime.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "1,2,3,4")]
    public class NoteBackController : ApiBaseController
    {
        public NoteBackController(IUnitOfWork unitOfWork, IAppService appService) : base(unitOfWork, appService)
        {
        }
        [HttpGet("GetUnRead")]
        public async Task<IActionResult> GetUnRead()
        {
            var _userId = int.Parse(User.Identity.Name);
            return Ok(await _appService.GetUnreadNotes(_userId));
        }
        [HttpGet("GetRead")]
        public async Task<IActionResult> GetRead()
        {
            var _userId = int.Parse(User.Identity.Name);
            return Ok(await _appService.GetReadNotes(_userId));
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var _userId = int.Parse(User.Identity.Name);
            return Ok(await _appService.GetAllNotes(_userId));

        }

        [HttpPut("ChangeToRead")]
        public IActionResult Update(int noteId)
        {
            _appService.UpdateNoteToRead(noteId);
            return Ok();
        }
        [HttpPost("SendToUser")]
        public async Task<IActionResult> SendToUser(NoteDto dto)
        {
            var _userId = int.Parse(User.Identity.Name);
            _appService.SendNote(_userId, dto);
            return Ok();
        }
        [HttpPost("SendToClass")]
        public async Task<IActionResult> SendToClass(string classCode, string message, string category = null)
        {
            var _userId = int.Parse(User.Identity.Name);
            NoteDto note = new NoteDto();
            foreach (var item in GetClass_Students(classCode))
            {
                note.Category = category;
                note.Message = message;
                _appService.SendNote(_userId, note);
            }

            return Ok();
        }
        private IEnumerable<int> GetClass_Students(string classCode)
        {
            var _StudentsIds = _unitOfWork.ClassStudentsV.FindAll(m => m.ClassCode == classCode);
            return _StudentsIds.Select(m => m.UserId);
        }
    }
}
