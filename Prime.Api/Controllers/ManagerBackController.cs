using BaseApi;
using CSV;
using CsvHelper.Configuration;
using DataTablesFilters;
using DataTablesHelper;
using Dto;
using Entities;
using HelperModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using  JqueryTb;
namespace Prime.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "2,3,4")]
    public class ManagerBackController : ApiBaseController
    {

        public ManagerBackController(IUnitOfWork unitOfWork, IAppService appService) : base(unitOfWork, appService)
        {

        }

        [HttpGet("GetTeachersCodes")]
        [Authorize(Roles = "3,4")]
        public async Task<IActionResult> GetTeachersCodesByAdminId(int id)
        {
            return Ok(await _unitOfWork.AdminTeachersBooksV.FindAllAsync(m => m.AdminId == id));
        }
        [HttpPut("BlockUser")]
        [Authorize(Roles = "3,4")]
        public IActionResult BlockUser(BlockUserDto dto)
        {
            var _user = _unitOfWork.Users.Find(m => m.UserName == dto.UserName);
            switch (_user.RoleId)
            {
                case 2:
                    switch (dto.Blocktype)
                    {

                        case "2":

                            var _teachers = _unitOfWork.Users.FindAll(m => m.AdminId == _user.UserId);
                            foreach (var teacher in _teachers)
                            {
                                teacher.IsActiveUser = !_user.IsActiveUser;
                                _unitOfWork.Users.Update(teacher);
                            }


                            break;
                    }
                    break;
                case 3:
                    switch (dto.Blocktype)
                    {
                        case "4":
                            var _teachers = _unitOfWork.Users.FindAll(m => m.AdminId == _user.UserId && m.RoleId == 2);
                            foreach (var teacher in _teachers)
                            {
                                teacher.IsActiveUser = !_user.IsActiveUser;
                                _unitOfWork.Users.Update(teacher);
                            }
                            break;
                        case "5":

                            var _allUsers = _unitOfWork.Users.FindAll(m => m.AdminId == _user.UserId);
                            foreach (var teacher in _allUsers)
                            {
                                teacher.IsActiveUser = !_user.IsActiveUser;
                                _unitOfWork.Users.Update(teacher);
                            }

                            break;
                    }
                    break;
            }



            if (_user.IsActiveUser)
            {
                _user.BlockLevel = int.Parse(dto.Blocktype);
            }
            else
            {
                _user.BlockLevel = 0;
            }
            _user.IsActiveUser = !_user.IsActiveUser;
            _unitOfWork.Users.Update(_user);
            _unitOfWork.Save();
            return Ok(true);
        }

        [HttpPut("ChangeOffLineLogTimes")]
        public async Task<IActionResult> ChangeOffLineLogTimes(IntIdUpdateDto dto)
        {
            var _user = await _unitOfWork.Users.FindAsync(m => m.UserId == dto.Id);
            _user.LogOfflineTimes = dto.IntValue;
            _unitOfWork.Users.Update(_user);
            _unitOfWork.Save();
            return Ok(true);
        }
        #region Users
        [HttpPost("FilterUsersAsync")]
        public async Task<IActionResult> FilterUsersAsync([FromBody] DataTableApi<UserFilter> model)
        {
            var _userId = int.Parse(User.Identity.Name);
            var _user = _unitOfWork.Users.Find(m => m.UserId == _userId);
            if (_user.RoleId == 3)
            {
                model.FilterC.AdminId = _userId;
                model.FilterC.AdminIdFilter = FilterOptionN.Equal;
            }
            var pageSize = model.DataTableInfo.PageSize;
            var skip = model.DataTableInfo.skip;
            var searchValue = model.DataTableInfo.SearchValue;
            var sortColumn = model.DataTableInfo.SortColumn;
            var sortColumnDirection = model.DataTableInfo.SortColumnDirection;

            Expression<Func<UsersV, bool>> criteriaSearchValue = m =>
           (
               (m.UserId.HasValue ? m.UserId.ToString() : "") +
               (m.UserName ?? "") +
               (m.FullName ?? "") +
               (m.RoleName ?? "") +
               (m.Status ?? "") +
               (m.AdminUserName ?? "") +
               (m.AdminFullName ?? "") +
               (m.CountryName ?? "")
           ).Contains(searchValue);

            var criteria =Dt.BuildCriteria<UsersV, UserFilter>(model.FilterC, criteriaSearchValue);

            var _users =await _unitOfWork.UsersV.FindWithFiltersAsync(
               criteria: criteria,
               sortColumn: sortColumn,
               sortColumnDirection: sortColumnDirection,
               skip: skip,
               take: pageSize);
            var recordsTotal = await _unitOfWork.UsersV.CountAsync(criteria: criteria) ;

            var jsonData = new { recordsFiltered = recordsTotal, recordsTotal, data = _users };

            return Ok(jsonData);
        }


        #endregion

        #region Logs
        [HttpPost("FilterLogs")]
        public IActionResult FilterLogs([FromBody] DataTableApi<LogFilter> model)
        {
            var _userId = int.Parse(User.Identity.Name);
            var _user = _unitOfWork.Users.Find(m => m.UserId == _userId);
            if (_user.RoleId == 3)
            {
                model.FilterC.AdminId = _userId;
                model.FilterC.AdminIdFilter = FilterOptionN.Equal;
            }
            var pageSize = model.DataTableInfo.PageSize;
            var skip = model.DataTableInfo.skip;
            var searchValue = model.DataTableInfo.SearchValue;
            var sortColumn = model.DataTableInfo.SortColumn;
            var sortColumnDirection = model.DataTableInfo.SortColumnDirection;

            Expression<Func<LogV, bool>> criteriaSearchValue = m =>
          (
              (m.UserId.HasValue ? m.UserId.ToString() : "") +
              (m.UserName ?? "") +
              (m.FullName ?? "") +
              (m.AdminId.HasValue ? m.AdminId.ToString() : "") +
              (m.TranDate.HasValue ? m.TranDate.ToString() : "") +
              (m.LogAction ?? "") 
              
          ).Contains(searchValue);


            var criteria =Dt.BuildCriteria<LogV, LogFilter>(model.FilterC, criteriaSearchValue);


            var _data = _unitOfWork.LogV.FindWithFilters(
               criteria: criteria,
               sortColumn: sortColumn,
               sortColumnDirection: sortColumnDirection,
               skip: skip,
               take: pageSize);
            var recordsTotal = _unitOfWork.LogV.FindWithFilters(criteria: criteria).Count();

            var jsonData = new { recordsFiltered = recordsTotal, recordsTotal, data = _data };

            return Ok(jsonData);
        }
        #endregion

        #region AdminTeachers
        [HttpPost("FilterAdminTeachers")]
        public IActionResult FilterAdminTeachers([FromBody] DataTableApi<AdminTeachersFilter> model)
        {
            var pageSize = model.DataTableInfo.PageSize;
            var skip = model.DataTableInfo.skip;
            var searchValue = model.DataTableInfo.SearchValue;
            var sortColumn = model.DataTableInfo.SortColumn;
            var sortColumnDirection = model.DataTableInfo.SortColumnDirection;
          
            Expression<Func<AdminTeachersV, bool>> criteriaSearchValue = m =>
              (
              (m.AdminId.HasValue ? m.AdminId.ToString() : "") +
              (m.AdminCode ?? "") +
              (m.AdminUserName ?? "") +
              (m.AdminFullName ?? "") +
              (m.TeacherId.HasValue ? m.TeacherId.ToString() : "") +
              (m.TeacherUserName ?? "") +
              (m.TeacherFullName ?? "") +
              (m.CountryName ?? "")+
              (m.Status ?? "")
             ).Contains(searchValue);

            var criteria =Dt.BuildCriteria<AdminTeachersV, AdminTeachersFilter>(model.FilterC, criteriaSearchValue);


            var _data = _unitOfWork.AdminTeachersV.FindWithFilters(
               criteria: criteria,
               sortColumn: sortColumn,
               sortColumnDirection: sortColumnDirection,
               skip: skip,
               take: pageSize);
            var recordsTotal = _unitOfWork.AdminTeachersV.FindWithFilters(criteria: criteria).Count();

            var jsonData = new { recordsFiltered = recordsTotal, recordsTotal, data = _data };

            return Ok(jsonData);
        }
        #endregion

        #region Mix
        [HttpGet("GetTeachers")]
        [Authorize(Roles = "2,3,4")]
        public async Task<IActionResult> GetTeachers(int userId)
        {
            var res = await _unitOfWork.TeachersClassessBooksV.FindAllAsync(m => m.StudentId == userId);
            return Ok(res);
        }
        [HttpGet("GetStudents")]
        [Authorize(Roles = "2,3,4")]
        public async Task<IActionResult> GetStudents(int userId)
        {
            var res = await _unitOfWork.StudentsClassessBooksV.FindAllAsync(m => m.TeacherId == userId);
            return Ok(res);
        }

        #endregion
        
    }
}
