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
#endregion
namespace Prime.Client.Controllers
{
    [Authorize(Roles = "3,4,5", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme + "," + JwtBearerDefaults.AuthenticationScheme)]
    public class ManagerController : BaseController
    {
        public ManagerController(IAppService api) : base(api)
        {
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetTeachers(int id)
        {
            var res = _api.Get<TeachersClassessBooksV>("ManagerBack/GetTeachers?userId=" + id);
            return res != null ? View(res) : Problem("Teachers are null.");
        }
        [HttpGet]
        public async Task<IActionResult> GetStudents(int id)
        {
            var res = _api.Get<StudentsClassessBooksV>("ManagerBack/GetStudents?userId=" + id);
            return res != null ? View(res) : Problem("Students are null.");
        }
        #region UsersManager
        public IActionResult FilterUsers()
        {
            ViewBag.Role = Request.Cookies["roleid"];
            prepareDatableForUsers();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> FilterUsers(UserFilter model)
        {
            prepareDatableForUsers();

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
                var result = await _api.PostDt<DataTableApi<UserFilter>, DataTableData<UsersV>>("ManagerBack/FilterUsers", dtApi);
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
        public IActionResult FilterLogs([FromRoute] int? id = null)
        {
            prepareDatableForLogs();
            LogFilter logFilter = new LogFilter();
            if (id != null)
            {
                logFilter.UserIdFilter = FilterOptionN.Equal;
                logFilter.UserId = id.Value;
            }
            return View(logFilter);
        }

        [HttpPost]
        public async Task<IActionResult> FilterLogs(LogFilter model)
        {
            prepareDatableForLogs();


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
                var result = await _api.PostDt<DataTableApi<LogFilter>, DataTableData<LogV>>("ManagerBack/FilterLogs", dtApi);
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
        public IActionResult FilterAdminTeachers()
        {
            prepareDatableForAdminTeachers();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> FilterAdminTeachers(AdminTeachersFilter model)
        {
            prepareDatableForAdminTeachers();


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
                var result = await _api.PostDt<DataTableApi<AdminTeachersFilter>, DataTableData<AdminTeachersV>>("ManagerBack/FilterAdminTeachers", dtApi);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return BadRequest();

        }
        #endregion

        #region ManageUser
        public IActionResult BlockUser(int id)
        {
            var res = _api.GetById<User>($"user/GetUserDataById?userId={id}");
            if (res != null)
            {
                string _type = "";
                switch (res.RoleId)
                {
                    case 1:
                        _type = "Student";
                        break;
                    case 2:
                        _type = "Teacher";
                        ViewBag.blocktypes = GetBlockTypeTeacherSelectList();
                        break;
                    case 3:
                        _type = "Admin";
                        ViewBag.blocktypes = GetBlockTypeAdminSelectList();
                        break;
                    case 4:
                        _type = "Manager";
                        break;
                }

                BlockUserDto blockUserDto = new BlockUserDto()
                {
                    FullName = res.FullName,
                    UserName = res.UserName,
                    Status = res.IsActiveUser ? "Active" : "Blocked",
                    UserType = _type,
                    BlockLevel = res.BlockLevel
                };
                return View(blockUserDto);

            }
            return View();
        }
        [HttpPost]
        public IActionResult BlockUser(BlockUserDto model)
        {
            var res = _api.Update<BlockUserDto>("ManagerBack/BlockUser", model);
            TempData["success"] = "Password changed successfully";
            return View();
        }

        public IActionResult ChangeOffLineLogTimes(int id)
        {
            var res = _api.GetById<User>($"user/GetUserDataById?userId={id}");

            UserOfflineLoginTimesVM userOfflineLoginTimesVM = new UserOfflineLoginTimesVM()
            {
                Id = res.UserId,
                FullName = res.FullName,
                UserName = res.UserName,
                UserOfflineLoginValue = res.LogOfflineTimes
            };
            return View(userOfflineLoginTimesVM);
        }
        [HttpPost]
        public IActionResult ChangeOffLineLogTimes(UserOfflineLoginTimesVM model)
        {
            IntIdUpdateDto intIdUpdateDto = new IntIdUpdateDto()
            {
                Id = model.Id,
                IntValue = model.UserOfflineLoginValue
            };

            var res = _api.Update<IntIdUpdateDto>("ManagerBack/ChangeOffLineLogTimes", intIdUpdateDto);


            return RedirectToAction(nameof(FilterUsers));
        }

       

        public async Task<IActionResult> ManageUser()
        {
            ViewBag.roles = new SelectList(_api.Get<Role>("Mes/GetRoles"), "RoleId", "RoleName");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ManageUser(UserUpdateRoleDto dto)
        {
            ViewBag.roles = new SelectList(_api.Get<Role>("Mes/GetRoles"), "RoleId", "RoleName");
            if (dto.RoleId == 0)
            {
                ModelState.AddModelError("RoleId", "Choose The role");
                return View();
            }
            var isAvailable = await _api.GetValAsync($"User/CheckValidity?userName={dto.UserName}");
            if (isAvailable)
            {
                ModelState.AddModelError("UserName", "Invalid User Name");
                return View();
            }
            _api.Update("user/ChangeRole", dto);
            return RedirectToAction("ManageUser");
        }

        public IActionResult GetLogV(int id)
        {
            var res = _api.Get<LogV>("User/GetLogV?userId=" + id);
            return res != null ? View(res) : Problem("Log is null.");
        }

        #region Private Methods

        private List<SelectListItem> GetBlockTypeTeacherSelectList()
        {
            // Manually define the items corresponding to the enum values
            var blockTypeTeacherItems = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Teacher Only" },
                new SelectListItem { Value = "2", Text = "Teacher and Students" } // Fixed spelling
            };

            return blockTypeTeacherItems;
        }
        private List<SelectListItem> GetBlockTypeAdminSelectList()
        {
            var blockTypeAdminItems = new List<SelectListItem>
            {
                new SelectListItem { Value = "3", Text = "Admin Only" },
                new SelectListItem { Value = "4", Text = "Admin And Teachers" } ,
                 new SelectListItem { Value = "5", Text = "Admin And Teachers and Students" } // Fixed spelling
            };

            return blockTypeAdminItems;
        }
        #endregion
        #endregion

        #region DataTable
        #region Users
        private void prepareDatableForUsers()
        {
            string links = getlinks();

            //  ViewBag.columns = DtGenerator.GenCols<UserDatatableCols>(links);
            ViewBag.actions = GenRefMethods();
        }
        private string getlinks()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"<a title='Edit Profile' href=\"/Account/Edit/${{row.userId}}\"><i class='fas fa-edit'></i></a>|");
            sb.AppendLine($"<a title='Logs' href=\"/Manager/FilterLogs/${{row.userId}}\"><i class='fas fa-history'></i></a>|");
            sb.AppendLine($"<a title='Reset Password' href=\"/Account/ResetPassword/${{row.userId}}\"><i class='fas fa-unlock-alt'></i></a>|");
            sb.AppendLine($"<a title='Offline Login Times' href=\"/Manager/ChangeOffLineLogTimes/${{row.userId}}\"><i class='fas fa-clock'></i></a>|");
            sb.AppendLine($"<a title='Send Notification' href=\"/Note/SendNote/${{row.userId}}\"><i class='fas fa-bell'></i></a>|");
            sb.AppendLine($"<a title='Block/UnBlock' href=\"/Manager/BlockUser/${{row.userId}}\"><i class='${{row.status == 'Active' ? 'fas fa-ban' : 'fas fa-check'}}'></i></a>|");
            sb.AppendLine("${row.roleName === 'teacher' ? `");

            sb.AppendLine($"    <a  title='Students' href=\"/Manager/GetStudents/${{row.userId}}\"><i class='fas fa-user-graduate'></i></a>");

            sb.AppendLine("` : row.roleName === 'student' ? `");

            sb.AppendLine($"    <a  title='Teachers' href=\"/Manager/GetTeachers/${{row.userId}}\"><i class='fas fa-chalkboard-teacher'></i></a>");

            sb.AppendLine("` : row.roleName === 'admin' ? `");

            sb.AppendLine($"    <a  title='Teachers' href=\"/Account/SomeTeacherAction/${{row.userId}}\"><i class='fas fa-chalkboard-teacher'></i></a>");

            sb.AppendLine("` : `");
            sb.AppendLine($"    <a title='Books' href=\"/Code/GenBookPrints\"><i class='fas fa-book'></i></a>");

            sb.AppendLine("`}");
            return sb.ToString();
        }
        private string GenRefMethods()
        {
            var stringBuilder = new StringBuilder();
            //execute the code by enter
            stringBuilder.AppendLine("    $(document).keydown(function (event) {");
            stringBuilder.AppendLine("        if (event.key === 'Enter') {");
            stringBuilder.AppendLine("            $('#btnSearch').click();");
            stringBuilder.AppendLine("        }");
            stringBuilder.AppendLine("    });");

            // Event listener for btnSearch click
            stringBuilder.AppendLine("    $('#btnSearch').click(function () {");
            stringBuilder.AppendLine("        table.ajax.reload();");
            stringBuilder.AppendLine("    });");

            // End JavaScript initialization
            stringBuilder.AppendLine("});");

            return stringBuilder.ToString();
        }

        #endregion

        #region Logs
        private void prepareDatableForLogs()
        {

            //  ViewBag.columns = DtGenerator.GenCols<LogV>();
            ViewBag.actions = GenRefMethods();
        }

        #endregion

        #region AdminTeachers
        private void prepareDatableForAdminTeachers()
        {
            //  ViewBag.filters = DtGenerator.GenFilters<AdminTeachersFilter>();
            //   ViewBag.columns = DtGenerator.GenCols<AdminTeachersV>();
            ViewBag.actions = GenRefMethods();
        }

        #endregion



        #endregion

       
    }
}
