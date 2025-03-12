using BaseApi;
using DAL;
using Dto;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Prime.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "1,3,4")]
    public class StudentController : ApiBaseController
    {
        public StudentController(IUnitOfWork unitOfWork, IAppService appService) : base(unitOfWork, appService)
        {
        }
        [HttpGet("GetBooksServices")]
        public async Task<IActionResult> GetBooksServices()
        {
            var _userId = int.Parse(User.Identity.Name);
            var res = await _unitOfWork.BookPrints.FindAllAsync(m => m.UserId == _userId );
            return Ok(res);
        }
        [HttpGet("GetGameScore")]
        public async Task<IActionResult> GetGameScore(int bookId)
        {
            var _userId = int.Parse(User.Identity.Name);
            var res = await _unitOfWork.BookGameStudents.FindAsync(m => m.StudentId == _userId && m.BookId == bookId);
            return Ok(res);
        }
        [HttpPut("SetGameScore")]
        public async Task<IActionResult> SetGameScore(int bookId, decimal score)
        {
            var _userId = int.Parse(User.Identity.Name);
            var res = await _unitOfWork.BookGameStudents.FindAsync(m => m.StudentId == _userId && m.BookId == bookId);
            if (res == null)
            {
                BookGameStudent bookGameStudent = new BookGameStudent()
                {
                    BookId = bookId,
                    Score = score,
                    StudentId = _userId
                };
                await _unitOfWork.BookGameStudents.AddAsync(bookGameStudent);
            }
            else
            {
                res.Score = score;
                _unitOfWork.BookGameStudents.Update(res);
            }
            _unitOfWork.Save();
            return Ok(true);
        }
        [HttpGet("GetTeachers")]
        public async Task<IActionResult> GetTeachers()
        {
            var _userId = int.Parse(User.Identity.Name);
            var res = await _unitOfWork.StudentClasss.FindAllAsync(m => m.StudentId == _userId);
            var _techersIds = res.Select(m => m.TeacherId).ToList();
            var teachers = await _unitOfWork.Users.FindAllAsync(m => _techersIds.Contains(m.UserId));
            return Ok(teachers.Select(m => new { teacherid = m.UserId, TeacherName = m.FullName }));
        }

        [HttpGet("GetMyClasses")]
        public async Task<IActionResult> GetClasses()
        {
            var _userId = int.Parse(User.Identity.Name);
            var studentClasses = await _unitOfWork.StudentClasss.FindAllAsync(m => m.StudentId == _userId);
            var classCodes = studentClasses.Select(m => m.ClassCode).Distinct();
            var teacherClasses = await _unitOfWork.TeacherClasss.FindAllAsync(m => classCodes.Contains(m.ClassCode));
            /*var enrichedStudentClasses = studentClasses
        .Select(sc => new
        {
            sc.Id,
            sc.StudentId,
            sc.TeacherId,
            sc.ClassCode,
            sc.BookId,
            sc.AssignDate,
            ClassName = teacherClasses.FirstOrDefault(tc => tc.ClassCode == sc.ClassCode)?.ClassName
        });*/

            return Ok(teacherClasses);
        }

        [HttpGet("GetPracticesByType")]
        public async Task<IActionResult> GetPracticesByType(int practiceTypeId)
        {
            var _userId = int.Parse(User.Identity.Name);
            DateTime _endDate = _appService.GetDTM(_unitOfWork.GetServerDate(), _userId);
            var res = await _unitOfWork.PracticesAssignStudents.FindAllAsync(m => m.StudentId == _userId && m.IsStart == false);
            var _practiceCodes = res.Select(m => m.PracticeCode);
           // var res2 = await _unitOfWork.PracticesAssigns.FindAllAsync(m => _practiceCodes.Contains(m.PracticeCode) && m.DateTo >= _endDate && m.DateFrom <= _endDate);
           
            //var _practices = await _unitOfWork.Practices.FindAllAsync(m => _practiceCodes2.Contains(m.PracticeCode)&& m.PracticeTypeId==practiceTypeId);
            return Ok(); //_practices
        }
        [HttpGet("GetPractices")]
        public async Task<IActionResult> GetPractices()
        {
            var _userId = int.Parse(User.Identity.Name);
            DateTime _endDate = _appService.GetDTM(_unitOfWork.GetServerDate(), _userId);
            var res = await _unitOfWork.PracticesAssignStudents.FindAllAsync(m => m.StudentId == _userId && m.ValidDateFrom <=_endDate && m.ValidDateTo>= _endDate && (m.IsSubmited==false || m.IsEditable==true));
            var _practiceCodes = res.Select(m => m.PracticeCode);   
            var _practices = await _unitOfWork.Practices.FindAllAsync(m => _practiceCodes.Contains(m.PracticeCode));
            return Ok(_practices);  
        }
        [HttpGet("GetPracticesQuestions")]
        public async Task<IActionResult> GetPracticesQuestions(int practiceCode)
        {
            var practice = await _unitOfWork.Practices.FindAsync(m => m.PracticeCode == practiceCode);
            var questions = await _unitOfWork.Questions.FindAllAsync(m => m.PracticeCode == practiceCode);
            PracticeQuestionsDto dto = new PracticeQuestionsDto();
            dto.Practice = practice;
            dto.Questions = questions;
            return Ok(dto);
        }
        [HttpPut("SubmitPractice")]
        public async Task<IActionResult> SubmitPractice(List<StudentPracticeAnswerDto> dto)
        {
            var _userId = int.Parse(User.Identity.Name);
            var _practiceQuestionsStudent = await _unitOfWork.PracticeQuestionsStudents.FindAsync(m => m.QuestionId == dto[0].QuestionId && m.StudentId == _userId);
            var _practicesAssignStudent = await _unitOfWork.PracticesAssignStudents.FindAsync(m => m.PracticesAssignId == _practiceQuestionsStudent.PracticesAssignId);
            double? _finalScorse = 0;
            
            double? _totalScorse = 0;
            foreach (var r in dto)
            {
                var res = await _unitOfWork.PracticeQuestionsStudents.FindAsync(m => m.StudentId == _userId && m.QuestionId == r.QuestionId);
                res.StudentAnswer = r.StudentAnswer;
                res.StartDateTime = r.StartDateTime;
                res.EndDateTime = r.EndDateTime;
                if (_practicesAssignStudent.IsAutoCheckAnswer)
                {
                    if (res.ModelAnswer.ToLower().Trim() == r.StudentAnswer.ToLower().Trim())
                    {
                        res.StudentScore = res.Score;
                        _finalScorse += res.StudentScore;
                    }
                }
                _unitOfWork.PracticeQuestionsStudents.Update(res);
                _totalScorse += res.Score;
            }
            var _practicesAssignStudents= await _unitOfWork.PracticesAssignStudents.FindAsync(m => m.StudentId== _userId && m.PracticesAssignId== _practiceQuestionsStudent.PracticesAssignId) ;
            var minStartDateTime = dto.Where(a => a.StartDateTime.HasValue).Min(a => a.StartDateTime);
            var maxEndDateTime = dto.Where(a => a.EndDateTime.HasValue).Max(a => a.EndDateTime);
            _practicesAssignStudents.Score= _finalScorse;
            _practicesAssignStudents.IsSubmited =true;
            _practicesAssignStudents.TotalScore = _totalScorse;
            _practicesAssignStudents.QuestionsCount = dto.Count;
            _practicesAssignStudents.AnswerStartDate  = minStartDateTime;
            _practicesAssignStudents.AnswerEndDate = maxEndDateTime;  
            _unitOfWork.PracticesAssignStudents.Update(_practicesAssignStudents);
            _unitOfWork.Save();
            return Ok(true);
        }
        [HttpGet("GetAttendance")]
        public async Task<IActionResult> GetAttendance( )
        {
            var _userId = int.Parse(User.Identity.Name);
            var res = await _unitOfWork.Attendances.FindAllAsync(m => m.StudentId == _userId);
            return Ok(res);
        }

        [HttpGet("GetStudentAttendance")]
        public async Task<IActionResult> GetStudentAttendance(int _userId)
        {
            var res = await _unitOfWork.Attendances.FindAllAsync(m => m.StudentId == _userId);
            return Ok(res);
        }

        [HttpGet("GetResult")]//Get the Question and answer anc Compare with studetns awers
        public async Task<IActionResult> GetResult(int practiceCode)
        {
            var _userId = int.Parse(User.Identity.Name);
            var res=await _unitOfWork.PracticesAssignStudents.FindAsync(m=>m.StudentId==_userId && m.PracticeCode==practiceCode);
            return Ok(true);
        }
        [HttpGet("GetStudentResult")]//Get the Question and answer anc Compare with studetns awers
        public async Task<IActionResult> GetStudentResult(int _userId)
        {
            var res = await _unitOfWork.PracticesAssignStudents.FindAsync(m => m.StudentId == _userId);
            return Ok(true);
        }
    }
}