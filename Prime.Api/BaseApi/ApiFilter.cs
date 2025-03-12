using Entities;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Services;

namespace Prime.Api.BaseApi;

public class  ApiFilter : IActionFilter
{
    private readonly IUnitOfWork _unitOfWork;
    
    public ApiFilter(IUnitOfWork unitOfWork )
    {
        _unitOfWork = unitOfWork;
    }
    public void OnActionExecuted(ActionExecutedContext context)
    {
        
    }
    
    public void OnActionExecuting(ActionExecutingContext context)
    {
        
        var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;

        if (controllerActionDescriptor != null && context.HttpContext.User.Identity.Name !=null)
        {
            // Get the name of the controller
            var controllerName = controllerActionDescriptor.ControllerName;

            // Get the name of the action method
            var actionName = controllerActionDescriptor.ActionName;

            // Concatenate the controller and action names to get the endpoint name
            var endpointName = $"{controllerName}/{actionName}";
            LogAction(int.Parse(context.HttpContext.User.Identity.Name), endpointName);
        }
          
    }

     

    private void LogAction(int userId, string actionName)
    {

        if (userId!=null)
        {
            var _userId = userId;
            Log log = new Log()
            {
                UserId = _userId,
                Action = actionName
            };
            _unitOfWork.Logs.Add(log);
            _unitOfWork.Save();
        }
        
    }
}