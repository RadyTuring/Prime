
using Dto;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Token;
using ViewModels;

namespace Prime.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private string _userCode;
        private string _schoolCode;
        private User _user;

        private readonly IAppService _appService;
        public AccountController(IUnitOfWork unitOfWork, IAppService appService, ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _appService = appService;
            _tokenService = tokenService;
        }

        [HttpGet("GetUerByCodeAsync")]
        //public async Task<IActionResult> GetUerByCodeAsync(string userCode)
        //{
        //    //var _currentUser = await _unitOfWork.Users.FindAsync(m => m.UserCode == userCode);

        //    //if (_currentUser is null)
        //    //{
        //    //    return BadRequest("Invalid User");
        //    //}
        //    //return Ok(_currentUser);
        //}

        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp(UserDto model)
        {
           // var user = await _unitOfWork.Users.FindAsync(m => m.UserCode == model.UserCode || m.UserName == model.UserName || m.Email == model.Email);

            //if (user != null)
            //{
            //    return BadRequest("User Already Exist");
            //}
            User newUser = new User()
            {
                //UserCode = model.UserCode,
                //UserType = model.UserType,
              //  role = model.Role,
                UserName = model.UserName,
                IsActiveUser = true,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
            };
            await _unitOfWork.Users.AddAsync(newUser);
            _unitOfWork.Save();
            return Ok(newUser);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto model)
        {
            var authModel = new AuthModel();
            if (model.UserName is null)
            {
                authModel.Message = "Invalid User ";
                return BadRequest(authModel);
            }
            //var _currentUser = await _unitOfWork.Users.FindAsync(m => m.UserCode == model.UserName || m.UserName == model.UserName || m.SchoolUserCode == model.UserName || m.GeneratedUserCode == model.UserName);

            //if (_currentUser is null)
            //{
            //    authModel.Message = "Invalid User ";
            //    return BadRequest(authModel);
            //}
            //if (!_currentUser.IsActiveUser)
            //{
            //    authModel.Message = "Blocked User ";
            //    return BadRequest(authModel);
            //}
            //if (_currentUser.role.ToLower() != "manager")
            //{
            //    if (!isAciveSchool(_currentUser.SchoolCode))
            //    {
            //        authModel.Message = "Blocked User School ";
            //        return BadRequest(authModel);
            //    }
            //}
           // bool verified = BCrypt.Net.BCrypt.Verify(model.Password, _currentUser.PasswordHash);
            //if (!verified)
            //{
            //    authModel.Message = "Invalid User ";
            //    return BadRequest(authModel);
            //}
            ////if ((bool)_currentUser.IsNewUser)
            //{
            //    authModel.Message = "New User ";
            //    authModel.UserCode = _currentUser.UserCode;
            //    authModel.SchoolUserCode = _currentUser.SchoolUserCode;
            //    authModel.IsNewUser = _currentUser.IsNewUser;
            //    authModel.FullName = _currentUser.FullName;
            //    authModel.FirstName = _currentUser.FirstName;
            //    authModel.LastName = _currentUser.LastName;
            //    authModel.ClassCode = _currentUser.ClassCode;
            //    authModel.UserType = _currentUser.UserType;
            //    authModel.SchoolCode = _currentUser.SchoolCode;
            //    authModel.GenderId = _currentUser.GenderId;
            //    authModel.ImageFile = _currentUser.ImageFile;
            //    return Ok(authModel);
            //}

            authModel.IsAuthenticated = true;
            authModel.Message = "Success";
            //authModel.UserCode = _currentUser.UserCode;
            //authModel.SchoolUserCode = _currentUser.SchoolUserCode;
            //authModel.IsNewUser = _currentUser.IsNewUser;
            //authModel.FullName = _currentUser.FullName;
            //authModel.FirstName = _currentUser.FirstName;
            //authModel.LastName = _currentUser.LastName;
            //authModel.ClassCode = _currentUser.ClassCode;
            //authModel.UserType = _currentUser.UserType;
            //authModel.SchoolCode = _currentUser.SchoolCode;
            //authModel.GenderId = _currentUser.GenderId;
            //authModel.ImageFile = _currentUser.ImageFile;

          //  authModel.Token = _tokenService.GenerateAccessToken(_currentUser);
            var refreshToken = _tokenService.GenerateRefreshToken();
            await _tokenService.SetRefreshToken(refreshToken, Response);

            //_currentUser.IsOnline = true;
            //_currentUser.RefreshToken = refreshToken.Token;
            //_currentUser.DateCreated = refreshToken.Created;
            //_currentUser.TokenExpires = refreshToken.Expires;

            _unitOfWork.Save();
            return Ok(authModel);
        }

        [HttpPut("Edit")]
        public IActionResult Edit([FromForm] UserDto dto)
        {
          //  var _currentUser = _unitOfWork.Users.Find(m => m.UserCode == dto.UserCode);
            if (dto.ImageFile != null && Path.GetExtension(dto.ImageFile.FileName).ToLower() != ".jpg")
            {
                return BadRequest("Invalid Image File");
            }
            string _defaultImage = _appService.Uploade(dto.ImageFile, "files\\images\\users");

          
            //_currentUser.FullName = dto.FullName;
            //_currentUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            //_currentUser.IsNewUser = false;
            //_currentUser.UserName = dto.UserName;
           
            //_currentUser.ImageFile = dto.ImageFile.FileName;


            //var m = _unitOfWork.Users.Update(_currentUser);
            _unitOfWork.Save();
            return Ok();
        }
        [HttpPost("refresh-token")]
        public async Task<ActionResult<string>> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var existinguser = await _unitOfWork.Users.FindAsync(user => user.RefreshToken == refreshToken);

            if (existinguser == null)
            {
                return Unauthorized("Invalid Refresh Token.");
            }
            else if (existinguser.TokenExpires < DateTime.Now)
            {
                return Unauthorized("Token Expired");
            }
            else
            {
                string token = _tokenService.GenerateAccessToken(existinguser);
                var newRefreshToken = _tokenService.GenerateRefreshToken();
                await _tokenService.SetRefreshToken(newRefreshToken, Response);
                return Ok(token);
            }
        }

        private bool isAciveSchool(string SchoolCode)
        {
            bool isActive = true;
           // var school = _unitOfWork.Schools.Find(m => m.SchoolCode == SchoolCode);
            var res = _unitOfWork.GetServerDate();
            //if (school.DateTo >= _unitOfWork.GetServerDate())
            //{
            //    isActive = true;
            //}
            return isActive;
        }
        private void GetInfo()
        {
            _userCode = HttpContext.User.Identity.Name;
            //_user = _unitOfWork.Users.Find(m => m.UserCode == _userCode);
            //_schoolCode = _user.SchoolCode;
        }
    }
}
