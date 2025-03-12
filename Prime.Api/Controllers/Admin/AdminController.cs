#region NameSpace
using BaseApi;
using CSV;
using DAL;
using DataTablesFilters;
using DataTablesHelper;
using Dto;
using Entities;
using HelperModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services;
using System.Linq.Expressions;
using System.Reflection.Emit;

namespace Prime.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "3")]
#endregion
public class AdminController : ApiBaseController
{

    #region Ctor_Declare
    private readonly ICSVService _csvService;
    public AdminController(IUnitOfWork unitOfWork, IAppService appService, ICSVService csvService) : base(unitOfWork, appService)
    {
        _csvService = csvService;
    }

    #endregion

   
   
    #region AddTeacherCodes
    [HttpPost("AddTeacherCode")]
    public async Task<IActionResult> AddTeacherCode(TeacherCodeAdminDto dto)
    {
        FeedBackReturn _feedBackReturn = new FeedBackReturn();
        var _teacherCode = _unitOfWork.TeacherCodes.Find(m => m.TCode == dto.TeacherCode);
        if (_teacherCode == null)
        {
            _feedBackReturn.Message = "Invalid Code";
            return Ok(_feedBackReturn);
        }

        if (_teacherCode.AdminId != null)
        {
            _feedBackReturn.Message = "Already assigned";
            return Ok(_feedBackReturn);
        }

        if (_teacherCode.ValidUpToDate < _unitOfWork.GetServerDate())
        {
            _feedBackReturn.Message = "Code Expired";
            return Ok(_feedBackReturn);
        }

        var _userId = int.Parse(User.Identity.Name);
        if (await UpdateAdminIftoTeacherWithStudents(new TeacherCode[] { _teacherCode }, _userId))
        {
            _feedBackReturn.Message = "success";
            _feedBackReturn.IsSuccess = true;
        }
        return Ok(_feedBackReturn);
    }

    [HttpPost("AddTeacherCodesFromCSV")]
    public async Task<IActionResult> AddTeacherCodesFromCSV([FromForm] TeacherCodeAdminDtoCsv dto)
    {
        FeedBackReturn _feedBackReturn = new FeedBackReturn();

        if (Path.GetExtension(dto.TeacherCodeFile.FileName).ToLower() != ".csv")
        {
            _feedBackReturn.Message = "invalid CSV File";
            return Ok(_feedBackReturn);
        }
        var _tCodes = _csvService.ReadCSV<TeacherCodeCsv>(dto.TeacherCodeFile.OpenReadStream());
        if (_tCodes.Count == 0)
        {
            _feedBackReturn.Message = "Empty CSV File";
            return Ok(_feedBackReturn);
        }
        string invalidcodes = "";
        foreach (var _tcode in _tCodes)
        {
            var _teacherCode = _unitOfWork.TeacherCodes.Find(m => m.TCode == _tcode.TeacherCode);
            if (_teacherCode == null || _teacherCode.AdminId != null)
            {
                invalidcodes += _tcode + "|";
            }
        }
        if (invalidcodes != "")
        {
            _feedBackReturn.Message = "Invalid Codes " + invalidcodes.Substring(0, invalidcodes.Length - 1);
            return Ok(_feedBackReturn);
        }
        var _userId = int.Parse(User.Identity.Name);
        var _tCodesList = _tCodes.Select(m => m.TeacherCode).ToList();
        var _teacherCodes = await _unitOfWork.TeacherCodes.FindAllAsync(m => _tCodesList.Contains(m.TCode));
        if (await UpdateAdminIftoTeacherWithStudents(_teacherCodes, _userId))
        {
            _feedBackReturn.Message = "success";
            _feedBackReturn.IsSuccess = true;
        }


        return Ok(_feedBackReturn);

    }

    #endregion

    [HttpPost("UnAssignAdminFromTeacherCode")]
    public async Task<IActionResult> UnAssignAdminFromTeacherCode(StringUpdateDto  dto)
    {
        FeedBackReturn _feedBackReturn = new FeedBackReturn();
        var _teacherCode = _unitOfWork.TeacherCodes.Find(m => m.TCode ==dto.StringValue);
        _teacherCode.AdminId = null;
        _unitOfWork.TeacherCodes.Update(_teacherCode);
        _unitOfWork.Save();
        _feedBackReturn.Message = "success";
        _feedBackReturn.IsSuccess = true;
        return Ok(_feedBackReturn);
    }
    
    #region Manage_Users_Block_Reset
    [HttpPut("BlockUser")]
    [Authorize(Roles = "3,4")]
    public IActionResult BlockUser(IntUpdateDto dto)
    {
        var _user = _unitOfWork.Users.GetById(dto.IntValue);
        if (_user.RoleId == 3)
        {
            var _teachers = _unitOfWork.Users.FindAll(m => m.AdminId == dto.IntValue);
            foreach (var teacher in _teachers)
            {
                teacher.IsActiveUser = !_user.IsActiveUser;
                _unitOfWork.Users.Update(teacher);
            }
        }

        _user.IsActiveUser = !_user.IsActiveUser;
        _unitOfWork.Users.Update(_user);
        _unitOfWork.Save();
        return Ok(true);
    }
    [HttpPut("ResetPassword")]
    public async Task<IActionResult> ResetPassword(StringIdUpdateDto dto)
    {
        var _user = await _unitOfWork.Users.FindAsync(m => m.UserId == dto.Id);
        _user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.StringValue);
        _unitOfWork.Users.Update(_user);
        _unitOfWork.Save();
        return Ok(true);
    }
    #endregion

    #region GetData
    [HttpGet("GetAdminBooks")]
    [Authorize(Roles = "3,4")]
    public async Task<IActionResult> GetAdminBooks()
    {
        var _userId = int.Parse(User.Identity.Name);
        return Ok(await _unitOfWork.AdminBooksV.FindAllAsync(m => m.AdminId == _userId));
    }

    [HttpGet("GetTeachers")]
    [Authorize(Roles = "3,4")]
    public async Task<IActionResult> GetTeachers()
    {
        var _userId = int.Parse(User.Identity.Name);
        return Ok(await _unitOfWork.AdminTeachersV.FindAllAsync(m => m.AdminId == _userId));
    }

    [HttpGet("GetTeachersCodes")]
    [Authorize(Roles = "3,4")]
    public async Task<IActionResult> GetTeachersCodes()
    {
        var _userId = int.Parse(User.Identity.Name);
        return Ok(await _unitOfWork.AdminTeachersBooksV.FindAllAsync(m => m.AdminId == _userId));
    }
    #endregion

    #region Private_Methods


    private async Task<bool> UpdateAdminIftoTeacherWithStudents(IEnumerable<TeacherCode> _teacherCodes, int _adminId)
    {
        foreach (var _code in _teacherCodes)
        {
            _code.AdminId = _adminId;
        }

        _unitOfWork.TeacherCodes.UpdateRange(_teacherCodes);

        if (_teacherCodes.All(m => m.UserId == null))
        {
            _unitOfWork.Save();
            return true;
        }
        var _teacherCodesIds = _teacherCodes.Select(m => m.UserId).ToList();
        var _teachers = await _unitOfWork.Users.FindAllAsync(m => _teacherCodesIds.Contains(m.UserId));
        foreach (var _code in _teachers)
        {
            _code.AdminId = _adminId;
        }

        _unitOfWork.Users.UpdateRange(_teachers);
        //get all  clasess of this teacher to update adminid of all students 

        foreach (var _code in _teachers)
        {
            _code.AdminId = _adminId;
        }
        var _teaherClasesCodes = _unitOfWork.TeacherClasss.FindAll(m => _teacherCodesIds.Contains(m.TeacherId)).Select(m => m.ClassCode);
        var _allStudentsofTeacher = _unitOfWork.StudentClasss.FindAll(m => _teaherClasesCodes.Contains(m.ClassCode)).Select(m => m.StudentId).ToList();
        var _allStudensofTeacher = _unitOfWork.Users.FindAll(m => _allStudentsofTeacher.Contains(m.UserId)).ToList();
        foreach (var u in _allStudensofTeacher)
        {
            u.AdminId = _adminId;
        }
        _unitOfWork.Users.UpdateRange(_allStudensofTeacher);
        _unitOfWork.Save();
        return true;
    }

    #endregion
}
