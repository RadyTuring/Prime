using Dto;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics.Metrics;
using System.Transactions;

namespace Prime.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PracticesController : ControllerBase
    {
        private  string _userCode;
        private  string _schoolCode;
        private User _user;
        private string _sep;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAppService _appService;
        public PracticesController(IUnitOfWork unitOfWork, IAppService appService)
        {
            _unitOfWork = unitOfWork;
            _appService = appService;
            
        }

        [HttpPost("Add")]
        [Authorize(Roles = "teacher")]
        public async Task<IActionResult> Add(PracticesDto dto)
        {
            //GetInfo();
            if (isExistTitle(dto.Practice.PracticeTitle))
                return BadRequest("The Title already Exist");

            #region Add Practice
            int count = _unitOfWork.Practices.Count(m => m.SchoolCode == _schoolCode);
            string _practiceCode = _schoolCode + (count + 1).ToString();
            Practice practice = new Practice()
            {
                PracticeCode = _practiceCode,
                PracticeTypeId = dto.Practice.PracticeTypeId,
                PracticeTitle = dto.Practice.PracticeTitle,
                SchoolCode = _schoolCode,
                TeacherCode = _userCode,
                BookId = dto.Practice.BookId,
                IsShared = dto.Practice.IsShared,
                DurationByMin = dto.Practice.DurationByMin,
                TranDt = DateTime.Now

            };
            _unitOfWork.Practices.Add(practice);
            #endregion

            #region Add practicesAssign

            PracticesAssign practicesAssign = new PracticesAssign()
            {
                PracticeCode = _practiceCode,
                SchoolCode = _schoolCode,
                TeacherCode = _userCode,
                AssignToClasses = dto.Publish.AssignToClasses,
                DateFrom = _appService.GetDTM(dto.Publish.DateFrom, _schoolCode),
                DateTo = _appService.GetDTM(dto.Publish.DateTo, _schoolCode),
                IsAutoCheckAnswer = dto.Publish.IsAutoCheckAnswer,
                DurationByMin = dto.Practice.DurationByMin,
                PracticeTypeid = dto.Practice.PracticeTypeId
            };
            _unitOfWork.PracticesAssigns.Add(practicesAssign);
            #endregion

            #region AddQuestions
            foreach (QuestionDto q in dto.Questions)
            {
                int _countQuestionFiles = 0;
                int _countAnswerFiles = 0;

                string questionFilesName = null, answerFilesNames = null;
                if (q.QuestionFiles != null)
                {
                    _countQuestionFiles = q.QuestionFiles.Count;
                }
                if (q.AnswerFiles != null)
                {
                    _countAnswerFiles = q.AnswerFiles.Count;
                }

                if (_countQuestionFiles != _countAnswerFiles)
                {
                    return BadRequest("Differenct numebr of Question and Answer files");
                }
                if (_countQuestionFiles > 0 && _countAnswerFiles > 0)
                {
                    questionFilesName = _appService.Uploade(q.QuestionFiles, "Files");
                    answerFilesNames = _appService.Uploade(q.AnswerFiles, "Files");
                }
                Question Question = new Question()
                {
                    TeacherCode = _userCode,
                    PracticeCode = _practiceCode,
                    SchoolCode = _schoolCode,
                    BookId = dto.Practice.BookId,
                    BookTopicId = q.BookTopicId,
                    QuestionTypeId = q.QuestionTypeId,
                    Dificulty = q.Dificulty,
                    OrderNo= q.OrderNo, 
                    ParentQuestionTitle = q.ParentQuestionTitle,
                    IsManyChoicesAnswer = q.IsManyChoicesAnswer,
                    DurationByMin = q.DurationByMin,
                    QuestionTitle = q.QuestionTitle,
                     
                    QuestionChoices = q.QuestionChoices,
                    ModelAnswer = q.ModelAnswer,
                    Score = q.Score,
                    QuestionKeywords = q.QuestionKeywords,
                    QuestionMedia = questionFilesName,
                    AnswerMedia = answerFilesNames,

                };
                await _unitOfWork.Questions.AddAsync(Question);
            }
            #endregion

            #region SendNotifications
            string[] _classes = dto.Publish.AssignToClasses.Split(_sep);
            string _noteCateg = _unitOfWork.PracticeTypes.Find(m => m.TypeId == dto.Practice.PracticeTypeId).TypeName;
            foreach (var _cl in _classes)
            {
                //_appService.SendNotification(_cl, dto.Practice.PracticeTitle, _noteCateg, _userCode, practicesAssign.DateFrom, practicesAssign.DateTo,true);
            }
     
            #endregion
            _unitOfWork.Save();
            return Ok("Success");
        }
        [HttpPost("IsExist")]
        [Authorize(Roles = "teacher")]
        public IActionResult IsExist(string title)
        {
            return Ok(isExistTitle(title));
        }
        private bool isExistTitle(string title)
        {
            bool b = _unitOfWork.Practices.IsExist(m => m.PracticeTitle == title && m.SchoolCode == _schoolCode);
            return b;
        }
        
    }
}
