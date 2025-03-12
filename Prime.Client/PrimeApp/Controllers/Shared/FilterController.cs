#region Using
using ApiCall;
using Custom_Filter;
using DataTablesFilters;
using DataTablesHelper;
using Dto;
using Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using System;
using System.Net.Http.Headers;
using System.Text;
using Datatable;
using ViewModels;
using JqDtDisplay;
#endregion
namespace Prime.Client.Controllers
{
    [Authorize(Roles = "3,4,5", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme + "," + JwtBearerDefaults.AuthenticationScheme)]
    public class FilterController : BaseController
    {
        public FilterController(IAppService api) : base(api)
        {
        }
        
        #region Manager
        #region Users
        public async Task<IActionResult> Users()
        {
            string[] cols = { "UserId", "User Name", "Full Name", "Role", "Status", "Country", "Admin User Name", "Admin Full Name", "Action" };

            string tableHtml = DtGenerator.GenerateHtmlTb(cols);
            var script = DtGenerator.GenDtScript<UserDatatableCols, UserFilter>("/Filter/Users", getlinks(), true);

            ViewBag.htmltb = tableHtml;
            ViewBag.script = script;

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Users(UserFilter model)
        {
            var pageSize = int.Parse(Request.Form["length"]);
            var skip = int.Parse(Request.Form["start"]);
            var searchValue = Request.Form["search[value]"];
            var sortColumn = Request.Form[string.Concat("columns[", Request.Form["order[0][column]"], "][name]")];
            var sortColumnDirection = Request.Form["order[0][dir]"];

            DataTableInfo dtInfo = new DataTableInfo()
            {
                PageSize = pageSize,
                skip = skip,
                SearchValue = searchValue,
                SortColumn = sortColumn,
                SortColumnDirection = sortColumnDirection

            };
            DataTableApi<UserFilter> dtApi = new DataTableApi<UserFilter>()
            {
                DataTableInfo = dtInfo,
                FilterC = model
            };


            try
            {
                var result = await _api.PostDt<DataTableApi<UserFilter>, DataTableData<UsersV>>("Filter/UsersAsync", dtApi);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return BadRequest();

        }
        #endregion

        #region Logs
        public async Task<IActionResult> Logs([FromRoute] int? id = null)
        {

            LogFilter logFilter = new LogFilter();
            if (id != null)
            {
                logFilter.UserIdFilter = FilterOptionN.Equal;
                logFilter.UserId = id.Value;
            }
            return View(logFilter);
        }

        [HttpPost]
        public async Task<IActionResult> Logs(LogFilter model)
        {



            var pageSize = int.Parse(Request.Form["length"]);
            var skip = int.Parse(Request.Form["start"]);
            var searchValue = Request.Form["search[value]"];
            var sortColumn = Request.Form[string.Concat("columns[", Request.Form["order[0][column]"], "][name]")];
            var sortColumnDirection = Request.Form["order[0][dir]"];

            DataTableInfo dtInfo = new DataTableInfo()
            {
                PageSize = pageSize,
                skip = skip,
                SearchValue = searchValue,
                SortColumn = sortColumn,
                SortColumnDirection = sortColumnDirection

            };
            DataTableApi<LogFilter> dtApi = new DataTableApi<LogFilter>()
            {
                DataTableInfo = dtInfo,
                FilterC = model
            };


            try
            {
                var result = await _api.PostDt<DataTableApi<LogFilter>, DataTableData<LogV>>("Filter/LogsAsync", dtApi);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return BadRequest();

        }
        #endregion

        #region AdminTeachers
        public async Task<IActionResult> AdminTeachers()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AdminTeachers(AdminTeachersFilter model)
        {
            var pageSize = int.Parse(Request.Form["length"]);
            var skip = int.Parse(Request.Form["start"]);
            var searchValue = Request.Form["search[value]"];
            var sortColumn = Request.Form[string.Concat("columns[", Request.Form["order[0][column]"], "][name]")];
            var sortColumnDirection = Request.Form["order[0][dir]"];

            DataTableInfo dtInfo = new DataTableInfo()
            {
                PageSize = pageSize,
                skip = skip,
                SearchValue = searchValue,
                SortColumn = sortColumn,
                SortColumnDirection = sortColumnDirection

            };
            DataTableApi<AdminTeachersFilter> dtApi = new DataTableApi<AdminTeachersFilter>()
            {
                DataTableInfo = dtInfo,
                FilterC = model
            };


            try
            {
                var result = await _api.PostDt<DataTableApi<AdminTeachersFilter>, DataTableData<AdminTeachersV>>("Filter/AdminTeachersAsync", dtApi);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return BadRequest();

        }

        #endregion
        #endregion

        #region Admin

        #region TeachersCodes
        public async Task<IActionResult> TeacherCodes()
        {
            string[] cols = { "Teacher Code", "Teacher", "Book", "Assign Date", "Valid Up To Date", "Action" };
            string[] alertMessages = 
                {
                    "Are you sure?",                // 1: Confirmation dialog title
                    "You won't be able to undo this action!", // 2: Confirmation dialog text
                    "Yes, delete it!",              // 3: Confirm button text
                    "Cancel",                       // 4: Cancel button text
                    "The record has been successfully deleted.", // 5: Success message
                    "An error occurred while deleting the record." // 6: Error message
                };


            string tableHtml = DtGenerator.GenerateHtmlTb(cols);
            var script = DtGenerator.GenDtScript<TeacherCodesDt, object>("/Filter/TeacherCodesDt", getlinksTeacherCodes());
            var alert = DtGenerator.GetSweetAlert("/Admin/UnAssignAdminFromTeacherCode", "tCode", alertMessages);

            ViewBag.htmltb = tableHtml;
            ViewBag.script = script;
            ViewBag.alert = alert;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> TeacherCodesDt()
        {

            var pageSize = int.Parse(Request.Form["length"]);
            var skip = int.Parse(Request.Form["start"]);
            var searchValue = Request.Form["search[value]"];
            var sortColumn = Request.Form[string.Concat("columns[", Request.Form["order[0][column]"], "][name]")];
            var sortColumnDirection = Request.Form["order[0][dir]"];

            DataTableInfo dtInfo = new DataTableInfo()
            {
                PageSize = pageSize,
                skip = skip,
                SearchValue = searchValue,
                SortColumn = sortColumn,
                SortColumnDirection = sortColumnDirection

            };
            try
            {
                var result = await _api.PostDt<DataTableInfo, DataTableData<AdminTeachersBooksV>>("Filter/TeachersCodesAsync", dtInfo);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            return BadRequest();

        }

        #endregion

        #region Users
        public IActionResult AdminUsers()
        {
            string[] cols = { "UserId", "User Name", "Full Name", "Role", "Status", "Action" };
           
            string tableHtml = DtGenerator.GenerateHtmlTb(cols);
            var script = DtGenerator.GenDtScript<UserDatatableColsAdmin, UserFilterForAdmin>("/Filter/AdminUsers", getlinks(), true);
            ViewBag.htmltb = tableHtml;
            ViewBag.script = script;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AdminUsers(UserFilterForAdmin model)
        {
            var pageSize = int.Parse(Request.Form["length"]);
            var skip = int.Parse(Request.Form["start"]);
            var searchValue = Request.Form["search[value]"];
            var sortColumn = Request.Form[string.Concat("columns[", Request.Form["order[0][column]"], "][name]")];
            var sortColumnDirection = Request.Form["order[0][dir]"];

            DataTableInfo dtInfo = new DataTableInfo()
            {
                PageSize = pageSize,
                skip = skip,
                SearchValue = searchValue,
                SortColumn = sortColumn,
                SortColumnDirection = sortColumnDirection

            };
            DataTableApi<UserFilterForAdmin> dtApi = new DataTableApi<UserFilterForAdmin>()
            {
                DataTableInfo = dtInfo,
                FilterC = model
            };


            try
            {
                var result = await _api.PostDt<DataTableApi<UserFilterForAdmin>, DataTableData<UsersV>>("Filter/UsersAsync", dtApi);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return BadRequest();

        }

        #endregion



        #region AdminLogs
        public async Task<IActionResult> AdminLogs([FromRoute] int? id = null)
        {
            string[] cols = { "User Id", "User Name", "Full Name", "Action", "Date"  };
           
            string tableHtml = DtGenerator.GenerateHtmlTb(cols);
            var script = DtGenerator.GenDtScript<AdminLogColsDt, LogFilterAdmin>("/Filter/AdminLogs", null, true);
             
            ViewBag.htmltb = tableHtml;
            ViewBag.script = script;
            LogFilterAdmin logFilter = new LogFilterAdmin();
            if (id != null)
            {
                logFilter.UserIdFilter = FilterOptionN.Equal;
                logFilter.UserId = id.Value;
            }
            return View(logFilter);
        }

        [HttpPost]
        public async Task<IActionResult> AdminLogs(LogFilterAdmin model)
        {
            var pageSize = int.Parse(Request.Form["length"]);
            var skip = int.Parse(Request.Form["start"]);
            var searchValue = Request.Form["search[value]"];
            var sortColumn = Request.Form[string.Concat("columns[", Request.Form["order[0][column]"], "][name]")];
            var sortColumnDirection = Request.Form["order[0][dir]"];

            DataTableInfo dtInfo = new DataTableInfo()
            {
                PageSize = pageSize,
                skip = skip,
                SearchValue = searchValue,
                SortColumn = sortColumn,
                SortColumnDirection = sortColumnDirection

            };
            DataTableApi<LogFilterAdmin> dtApi = new DataTableApi<LogFilterAdmin>()
            {
                DataTableInfo = dtInfo,
                FilterC = model
            };


            try
            {
                var result = await _api.PostDt<DataTableApi<LogFilterAdmin>, DataTableData<LogV>>("Filter/LogsAsync", dtApi);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return BadRequest();

        }
        #endregion
        #endregion


        #region DataTable

        private string getlinksTeacherCodes()
        {
            return @"
        <a href='javascript:;' class='btn-danger js-myaction' data-id='${row.teacherCode}'>
            <i class='fas fa-trash fa-sm'></i>
        </a>";
        }
        private string getlinks()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"<a title='Edit Profile' target='_blank' href=\"/Account/Edit/${{row.userId}}\"><i class='fas fa-edit'></i></a>|");
            sb.AppendLine($"<a title='Logs' target='_blank' href=\"/Filter/AdminLogs/${{row.userId}}\"><i class='fas fa-history'></i></a>|");
            sb.AppendLine($"<a title='Reset Password' target='_blank' href=\"/Account/ResetPassword/${{row.userId}}\"><i class='fas fa-unlock-alt'></i></a>|");
            //sb.AppendLine($"<a title='Offline Login Times' target='_blank' href=\"/ManageUsers/ChangeOffLineLogTimes/${{row.userId}}\"><i class='fas fa-clock'></i></a>|");
            sb.AppendLine($"<a title='Send Notification' target='_blank' href=\"/Note/SendNote/${{row.userId}}\"><i class='fas fa-bell'></i></a>|");
            sb.AppendLine($"<a title='Block/UnBlock' target='_blank' href=\"/ManageUsers/BlockUser/${{row.userId}}\"><i class='${{row.status == 'Active' ? 'fas fa-ban' : 'fas fa-check'}}'></i></a>|");
            sb.AppendLine("${row.roleName === 'teacher' ? `");

            sb.AppendLine($"    <a  title='Students' target='_blank' href=\"/Manager/GetStudents/${{row.userId}}\"><i class='fas fa-user-graduate'></i></a>");

            sb.AppendLine("` : row.roleName === 'student' ? `");

            sb.AppendLine($"    <a  title='Teachers' target='_blank' href=\"/Manager/GetTeachers/${{row.userId}}\"><i class='fas fa-chalkboard-teacher'></i></a>");

            sb.AppendLine("` : row.roleName === 'admin' ? `");

            sb.AppendLine($"    <a  title='Teachers' target='_blank' href=\"/Account/SomeTeacherAction/${{row.userId}}\"><i class='fas fa-chalkboard-teacher'></i></a>");

            sb.AppendLine("` : `");
            sb.AppendLine($"    <a title='Books' target='_blank' href=\"/Code/GenBookPrints\"><i class='fas fa-book'></i></a>");

            sb.AppendLine("`}");
            return sb.ToString();
        }


        #endregion
    }
}
