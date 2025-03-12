using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Prime.Api.BaseApi;
//using Microsoft.AspNetCore.Mvc.Filters;

using Entities;
using Services;

namespace BaseApi;

[Route("api/[controller]")]
[ApiController]
//[ServiceFilter(typeof(ApiFilter))]
public class ApiBaseController : ControllerBase
{
    protected readonly  IUnitOfWork _unitOfWork;
    protected readonly IAppService _appService;
    public ApiBaseController(IUnitOfWork unitOfWork, IAppService appService)
    {
        _unitOfWork = unitOfWork;
        _appService = appService; 
        
    }
}

