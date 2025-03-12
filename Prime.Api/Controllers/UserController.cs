using BaseApi;
using DAL;
using Dto;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Services;
using System.Data;
using Token;
namespace Prime.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ApiBaseController
{
    private readonly ITokenService _tokenService;
    private string _imagesPath = "files\\images\\users\\";
    private string _ekitPath = "files\\ppk\\";
    public UserController(IUnitOfWork unitOfWork, IAppService appService, ITokenService tokenService) : base(unitOfWork, appService)
    {
        _tokenService = tokenService;
    }
   
    #region UserAccountAndLogin
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromForm] UserDto model)
    {
        var res = _tokenService.RegisterAsync(model);
        if (res.Result.IsSuccess)
        {
            return Ok(res.Result);
        }

        return BadRequest(res.Result);
    }

    [HttpGet("CheckValidity")]
    public IActionResult CheckValidity(string userName)
    {
        return Ok(!_tokenService.isUserExist(userName));
    }
    [HttpPut("UpdateById")]
    [Authorize(Roles = "1,2,3,4")]
    public IActionResult UpdateById([FromForm] UserUpdateDto model)
    {
      
        var _currentUser = _unitOfWork.Users.Find(m => m.UserName == model.UserName);
        string _extension = "", _defaultImage = null;
        if (model.ImageFile != null)
        {
            string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff" };
            _extension = Path.GetExtension(model.ImageFile.FileName).ToLower();
            if (!imageExtensions.Contains(_extension))
            {
                return BadRequest("Invalid Image File");
            }
            _defaultImage = _appService.Uploade(model.ImageFile, _imagesPath);
            if (!string.IsNullOrEmpty(_currentUser.ImageFile))
                _appService.Delete(_imagesPath + _currentUser.ImageFile);
            _currentUser.ImageFile = _defaultImage;
        }
        _currentUser.FullName = model.FullName;
        _currentUser.CountryId = model.CountryId;
        var m = _unitOfWork.Users.Update(_currentUser);
        _unitOfWork.Save();
        return Ok();
    }
    [HttpPut("Update")]
    [Authorize(Roles = "1,2,3,4")]
    public IActionResult Update([FromForm] UserUpdateDto model)
    {
        var _userId = int.Parse(User.Identity.Name);
        var _currentUser = _unitOfWork.Users.Find(m => m.UserId == _userId);
        string _extension = "", _defaultImage = null;
        if (model.ImageFile != null)
        {
            string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff" };
            _extension = Path.GetExtension(model.ImageFile.FileName).ToLower();
            if (!imageExtensions.Contains(_extension))
            {
                return BadRequest("Invalid Image File");
            }
            _defaultImage = _appService.Uploade(model.ImageFile, _imagesPath);
            if (!string.IsNullOrEmpty(_currentUser.ImageFile))
                _appService.Delete(_imagesPath + _currentUser.ImageFile);
            _currentUser.ImageFile = _defaultImage;
        }
        _currentUser.FullName = model.FullName;
        _currentUser.CountryId = model.CountryId;
        var m = _unitOfWork.Users.Update(_currentUser);
        _unitOfWork.Save();
        return Ok();
    }
    [HttpPut("ChangeRole")]
    [Authorize(Roles = "4,5")]
    public IActionResult ChangeRole(UserUpdateRoleDto model)
    {
        var _currentUser = _unitOfWork.Users.Find(m => m.UserName == model.UserName);
        _currentUser.RoleId = model.RoleId;
        _unitOfWork.Users.Update(_currentUser);
        _unitOfWork.Save();
        return Ok(true);
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLoginDto model)
    {
        var result = await _tokenService.LoginAsync(model);
        if (result.IsAuthenticated)
            SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);
        return Ok(result);
    }

    [HttpGet("RefreshToken")]
    public async Task<IActionResult> RefreshToken()
    {
        var refreshToken = Request.Cookies["refreshToken"];

        var result = await _tokenService.RefreshTokenAsync(refreshToken);

        if (!result.IsAuthenticated)
            return BadRequest(result);

        SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

        return Ok(result);
    }

    [HttpPost("RevokeToken")]
    public async Task<IActionResult> RevokeToken([FromBody] string? token)
    {
        var _token = token ?? Request.Cookies["refreshToken"];

        if (string.IsNullOrEmpty(_token))
            return BadRequest("Token is required!");

        var result = await _tokenService.RevokeTokenAsync(_token);

        if (!result)
            return BadRequest("Token is invalid!");

        return Ok();
    }

    #endregion

    #region GetData
    [HttpGet("GetData")]
    [Authorize(Roles = "1,2,3,4")]
    public async Task<IActionResult> GetData()
    {
        var _userId = int.Parse(User.Identity.Name);
        var m = await _unitOfWork.Users.FindAsync(m => m.UserId == _userId, new string[] { "Country" });
        return Ok(new UserData { UserId = m.UserId, UserName = m.UserName, FullName = m.FullName, ImageFile = m.ImageFile, RoleId = m.RoleId, Country = m.Country.CountryName, CountryUtc = m.Country.CountryNameUtc,TimeDif=m.Country.TimeDif });
    }

    [HttpGet("GetUserDataById")]
    [Authorize(Roles = "1,2,3,4")]
    public async Task<IActionResult> GetUserDataById(int userId)
    {
        var m = await _unitOfWork.Users.FindAsync(m => m.UserId == userId);
        return Ok(m);
    }
    [HttpGet("GetUserData")]
    [Authorize(Roles = "1,2,3,4")]
    public async Task<IActionResult> GetUserData()
    {
        var _userId = int.Parse(User.Identity.Name);
        var m = await _unitOfWork.Users.FindAsync(m => m.UserId == _userId);
        return Ok(m);
    }
    [HttpGet("GetLocalDateTimeByUser")]
    [Authorize(Roles = "1,2,3,4")]
    public async Task<IActionResult> GetLocalDateTimeByUser(int userId)
    {
        var m = _appService.GetDTM(_unitOfWork.GetServerDate(), userId);
        return Ok(m);
    }
    [HttpGet("GetLocalDateTime")]
    [Authorize(Roles = "1,2,3,4")]
    public async Task<IActionResult> GetLocalDateTime()
    {
        var _userId = int.Parse(User.Identity.Name);
        var m = _appService.GetDTM(_unitOfWork.GetServerDate(), _userId);
        return Ok(m);
    }
    [HttpGet("GetLog")]
    [Authorize(Roles = "2,3,4")]
    public async Task<IActionResult> GetLog(int userId)
    {
        var res = await _unitOfWork.Logs.FindAllAsync(m => m.UserId == userId);
        return Ok(res);
    }
    [HttpGet("GetLogV")]
    [Authorize(Roles = "2,3,4")]
    public async Task<IActionResult> GetLogV(int userId)
    {
        var res = await _unitOfWork.LogV.FindAllAsync(m => m.UserId == userId);
        return Ok(res);
    }
    [HttpPut("ChangePassword")]
    [Authorize(Roles = "1,2,3,4")]
    public async Task<IActionResult> ChangePassword(StringUpdateDto model)
    {
        var _userId = int.Parse(User.Identity.Name);
        var _user = await _unitOfWork.Users.FindAsync(m => m.UserId == _userId);
        _user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.StringValue);
        _unitOfWork.Users.Update(_user);
        _unitOfWork.Save();
        return Ok(true);
    }
    [HttpPut("ChangePasswordById")]
    [Authorize(Roles = "1,2,3,4")]
    public async Task<IActionResult> ChangePasswordById(StringUpdateDto model)
    {
        var _userId = int.Parse(User.Identity.Name);
        var _user = await _unitOfWork.Users.FindAsync(m => m.UserId == _userId);
        _user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.StringValue);
        _unitOfWork.Users.Update(_user);
        _unitOfWork.Save();
        return Ok(true);
    }
    [HttpPut("ChangeTheme")]
    [Authorize(Roles = "1,2,3,4")]
    public async Task<IActionResult> ChangeTheme(IntUpdateDto model)
    {
        var _userId = int.Parse(User.Identity.Name);
        var _user = await _unitOfWork.Users.FindAsync(m => m.UserId == _userId);
        _user.ThemeId = model.IntValue;
        _unitOfWork.Users.Update(_user);
        _unitOfWork.Save();
        return Ok(true);
    }
    [HttpGet("GetProfileImage")]
    [Authorize(Roles = "1,2,3,4")]
    public async Task<IActionResult> GetProfileImage()
    {
        var _userId = int.Parse(User.Identity.Name);
        var m = _unitOfWork.Users.Find(m => m.UserId == _userId);
        if (m.ImageFile == null || m.ImageFile == "")
        {
            return NotFound();
        }
        string _fileName = m.ImageFile;
        var result = await _appService.DownloadFile(_imagesPath + _fileName);
        return File(result.Item1, result.Item2, result.Item2);
    }
    [HttpGet("GetProfileImageByUser")]
    [Authorize(Roles = "1,2,3,4")]
    public async Task<IActionResult> GetProfileImageByUser(int userId)
    {

        var m = _unitOfWork.Users.Find(m => m.UserId == userId);
        if (m.ImageFile == null || m.ImageFile == "")
        {
            return NotFound();
        }
        string _fileName = m.ImageFile;
        var result = await _appService.DownloadFile(_imagesPath + _fileName);
        return File(result.Item1, result.Item2, result.Item2);
    }

    [HttpDelete("DeleteProfileImage")]
    [Authorize(Roles = "1,2,3,4")]
    public async Task<IActionResult> DeleteProfileImage()
    {
        var _userId = int.Parse(User.Identity.Name);
        var m = _unitOfWork.Users.Find(m => m.UserId == _userId);
        _appService.Delete(_imagesPath + m.ImageFile);
        m.ImageFile = null;
        _unitOfWork.Users.Update(m);

        _unitOfWork.Save();
        return Ok(true);
    }
    [HttpDelete("DeleteImage")]
    [Authorize(Roles = "1,2,3,4")]
    public async Task<IActionResult> DeleteImage(string userName)
    {
        var m = _unitOfWork.Users.Find(m => m.UserName == userName);
        _appService.Delete(_imagesPath + m.ImageFile);
        m.ImageFile = null;
        _unitOfWork.Users.Update(m);

        _unitOfWork.Save();
        return Ok(true);
    }

    #endregion

    #region EnrollCodeAndClass


    [HttpPut("EnrollCode")]
    [Authorize(Roles = "1,2")]
    public async Task<IActionResult> EnrollCode(string code)
    {
        if (code == null)
        {
            return BadRequest("Please enter a valid code.");
        }
        int _codeLength = code.Length;

        int[] _len = new int[] { 12, 10, 6,14,15,16,17,18,19,20,21 };
        if (!_len.Contains(_codeLength))
        {
            return BadRequest("Invalid Code");
        }
        var _userId = int.Parse(User.Identity.Name);
        var _currentUser = _unitOfWork.Users.Find(m => m.UserId == _userId);
        if (_codeLength >= 12)
        {
            string res = CheckBookCode(code);
            if (res == "")
            {
                if (_currentUser.IsNewUser)
                {
                    _currentUser.IsNewUser = false;
                    _unitOfWork.Users.Update(_currentUser);
                }
                else
                {
                    if (_currentUser.RoleId == 2)
                    {
                        return BadRequest("Invalid Teacher Code");
                    }
                }
                var _bookpintCode = _unitOfWork.BookPrints.Find(m => m.BkCode == code);
                var _bookpintusers = _unitOfWork.BookPrints.FindAll(m => m.UserId == _currentUser.UserId);
                if ((_bookpintusers != null && _bookpintusers.All(m=>m.BookId != _bookpintCode.BookId)) || _bookpintusers == null || _bookpintusers.Count()==0)
                {
                    _bookpintCode.UserId = _currentUser.UserId;
                    _bookpintCode.AssignDate = _unitOfWork.GetServerDate();
                    _bookpintCode.ValidUpToDate = _unitOfWork.GetServerDate().AddYears(1);
                    _unitOfWork.BookPrints.Update(_bookpintCode);
                    AddUserBook(_currentUser.UserId, _bookpintCode.BookId, _bookpintCode.AssignDate);
                    var _codePatch = _unitOfWork.CodePatchs.Find(m => m.Id == _bookpintCode.PatchNumber);
                    _codePatch.IsOneAssinged = true;
                    _unitOfWork.CodePatchs.Update(_codePatch);
                }
                else
                {
                    return BadRequest("Already assigned to this book.");
                }
            }
            else
            {
                return BadRequest(res);
            }
        }
        if (_codeLength == 10)
        {
            string res = CheckTeacherCode(code);
            if (res == "")
            {
                if (_currentUser.IsNewUser)
                {
                    _currentUser.RoleId = 2;
                    _currentUser.IsNewUser = false;
                    _unitOfWork.Users.Update(_currentUser);
                }
                if (!_currentUser.IsNewUser && _currentUser.RoleId != 2)
                {
                    return BadRequest("This code is allowed to teachers only.");
                }
                var _teacherCode = _unitOfWork.TeacherCodes.Find(m => m.TCode == code);
                var _booksIds = _unitOfWork.TeacherCodes.FindAll(m => m.UserId == _currentUser.UserId).Select(m => m.BookId);
                if (_booksIds != null && !_booksIds.Contains(_teacherCode.BookId) || _booksIds == null)
                {
                    _teacherCode.UserId = _currentUser.UserId;
                    _teacherCode.AssignDate = _unitOfWork.GetServerDate();
                    _teacherCode.ValidUpToDate = _unitOfWork.GetServerDate().AddYears(1);
                    _unitOfWork.TeacherCodes.Update(_teacherCode);
                    AddUserBook(_currentUser.UserId, _teacherCode.BookId, _teacherCode.AssignDate);
                    var _codePatch = _unitOfWork.CodePatchs.Find(m => m.Id == _teacherCode.PatchNumber);
                    _codePatch.IsOneAssinged = true;
                    _unitOfWork.CodePatchs.Update(_codePatch);
                    CreateClass(_teacherCode.BookId);
                    if(_teacherCode.AdminId != null)
                    {
                        var _teacher = _unitOfWork.Users.Find(m => m.UserId == _currentUser.UserId);
                        _teacher.AdminId = _teacherCode.AdminId;
                        _unitOfWork.Users.Update(_teacher);
                    }
                }
                else
                {
                    return BadRequest("Already assigned to this book.");
                }
            }
            else
            {
                return BadRequest(res);
            }
        }
        if (_codeLength == 6)
        {
            if (_currentUser.RoleId != 1)
            {
                return BadRequest("This code is allowed to students only.");
            }
            string res = CheckClassCode(code);
            if (res == "")
            {
                var m = _unitOfWork.TeacherClasss.Find(m => m.ClassCode == code);
                var teacherData = _unitOfWork.Users.Find(u => u.UserId == m.TeacherId);
                if (teacherData.AdminId != null)
                {
                    _currentUser.AdminId = teacherData.AdminId;
                }
                if (_currentUser.Books == null)
                {
                    _currentUser.Books = m.BookId.ToString();
                    _currentUser.Clasess = code;
                    _currentUser.Teachers=m.TeacherId.ToString();
                }
                else
                {
                    _currentUser.Books += "|"+ m.BookId.ToString();
                    _currentUser.Clasess +="|"+ code;
                    _currentUser.Teachers += "|" + m.TeacherId.ToString();
                }
                await AddStudentClass(_currentUser.UserId, m.BookId, code, _unitOfWork.GetServerDate());
            }
            else
            {
                return BadRequest(res);
            }
        }
        _unitOfWork.Save();
        return Ok("Done");
    }
    #endregion


    #region PrivateMethods

    private bool IsValidBookPrintCode(string code)
    {
        return _unitOfWork.BookPrints.IsExist(m => m.BkCode == code && m.UserId == null);
    }
    private string CheckTeacherCode(string code)
    {
        string res = "";
        var m = _unitOfWork.TeacherCodes.Find(m => m.TCode == code);
        if (m == null)
            res = "Enter a valid code.";
        if (m != null && m.UserId != null)
            res = "This code is already used by another user.";
        return res;
    }
    private string CheckBookCode(string code)
    {
        string res = "";
        var m = _unitOfWork.BookPrints.Find(m => m.BkCode == code || m.IsBlocked==true);
        if (m == null)
            res = "Enter a valid code.";
        if (m != null && m.UserId != null)
            res = "This code is already used.";
        return res;
    }
    private string CheckClassCode(string code)
    {
        string res = "";
        var _teacherClasss = _unitOfWork.TeacherClasss.Find(m => m.ClassCode == code);
        if (_teacherClasss == null)
            return "Enter a valid code.";
        var _userId = int.Parse(User.Identity.Name);
        var _studentClass = _unitOfWork.StudentClasss.Find(m => m.ClassCode == code && m.StudentId == _userId);
        if (_studentClass != null)
        {
            return "Already assigned to this class.";
        }
        var _studentBookClass = _unitOfWork.StudentClasss.Find(m => m.BookId == _teacherClasss.BookId && m.StudentId == _userId);
        if (_studentBookClass != null)
        {
            return "Already assigned to this book with another class.";
        }
        int[] userBooks = _unitOfWork.UserBooks.FindAll(m => m.UserId == _userId).Select(m => m.BookId).ToArray();
        if (!userBooks.Contains(_teacherClasss.BookId))
        {
            return "You are not enrolled in the book of this class.";
        }
        return res;
    }
    private void AddUserBook(int _userId, int _bookId, DateTime? _dt)
    {
        UserBook _userBook = new UserBook()
        {
            UserId = _userId,
            BookId = _bookId,
            AssignDate = _dt

        };
        _unitOfWork.UserBooks.Add(_userBook);
    }
    private async Task AddStudentClass(int _userId, int _bookId, string _classCode, DateTime? _dt)
    {
        var _teacherClasss = await _unitOfWork.TeacherClasss.FindAsync(m => m.ClassCode == _classCode);
        StudentClass _studentClass = new StudentClass()
        {
            StudentId = _userId,
            ClassCode = _classCode,
            TeacherId = _teacherClasss.TeacherId,
            BookId = _bookId,
            AssignDate = _dt
        };
        await _unitOfWork.StudentClasss.AddAsync(_studentClass);
    }
    private void SetRefreshTokenInCookie(string refreshToken, DateTime expires)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = expires.ToLocalTime(),
            // Secure = true,
            //IsEssential = true,
            // SameSite = SameSiteMode.None
        };

        Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
    }

    private void CreateClass(int bookId)
    {
        var _userId = int.Parse(User.Identity.Name);

        string _code = "";
        bool flag = false;
        for (int i = 0; i < 10; i++)
        {
            _code = _appService.GenerateCode(6);
            flag = _unitOfWork.TeacherClasss.IsExist(m => m.ClassCode == _code);
            if (flag)
                continue;
            else
                break;
        }
        if (!flag)
        {
            var _bokName = _unitOfWork.Books.Find(m => m.BookId == bookId);
            TeacherClass _class = new TeacherClass()
            {
                ClassCode = _code,
                ClassName = _bokName.BookName,
                TeacherId = _userId,
                BookId = bookId
            };
            _unitOfWork.TeacherClasss.Add(_class);
        }
    }

    #endregion
}
