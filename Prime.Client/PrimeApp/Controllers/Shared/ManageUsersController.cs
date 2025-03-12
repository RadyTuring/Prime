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
namespace Prime.Client.Controllers;

[Authorize(Roles = "3,4,5", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme + "," + JwtBearerDefaults.AuthenticationScheme)]
public class ManageUsersController : BaseController
{
    #region Ctor
    private int _userRole = 0;
    public ManageUsersController(IAppService api) : base(api)
    {

    }
    #endregion

    #region ManageUser
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || id == 0)
        {
            id = int.Parse(Request.Cookies["userid"]);
            TempData["_backurl"] = "p";
        }
        else
        {
            TempData["_backurl"] = "u";
        }
        var res = _api.GetById<User>($"user/GetUserDataById?userId={id}");
        ViewBag.countries = new SelectList(_api.Get<Country>("Country/GetAll"), "CountryId", "CountryNameUtc");

        var imageData = _api.GetImage($"user/GetProfileImageByUser?userId={id}");
        if (imageData.ToLower() == "notfound" || imageData.ToLower() == "internalservererror")
            ViewBag.userimage = null;
        else
            ViewBag.userimage = $"data:image/jpeg;base64,{imageData}";
        TempData["username"] = res.UserName;
        UserUpdateDto dto = new UserUpdateDto()
        {
            UserName = res.UserName,
            FullName = res.FullName,
            CountryId = res.CountryId,
        };
        TempData.Keep("_backurl");
        TempData.Keep("_userid");
        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit([FromForm] UserUpdateDto model)
    {

        if (ModelState.IsValid)
        {
            _api.CreateWithFile("User/UpdateById", model, true);
            return RedirectToAction(nameof(Edit));
            ViewBag.countries = new SelectList(_api.Get<Country>("Country/GetAll"), "CountryId", "CountryNameUtc");

        }
        return View(model);
    }


    [HttpPost]
    public IActionResult DeleteProfileImage(string? userName)
    {
        try
        {
            // Call the API to delete the profile image
            var res = _api.Delete($"user/DeleteImage?userName={userName}");

            // If the deletion is successful, update ViewBag or perform necessary actions
            if (res)
            {
                ViewBag.profileimage = null;
                return Json(new { success = true, message = "Image deleted successfully." });
            }

            // Return failure response
            return Json(new { success = false, message = "Failed to delete the image." });
        }
        catch (Exception ex)
        {
            // Handle any exceptions and return an error response
            return Json(new { success = false, message = "An error occurred while deleting the image." });
        }
    }


    public IActionResult ResetPassword(int id)
    {
        if (id == 0)
        {
            id = int.Parse(Request.Cookies["userid"]);
            TempData["_backurl"] = "p"; //profile page to back
        }
        else
        {
            TempData["_backurl"] = "u"; //profile page to users
        }
        TempData["_userid"] = id;
        TempData.Keep("_backurl");
        TempData.Keep("_userid");
        return View();
    }

    [HttpPost]
    public IActionResult ResetPassword(ResetPasswordVM model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        StringIdUpdateDto _userPassword = new StringIdUpdateDto()
        {
            Id = (int)TempData["_userid"],
            StringValue = model.NewPassword
        };
        string _url = TempData["_backurl"].ToString();
        var res = _api.Update<StringIdUpdateDto>("Admin/ResetPassword", _userPassword);
        TempData["success"] = "Password changed successfully";
        return View();
    }
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
        TempData["success"] = "User Status changed successfully";
        return RedirectToAction(controllerName: GetReturn().Controller, actionName: GetReturn().Action);
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


        return RedirectToAction(controllerName: GetReturn().Controller, actionName: GetReturn().Action);
    }

    public async Task<IActionResult> ManageUser()
    {
        ViewBag.roles = new SelectList(_api.Get<Role>("Mes/GetRoles"), "RoleId", "RoleName");
        return RedirectToAction(controllerName: GetReturn().Controller, actionName: GetReturn().Action);
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
        return RedirectToAction(controllerName: GetReturn().Controller, actionName: GetReturn().Action);
    }

    #endregion


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

    private (string Controller,string Action) GetReturn()
    {
        if (  Request.Cookies != null && Request.Cookies["roleid"] != null)
            _userRole = int.Parse(Request.Cookies["roleid"]);
        string cnt="", act="";
        switch (_userRole)
        {
            case 3:
                cnt = "Filter";
                act = "AdminUsers";
                break;
            case 4:
            case 5:
                cnt = "Filter";
                act = "Users";
                break;
        }
        return (cnt, act);
    }
    #endregion
}
