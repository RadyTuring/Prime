using BaseApi;
using Dto;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;
using System.Data;
using System.Linq.Expressions;

namespace Prime.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "4,5")]
    public class CodeGenController : ApiBaseController
    {
        public CodeGenController(IUnitOfWork unitOfWork, IAppService appService) : base(unitOfWork, appService)
        {
        }
        #region GetCodes

        [HttpGet("GetBookPrintCodes")]
        public async Task<IActionResult> GetBookPrintCodes(int id)
        {
            var res = await _unitOfWork.BookPrints.FindAllAsync(m => m.PatchNumber == id);
            var res2 = res.Select(m => new BookPrintCodeCsv() { BookStudentCode = m.BkCode }).ToList();
            return Ok(res2);
        }

        [HttpGet("GetTeacherCodes")]
        public async Task<IActionResult> GetTeacherCodes(int id)
        {
            var res = await _unitOfWork.TeacherCodes.FindAllAsync(m => m.PatchNumber == id);
            var res2 = res.Select(m => new TeacherCodeCsv() { TeacherCode = m.TCode }).ToList();
            return Ok(res2);
        }

        [HttpGet("GetAdminCodes")]
        public async Task<IActionResult> GetAdminCodes(int id)
        {
            var res = await _unitOfWork.Users.FindAllAsync(m => m.PatchNumber == id);
            var res2 = res.Select(m => new AdminCodesCsv() { AdminUser = m.UserName, Password = m.UserName }).ToList();
            return Ok(res2);
        }

        [HttpGet("IsBookPrintCodesAssigned")]
        public async Task<IActionResult> IsBookPrintCodesAssigned(int id)
        {
            var IsExist = _unitOfWork.BookPrints.IsExist(m => m.PatchNumber == id && m.UserId != null);
            return Ok(IsExist);
        }
        #endregion

        #region Generate_Delete

        [HttpDelete("DeleteCodes")]
        public async Task<IActionResult> DeleteCodes(int id)
        {
            var _codePatch = await _unitOfWork.CodePatchs.GetByIdAsync(id);
            if (_codePatch.PatchType.ToLower() == "book")
            {
                var _bookPrints = await _unitOfWork.BookPrints.FindAllAsync(m => m.PatchNumber == id);
                _unitOfWork.BookPrints.DeleteRange(_bookPrints);
            }
            else if (_codePatch.PatchType.ToLower() == "teacher")
            {
                var _teacherCodes = await _unitOfWork.TeacherCodes.FindAllAsync(m => m.PatchNumber == id);
                _unitOfWork.TeacherCodes.DeleteRange(_teacherCodes);
            }
            else if (_codePatch.PatchType.ToLower() == "admin")
            {
                var _adminCodes = await _unitOfWork.Users.FindAllAsync(m => m.PatchNumber == id);
                _unitOfWork.Users.DeleteRange(_adminCodes);
            }

            _unitOfWork.CodePatchs.Delete(_codePatch);
            _unitOfWork.Save();
            return Ok(true);
        }
        [HttpPost("GenBookPrintCodes")]
        
        public async Task<IActionResult> GenBookPrintCodes(BookPrintDto dto)
        {
            var _userId = int.Parse(User.Identity.Name);
            var _currentUser = _unitOfWork.Users.Find(m => m.UserId == _userId);
            CodePatch codePatch = new CodePatch()
            {
                PatchDesc = dto.PatchDesc,
                PatchType = dto.PatchType,
                BookId = dto.BookId,
                UserName = _currentUser.UserName,
                NumberOfCodes = dto.NumberOfCodes
            };

            _unitOfWork.CodePatchs.Add(codePatch);
            _unitOfWork.Save();
            var _max = _unitOfWork.CodePatchs.Max(m => m.Id);
            List<BookPrint> _bookPrints = new List<BookPrint>();
            var _book = _unitOfWork.Books.Find(m => m.BookId == dto.BookId);
            for (int i = 0; i < dto.NumberOfCodes; i++)
            {
                string _code = _appService.GenerateCode(9);
                BookPrint bookPrint = new BookPrint()
                {
                    PatchNumber = (int)_max,
                    BookId = dto.BookId,
                    BookServices = string.Join(",", dto.BookServices),
                    BkCode = _book.Prefix + _code,
                    IncludeEkit = dto.IncludeEkit
                };
                _bookPrints.Add(bookPrint);
            }
            await _unitOfWork.BookPrints.AddRangeAsync(_bookPrints);
            _unitOfWork.Save();
            var res = await _unitOfWork.BookPrints.FindAllAsync(m => m.PatchNumber == _max);
            var res2 = res.Select(m => new BookPrintCodeCsv() { BookStudentCode = m.BkCode }).ToList();
            return Ok(res2);
        }
       
        [HttpPost("GenTeacherCodes")]
        public async Task<IActionResult> GenTeacherCodes(TeacherCodeDto dto)
        {
            var _userId = int.Parse(User.Identity.Name);
            var _currentUser = _unitOfWork.Users.Find(m => m.UserId == _userId);
            CodePatch codePatch = new CodePatch()
            {
                PatchDesc = dto.PatchDesc,
                PatchType = dto.PatchType,
                BookId = dto.BookId,
                UserName = _currentUser.UserName,
                NumberOfCodes = dto.NumberOfCodes
            };

            _unitOfWork.CodePatchs.Add(codePatch);
            _unitOfWork.Save();
            var _max = _unitOfWork.CodePatchs.Max(m => m.Id);
            List<TeacherCode> teacherCodes = new List<TeacherCode>();
            var _book = _unitOfWork.Books.Find(m => m.BookId == dto.BookId);
            for (int i = 0; i < dto.NumberOfCodes; i++)
            {
                string _code = _appService.GenerateCode(10);
                TeacherCode _teacherCode = new TeacherCode()
                {
                    PatchNumber   = (int)_max,
                    BookId = dto.BookId,

                    TCode = _code,

                };
                teacherCodes.Add(_teacherCode);
            }
            await _unitOfWork.TeacherCodes.AddRangeAsync(teacherCodes);
            _unitOfWork.Save();
            var res = await _unitOfWork.TeacherCodes.FindAllAsync(m => m.PatchNumber == _max);
            var res2 = res.Select(m => new TeacherCodeCsv() { TeacherCode = m.TCode }).ToList();
            return Ok(res2);
        }

        [HttpPost("GenAdminCodes")] 
        public async Task<IActionResult> GenAdminCodes(AdminUserDto dto)
        {
            var _userId = int.Parse(User.Identity.Name);
            var _currentUser = _unitOfWork.Users.Find(m => m.UserId == _userId);
            CodePatch codePatch = new CodePatch()
            {
                PatchDesc = dto.PatchDesc,
                PatchType = dto.PatchType,
                UserName = _currentUser.UserName,
                NumberOfCodes = dto.NumberOfCodes
            };

            _unitOfWork.CodePatchs.Add(codePatch);
            _unitOfWork.Save();
            var _max = _unitOfWork.CodePatchs.Max(m => m.Id);
            List<User> _adminUsers = new List<User>();
            for (int i = 0; i < dto.NumberOfCodes; i++)
            {
                string _adminCode = _appService.GenerateCode(9);
                User _adminUser = new Entities.User()
                {
                    PatchNumber = (int)_max,
                    UserName = _adminCode,
                    AdminCode = _adminCode,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(_adminCode),
                    RoleId = 3

                };
                _adminUsers.Add(_adminUser);
            }
            await _unitOfWork.Users.AddRangeAsync(_adminUsers);
            _unitOfWork.Save();
            var res = await _unitOfWork.Users.FindAllAsync(m => m.PatchNumber == _max);
            var res2 = res.Select(m => new AdminCodesCsv() { AdminUser = m.UserName, Password = m.UserName }).ToList();
            return Ok(res2);
        }

        #endregion

        [HttpPost("GetPrimeCodes")]
        public async Task<IActionResult> GetPrimeCodes(CodeSearchDto dto)
        {
            var criteria = BuildCriteria<CodePatch>(dto);
            var res = await _unitOfWork.CodePatchs.FindAllAsync(criteria, new string[] { "Book" });
            return Ok(res);
        }

        [HttpGet("IsExist")]
        public IActionResult IsExist(string name)
        {
            return Ok(_unitOfWork.CodePatchs.IsExist(m => m.PatchDesc == name));
        }

        private Expression<Func<T, bool>> BuildCriteria<T>(CodeSearchDto dto)
        {
            var parameterExp = Expression.Parameter(typeof(T));
            Expression conditionExp = Expression.Constant(true);
            try
            {
                var patchType = Expression.Equal(Expression.Property(parameterExp, "PatchType"), Expression.Constant(dto.PatchType));
                conditionExp = Expression.AndAlso(conditionExp, patchType);

                if (dto.BookId != 0)
                {
                    if (dto.BookId.HasValue)
                    {
                        var bookIdExp = Expression.Equal(
                            Expression.Property(parameterExp, "BookId"),
                            Expression.Constant(dto.BookId, typeof(int?))
                        );
                        conditionExp = Expression.AndAlso(conditionExp, bookIdExp);
                    }
                }

                if (dto.UserName != "--Choose--")
                {
                    var userNameExp = Expression.Equal(Expression.Property(parameterExp, "UserName"), Expression.Constant(dto.UserName));
                    conditionExp = Expression.AndAlso(conditionExp, userNameExp);
                }

                if (dto.PatchDesc != "--Choose--")
                {
                    var descriptionExp = Expression.Equal(Expression.Property(parameterExp, "PatchDesc"), Expression.Constant(dto.PatchDesc));
                    conditionExp = Expression.AndAlso(conditionExp, descriptionExp);
                }

                if (dto.DateFrom != null && dto.DateTo != null)
                {
                    var dateFromExp = Expression.GreaterThanOrEqual(Expression.Property(parameterExp, "PatchDate"), Expression.Constant(dto.DateFrom));
                    conditionExp = Expression.AndAlso(conditionExp, dateFromExp);
                    var dateToExp = Expression.LessThanOrEqual(Expression.Property(parameterExp, "PatchDate"), Expression.Constant(dto.DateTo));
                    conditionExp = Expression.AndAlso(conditionExp, dateToExp);
                }




            }
            catch (Exception ex)
            {

                throw;
            }


            var lambdaExp = Expression.Lambda<Func<T, bool>>(conditionExp, parameterExp);
            return lambdaExp;
        }
    }
}
