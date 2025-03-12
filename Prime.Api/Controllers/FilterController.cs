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
    public class FilterController : ApiBaseController
    {

        public FilterController(IUnitOfWork unitOfWork, IAppService appService) : base(unitOfWork, appService)
        {

        }

        
        [HttpPost("UsersAsync")]
        public async Task<IActionResult> UsersAsync([FromBody] DataTableApi<UserFilter> model)
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
                 (_user.RoleId == 4
                     ? (m.AdminUserName ?? "") + (m.AdminFullName ?? "") + (m.CountryName ?? "")
                     : "")
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

        
        [HttpPost("TeachersCodesAsync")]
        public async Task<IActionResult> TeachersCodesAsync([FromBody] DataTableInfo model)
        {
            var _userId = int.Parse(User.Identity.Name);

            var pageSize = model.PageSize;
            var skip = model.skip;
            var searchValue = model.SearchValue;
            var sortColumn = model.SortColumn;
            var sortColumnDirection = model.SortColumnDirection;

            Expression<Func<AdminTeachersBooksV, bool>> criteria = m =>
                (
                    (m.TeacherCode ?? "") +
                    (m.TeacherFullName ?? "") +
                    (m.ValidUpToDate.HasValue ? m.ValidUpToDate.ToString() : "") +
                    (m.AssignDate.HasValue ? m.AssignDate.ToString() : "") +
                    (m.BookName ?? "")
                ).Contains(searchValue) && m.AdminId == _userId;

            var _adminTeachersBooksV = await _unitOfWork.AdminTeachersBooksV.FindWithFiltersAsync(
               criteria: criteria,
               sortColumn: sortColumn,
               sortColumnDirection: sortColumnDirection,
               skip: skip,
               take: pageSize);
            var data = _adminTeachersBooksV.Select(m => new
            {
                m.TeacherCode,
                m.TeacherFullName,
                m.AssignDate,
                m.ValidUpToDate,
                m.BookName,

            }).ToList();
            var recordsTotal = await _unitOfWork.AdminTeachersBooksV.CountAsync(criteria: criteria);

            var jsonData = new { recordsFiltered = recordsTotal, recordsTotal, data = data };

            return Ok(jsonData);
        }
        

        [HttpPost("LogsAsync")]
        public async Task<IActionResult> LogsAsync([FromBody] DataTableApi<LogFilter> model)
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
              (m.LogAction ?? "")+
              (_user.RoleId == 4? m.AdminId.HasValue? m.AdminId.ToString() : "":"")
          ).Contains(searchValue);

            var criteria =Dt.BuildCriteria<LogV, LogFilter>(model.FilterC, criteriaSearchValue);

            var _data = await _unitOfWork.LogV.FindWithFiltersAsync(
               criteria: criteria,
               sortColumn: sortColumn,
               sortColumnDirection: sortColumnDirection,
               skip: skip,
               take: pageSize);
            var recordsTotal = await _unitOfWork.LogV.CountAsync(criteria: criteria) ;

            var jsonData = new { recordsFiltered = recordsTotal, recordsTotal, data = _data };

            return Ok(jsonData);
        }
      

        
        [HttpPost("AdminTeachersAsync")]
        public async Task<IActionResult> AdminTeachersAsync([FromBody] DataTableApi<AdminTeachersFilter> model)
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

            var _data =await _unitOfWork.AdminTeachersV.FindWithFiltersAsync(
               criteria: criteria,
               sortColumn: sortColumn,
               sortColumnDirection: sortColumnDirection,
               skip: skip,
               take: pageSize);
            var recordsTotal = await _unitOfWork.AdminTeachersV.CountAsync(criteria: criteria);

            var jsonData = new { recordsFiltered = recordsTotal, recordsTotal, data = _data };

            return Ok(jsonData);
        }
    }
}
