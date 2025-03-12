#region using
using ApiCall;
using Dto;
using Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System.Security.Claims;
using ViewModels;
using Newtonsoft.Json;
using Renci.SshNet;
using Prime.Client.Controllers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Custom_Filter;

#endregion
public class AccountController : BaseController
{
    public AccountController(IAppService api) : base(api)
    {
    }
    public async Task<IActionResult> CheckValidity(string username)
    {
        var isAvailable = await _api.GetValAsync($"User/CheckValidity?userName={username}");

        return Json(new { isAvailable = isAvailable });
    }
    public async Task<IActionResult> Create()
    {
        ViewBag.countries = new SelectList(_api.Get<Country>("Country/GetAll"), "CountryId", "CountryNameUtc");
        ViewBag.timezones = new SelectList(_api.Get<TbTimeZone>("Country/GetTimeZones"), "Id", "Location");
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromForm] UserVM model)
    {
        ViewBag.countries = new SelectList(_api.Get<Country>("Country/GetAll"), "CountryId", "CountryNameUtc");
        ViewBag.timezones = new SelectList(_api.Get<TbTimeZone>("Country/GetTimeZones"), "Id", "Location");
        if (!ModelState.IsValid)
        {
            return View();
        }
        var isAvailable = await _api.GetValAsync($"User/CheckValidity?userName={model.UserName}");
        if (!isAvailable)
        {
            ModelState.AddModelError("UserName", "Invalid User Name");
            return View();
        }
        UserDto userDto = new UserDto()
        {
            UserName = model.UserName,
            FullName = model.FullName,
            CountryId = model.CountryId,
            Password = model.Password,
            ImageFile = model.ImageFile
        };
        _api.CreateWithFile("User/Register", userDto, false);
        return RedirectToAction(nameof(Login));
    }


    [AllowAnonymous]
    [ExcludeAction]
    public IActionResult Login()
    {

        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ExcludeAction]
    public async Task<IActionResult> Login(LoginVm model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var res = _api.Login<AuthModel>("User/login", model);

        var _authModel = res.Result as AuthModel;
        if (!_authModel.IsAuthenticated)
        {
            ModelState.AddModelError("UserName", "Invalid User");
            return View(model);
        }

        var token = _authModel.Token;

        string controllerUser = "", action = "Index";
        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, model.UserName),
        new Claim(ClaimTypes.Role, _authModel.RoleId.ToString())
    };

        switch (_authModel.RoleId)
        {
            case 1:
                controllerUser = "Student";
                break;
            case 2:
                controllerUser = "Teacher";
                break;
            case 3:
                controllerUser = "Admin";
                break;
            case 4:
                controllerUser = "Manager";
                break;
            case 5:
                controllerUser = "SuperManager";
                break;
        }



        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties
        {
            IsPersistent = model.RememberMe
        };
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

        var options = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(30),
            // IsEssential = true,
            // Secure = true
        };

        Response.Cookies.Append("userid", _authModel.UserId.ToString(), options);
        Response.Cookies.Append("roleid", _authModel.RoleId.ToString(), options);

        return RedirectToAction(action, controllerUser);
    }

    public async Task<IActionResult> LogOut()
    {
        TempData["userr"] = null;

        Response.Cookies.Delete("prmtoken");
        Response.Cookies.Delete("mnu");

        await HttpContext.SignOutAsync(
         CookieAuthenticationDefaults.AuthenticationScheme);

        return RedirectToAction("Login");
    }

    

}