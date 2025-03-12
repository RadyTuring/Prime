using ApiCall;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Prime.Client.Controllers;
using System.Threading.Tasks;
using ViewModels;
namespace Custom_Filter;
public class CustomFilter : IAsyncActionFilter
{
    private readonly IAppService _api;
    public CustomFilter(IAppService api)
    {
        _api = api;
    }
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {

        if (!IsExcludedAction(context))
        {
            var httpContext = context.HttpContext;

            var useridFromCookie = httpContext.Request.Cookies["userid"];
            var roleidFromCookie = httpContext.Request.Cookies["roleid"];
            if (!string.IsNullOrEmpty(useridFromCookie))
            {
                if (!string.IsNullOrEmpty(useridFromCookie))
                {
                    var _user = _api.GetById<User>("user/GetUserData?userId=" + useridFromCookie);
                    var imageData = _api.GetImage("user/GetProfileImage");
                    IEnumerable<PagesV> _sideMenu = _api.Get<PagesV>("Menu/GetPages?roleId=" + roleidFromCookie);
                    IEnumerable<NoteController> notes = _api.Get<NoteController>("Notification/GetUnRead");

                    if (context.Controller is Controller controller)
                    {

                        if (imageData.ToLower() == "notfound")
                            controller.ViewData["profileimage"] = null;
                        else
                            controller.ViewData["profileimage"] = $"data:image/jpeg;base64,{imageData}";

                        controller.ViewData["usernotes"] = notes;
                        controller.ViewData["user"] = _user;
                        controller.ViewData["_sideMenu"] = _sideMenu;
                    }


                }
            }
        }
        await next();
    }

    private bool IsExcludedAction(ActionExecutingContext context)
    {
        var actionDescriptor = context.ActionDescriptor as Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor;
        var excludeFromUserDataFilterAttribute = actionDescriptor.MethodInfo.GetCustomAttributes(inherit: true)
            .Any(a => a.GetType() == typeof(ExcludeAction));

        return excludeFromUserDataFilterAttribute;
    }
}
