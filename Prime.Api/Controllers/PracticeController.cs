using BaseApi;
using Dto;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;
using Token;

namespace Prime.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "2,3,4")]
    public class PracticeController : ApiBaseController
    {
      public PracticeController(IUnitOfWork unitOfWork, IAppService appService) : base(unitOfWork, appService)
        {
        }
        [HttpPut("DeletePractice")]
        public IActionResult DeletePractice(int practiceCode)
        {
            var _practice = _unitOfWork.Practices.Find(m => m.PracticeCode == practiceCode);
            if (_practice.IsGlobal)
            {
                return BadRequest("You can't delete this practice.");
            }

            if (_practice.IsAssigned)
            {
                return BadRequest("You can't delete this practice. Already assigned to student(s).");
            }
            _unitOfWork.Practices.Delete(_practice);
            var _questionsofPratice = _unitOfWork.Questions.FindAll(m => m.PracticeCode == practiceCode);
            _unitOfWork.Questions.DeleteRange(_questionsofPratice);
            _unitOfWork.Save();
            return Ok(true);
        }
        [HttpPost("EditPractice")]
        public async Task<IActionResult> EditPractice(PracticesDto dto, int practiceCode)
        {
            var _userId = int.Parse(User.Identity.Name);
            var _practice = _unitOfWork.Practices.Find(m => m.PracticeCode == practiceCode);

            if (_practice == null)
            {
                return NotFound("Practice not found.");
            }

            if (_practice.IsAssigned)
            {
                return BadRequest("The practice cannot be edited. Already assigned to student(s).");
            }

            if (_userId != _practice.TeacherId)
            {
                return BadRequest("You are not the author of this practice.");
            }

            if (IsTitleExist(dto.Practice.PracticeTitle, practiceCode, _practice.TeacherId))
            {
                return BadRequest("The title of this practice already exists.");
            }

            var _questionsOfPractice = _unitOfWork.Questions.FindAll(m => m.PracticeCode == practiceCode);

            _unitOfWork.Practices.Delete(_practice);
            _unitOfWork.Questions.DeleteRange(_questionsOfPractice);

            string _noteCateg = _unitOfWork.PracticeTypes.Find(m => m.TypeId == dto.Practice.PracticeTypeId)?.TypeName ?? "Unknown";

            int _newPracticeCode = (int)_unitOfWork.Practices.Max(m => m.PracticeCode) + 1;

            #region Add Practice
            Practice practice = new Practice()
            {
                PracticeCode = _newPracticeCode,
                PracticeTypeId = dto.Practice.PracticeTypeId,
                PracticeTitle = dto.Practice.PracticeTitle,
                TeacherId = _userId,
                BookId = dto.Practice.BookId,
                IsShared = dto.Practice.IsShared,
                DurationByMin = dto.Practice.DurationByMin,
                TranDt = _practice.TranDt
            };

            _unitOfWork.Practices.Add(practice);
            #endregion

            #region Add Questions
            List<Question> _questions = new List<Question>();

            foreach (QuestionDto q in dto.Questions)
            {
                int _countQuestionFiles = q.QuestionFiles?.Count ?? 0;
                int _countAnswerFiles = q.AnswerFiles?.Count ?? 0;

                string questionFilesName = null, answerFilesNames = null;

                if (_countQuestionFiles > 0 && _countAnswerFiles > 0)
                {
                    questionFilesName = _appService.Uploade(q.QuestionFiles, "Files");
                    answerFilesNames = _appService.Uploade(q.AnswerFiles, "Files");
                }

                _questions.Add(new Question()
                {
                    TeacherId = _userId,
                    PracticeCode = _newPracticeCode,
                    BookId = dto.Practice.BookId,
                    QuestionTypeId = q.QuestionTypeId,
                    OrderNo = q.OrderNo,
                    ParentQuestionTitle = q.ParentQuestionTitle,
                    IsManyChoicesAnswer = q.IsManyChoicesAnswer,
                    DurationByMin = q.DurationByMin,
                    QuestionTitle = q.QuestionTitle,
                    QuestionChoices = q.QuestionChoices,
                    ModelAnswer = q.ModelAnswer,
                    Score = q.Score,
                    QuestionMedia = questionFilesName,
                    AnswerMedia = answerFilesNames,
                });
            }

            if (_questions.Any())
            {
                await _unitOfWork.Questions.AddRangeAsync(_questions);
            }
            #endregion

             _unitOfWork.Save();

            return Ok("Success");
        }

        [HttpPost("UpdatePracticeHeader")]
        public IActionResult UpdatePracticeHeader(int practiceCode, PracticeDto dto)
        {
            var _practice = _unitOfWork.Practices.Find(m => m.PracticeCode == practiceCode);
            _practice.PracticeTitle = dto.PracticeTitle;
            _practice.BookId = dto.BookId;
            _practice.DurationByMin = dto.DurationByMin;
            _practice.PracticeTypeId = dto.PracticeTypeId;
            _practice.IsShared = dto.IsShared;
            _unitOfWork.Practices.Update(_practice);
            _unitOfWork.Save();
            return Ok(_practice);
        }
        [Authorize(Roles = "1,2,3,4")]
        [HttpPost("Suspend")]
        public IActionResult Suspend(int practiceCode)
        {
            var _practice = _unitOfWork.Practices.Find(m => m.PracticeCode == practiceCode);
            _practice.IsSuspend = true;
            _unitOfWork.Practices.Update(_practice);
            _unitOfWork.Save();
            return Ok(true);
        }
        [HttpPost("UnSuspend")]
        [Authorize(Roles = "1,2,3,4")]
        public IActionResult UnSuspend(int practiceCode)
        {
            var _practice = _unitOfWork.Practices.Find(m => m.PracticeCode == practiceCode);
            _practice.IsSuspend = false;
            _unitOfWork.Practices.Update(_practice);
            _unitOfWork.Save();
            return Ok(true);
        }
        [HttpPost("GetSuspend")]
        [Authorize(Roles = "1,2,3,4")]
        public IActionResult GetSuspend(int practiceCode)
        {
            var _practice = _unitOfWork.Practices.Find(m => m.PracticeCode == practiceCode);

            return Ok(_practice.IsSuspend);
        }
        private bool IsTitleExist(string title, int practiceCode, int? teacherId)
        {
            bool b = _unitOfWork.Practices.IsExist(m => m.PracticeTitle == title && m.PracticeCode != practiceCode && m.TeacherId == teacherId);
            return b;
        }
    }
}
