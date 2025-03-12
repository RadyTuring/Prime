#region Controller
using BaseApi;
using DAL;
using Dto;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;
using Token;

namespace Prime.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "2")]
public class TeacherController : ApiBaseController
{
    public TeacherController(IUnitOfWork unitOfWork, IAppService appService) : base(unitOfWork, appService)
    {
    }
    #endregion

    #region AssignPractice


    [HttpPost("AssignPracticeToStudents")]
    public async Task<IActionResult> AssignPracticeToStudents(PracticesAssignStudentsDto dto)
    {
        if (dto.StudentsId.Count() == 0)
        {
            return BadRequest("There are no students.");
        }
        var _userId = int.Parse(User.Identity.Name);
        var res = await assignPractice(_userId, dto.PracticeCode, dto.IsAutoCheckAnswer, dto.IsEditable, dto.DurationByMin, dto.DateFrom, dto.DateTo, null, dto.StudentsId);
        if (res != "ok")
            return BadRequest(res);
        return Ok(true);
    }
    [HttpPost("AssignPracticeToClass")]
    public async Task<IActionResult> AssignPracticeToClass(PracticesAssignClassDto dto)
    {
        var _userId = int.Parse(User.Identity.Name);
        var res = await assignPractice(_userId, dto.PracticeCode, dto.IsAutoCheckAnswer, dto.IsEditable, dto.DurationByMin, dto.DateFrom, dto.DateTo, dto.ClassCode);
        if (res != "ok")
            return BadRequest(res);
        return Ok(true);
    }
    private async Task<string> assignPractice(int _teacherId, int _practiceCode, bool _isAutoCheckAnswer, bool _isEditable, int _durationByMin, DateTime _validDateFrom, DateTime _validDateTo, string _classCode = null, List<int> _students = null, bool isOverWrite = true)
    {
        string msg = "";

        List<int> students = new List<int>();
        var _practice = await _unitOfWork.Practices.FindAsync(m => m.PracticeCode == _practiceCode);

        if (_classCode != null)
        {
            var _class = await _unitOfWork.TeacherClasss.FindAsync(m => m.ClassCode == _classCode);
            if (_practice.BookId != _class.BookId)
            {
                msg = "This class belongs to another book.";
            }
            var res = await GetClass_Students(_classCode);
            if (res == null)
            {
                msg = "There are no students.";
            }
            students = res.Select(m => m.UserId).ToList();
        }
        else
        {
            students = _students;
        }
        if (msg != "")
            return msg;
        List<PracticeQuestionsStudent> _practiceQuestionsStudents = new List<PracticeQuestionsStudent>();
        List<PracticesAssignStudent> _practicesAssignStudents = new List<PracticesAssignStudent>();
        List<Notification> notes = new List<Notification>();
        if (isOverWrite)
        {
            var _list1 = await _unitOfWork.PracticesAssignStudents.FindAllAsync(m => students.Contains(m.StudentId) && m.PracticeCode == _practiceCode && m.TeacherId == _teacherId);
            var _list2 = await _unitOfWork.PracticeQuestionsStudents.FindAllAsync(m => students.Contains(m.StudentId) && m.PracticeCode == _practiceCode && m.TeacherId == _teacherId);
            _unitOfWork.PracticesAssignStudents.DeleteRange(_list1);
            _unitOfWork.PracticeQuestionsStudents.DeleteRange(_list2);
        }
        var _practiceType = await _unitOfWork.PracticeTypes.FindAsync(m => m.TypeId == _practice.PracticeTypeId);




        var _questions = await _unitOfWork.Questions.FindAllAsync(m => m.PracticeCode == _practiceCode);
        _practice.IsAssigned = true;
        _unitOfWork.Practices.Update(_practice);
        long _assignId = _unitOfWork.PracticesAssignStudents.Max(m => m.PracticesAssignId) + 1;
        foreach (var _st in students)
        {
            PracticesAssignStudent practicesAssignStudent = new PracticesAssignStudent()
            {
                PracticesAssignId = _assignId,
                TeacherId = _teacherId,
                StudentId = _st,
                BookId = _practice.BookId,
                PracticeCode = _practiceCode,
                ValidDateFrom = _appService.GetDTM(_validDateFrom, _teacherId),
                ValidDateTo = _appService.GetDTM(_validDateTo, _teacherId),
                DurationByMin = _durationByMin,
                IsAutoCheckAnswer = _isAutoCheckAnswer,
                PracticeTypeId = _practice.PracticeTypeId,
                IsEditable = _isEditable

            };
            _practicesAssignStudents.Add(practicesAssignStudent);
            #region SendNotifications
            Notification note = new Notification()
            {
                Category = _practiceType.TypeName,
                Message = $"{_practice.PracticeTitle} from {_validDateFrom} to {_validDateTo}",
                NoteDate = DateTime.Now,
                FromUserId = _teacherId,
                ToUserId = _st,
                DocNo = _assignId

            };
            notes.Add(note);

            #endregion

            foreach (var _question in _questions)
            {
                PracticeQuestionsStudent practiceQuestionsStudent = new PracticeQuestionsStudent()
                {
                    TeacherId = _teacherId,
                    StudentId = _st,
                    PracticeCode = _practiceCode,
                    PracticesAssignId = _assignId,
                    QuestionId = _question.QuestionId,
                    ModelAnswer = _question.ModelAnswer,
                    Score = _question.Score,
                    AnswerMedia = _question.AnswerMedia
                };
                _practiceQuestionsStudents.Add(practiceQuestionsStudent);
            }

        }
        await _unitOfWork.Notifications.AddRangeAsync(notes);
        await _unitOfWork.PracticeQuestionsStudents.AddRangeAsync(_practiceQuestionsStudents);
        await _unitOfWork.PracticesAssignStudents.AddRangeAsync(_practicesAssignStudents);
        _unitOfWork.Save();
        return "ok";
    }

    #endregion

    #region UnAssignPractice
    [HttpPost("UnAssignPracticeToStudents")]
    public async Task<IActionResult> UnAssignPracticeToStudents(PracticesUnAssignStudentsDto dto)
    {
        if (dto.StudentsId.Count() == 0)
        {
            return BadRequest("There are no students.");
        }
        var _userId = int.Parse(User.Identity.Name);
        var res = await unAssignPractice(_userId, dto.PracticeCode, _students: dto.StudentsId);
        if (res != "ok")
            return BadRequest(res);
        return Ok(true);
    }
    [HttpPost("UnAssignPracticeToClass")]
    public async Task<IActionResult> UnAssignPracticeToClass(PracticesUnAssignClassDto dto)
    {
        var _userId = int.Parse(User.Identity.Name);
        var res = await unAssignPractice(_userId, dto.PracticeCode, _classCode: dto.ClassCode);
        if (res != "ok")
            return BadRequest(res);
        return Ok(true);
    }

    private async Task<string> unAssignPractice(int _teacherId, int _practiceCode, string _classCode = null, List<int> _students = null)
    {
        string msg = "";

        List<int> students = new List<int>();
        var _practice = await _unitOfWork.Practices.FindAsync(m => m.PracticeCode == _practiceCode);

        if (_classCode != null)
        {
            var _class = await _unitOfWork.TeacherClasss.FindAsync(m => m.ClassCode == _classCode);
            if (_practice.BookId != _class.BookId)
            {
                msg = "This class belongs to another book.";
            }
            var res = await GetClass_Students(_classCode);
            if (res == null)
            {
                msg = "There are no students.";
            }
            students = res.Select(m => m.UserId).ToList();
        }
        else
        {
            students = _students;
        }
        if (msg != "")
            return msg;

        List<PracticeQuestionsStudent> _practiceQuestionsStudents = new List<PracticeQuestionsStudent>();
        List<PracticesAssignStudent> _practicesAssignStudents = new List<PracticesAssignStudent>();
        List<Notification> notes = new List<Notification>();
        var _isAssignedBefore = _unitOfWork.PracticesAssignStudents.IsExist(m => m.PracticeCode == _practiceCode && !students.Contains(m.StudentId));
        if (!_isAssignedBefore)
        {
            _practice.IsAssigned = false;
            _unitOfWork.Practices.Update(_practice);
        }

        var _practicesAssignStudent = await _unitOfWork.PracticesAssignStudents.FindAsync(m => students.Contains(m.StudentId) && m.PracticeCode == _practiceCode && m.TeacherId == _teacherId);
        if (_practicesAssignStudent == null)
            return "there is no data ";
        var _assignId = _practicesAssignStudent.PracticesAssignId;
        await _unitOfWork.PracticesAssignStudents.DeleteRangeAsync(m => m.PracticesAssignId == _assignId && students.Contains(m.StudentId));
        await _unitOfWork.PracticeQuestionsStudents.DeleteRangeAsync(m => m.PracticesAssignId == _assignId && students.Contains(m.StudentId));
        await _unitOfWork.Notifications.DeleteRangeAsync(m => m.DocNo == _assignId && students.Contains(m.ToUserId));
        _unitOfWork.Save();
        return "ok";
    }


    #endregion

    #region Class_Operations
    [HttpGet("GetClasses")]
    public async Task<IActionResult> GetClasses()
    {
        var _userId = int.Parse(User.Identity.Name);
        var res = await _unitOfWork.TeacherClasss.FindAllAsync(m => m.TeacherId == _userId);
        return Ok(res);
    }
    [HttpGet("GetClassesofTeacher")]
    public async Task<IActionResult> GetClassesofTeacher(int id)
    {
        var res = await _unitOfWork.TeacherClasss.FindAllAsync(m => m.TeacherId == id, new string[] { "Book" });
        return Ok(res);
    }
    [HttpGet("GetClassStudents")]
    public async Task<IActionResult> GetClassStudents(string classCode)
    {
        return Ok(await GetClass_Students(classCode));
    }

    [HttpGet("GetStudentsOfClass")]
    public async Task<IActionResult> GetStudentsOfClass(int classId)
    {
        var res = await _unitOfWork.ClassStudentsV.FindAllAsync(m => m.Id == classId);
        return Ok(res);
    }
    [HttpGet("GetClassStudentsIds")]
    public async Task<IActionResult> GetClassStudentsIds(string classCode)
    {
        var res = await GetClass_Students(classCode);
        return Ok(res.Select(m => new { StudentId = m.UserId }));
    }
    [HttpGet("GetPracticeStudentAssigned")]
    public async Task<IActionResult> GetPracticeStudentAssigned(string classCode, int practiceCode)
    {
        var res = await GetClass_Students(classCode);
        List<int> studentsIds = res.Select(m => m.UserId).ToList();
        var studentsAssigned = await _unitOfWork.PracticesAssignStudents.FindAllAsync(m => studentsIds.Contains(m.StudentId) && m.PracticeCode == practiceCode);

        // Extract the IDs of assigned students
        List<int> assignedIds = studentsAssigned.Select(m => m.StudentId).ToList();

        // Create the new list with assignment status
        var result = res.Select(m => new { StudentId = m.UserId, StudentUserName = m.UserName, StudentFullName = m.FullName, IsAssigned = assignedIds.Contains(m.UserId) }).ToList();
        return Ok(result);
    }
    [HttpGet("GetPracticeClassAssigned")]
    public async Task<IActionResult> GetPracticeClassAssigned(int practiceCode)
    {
        var _userId = int.Parse(User.Identity.Name);
        var studentsAssigned = await _unitOfWork.PracticesAssignStudents.FindAllAsync(m => m.TeacherId == _userId && m.PracticeCode == practiceCode);

        // Extract the IDs of assigned students
        List<int> assignedIds = studentsAssigned.Select(m => m.StudentId).ToList();

        // Create the new list with assignment status
        var result = await _unitOfWork.StudentClasss.FindAllAsync(m => assignedIds.Contains(m.StudentId));
        List<string> distinctClassCodes = result.Select(m => m.ClassCode).Distinct().ToList();

        // for more Info return classes
        var classes = await _unitOfWork.TeacherClasss.FindAllAsync(m => distinctClassCodes.Contains(m.ClassCode));
        return Ok(classes);
    }

    [HttpPut("UpdateStudentClass")]
    public async Task<IActionResult> UpdateStudentClass(string oldClassCode, int studentId, string newClassCode)
    {
        var _studentClasss = await _unitOfWork.StudentClasss.FindAsync(m => m.ClassCode == oldClassCode && m.StudentId == studentId);
        if (_studentClasss == null)
        {
            return NotFound("Wrong data.");
        }
        _studentClasss.ClassCode = newClassCode;
        var res = _unitOfWork.StudentClasss.Update(_studentClasss);
        return Ok(res);
    }
    [HttpPut("RemoveStudentClass")]
    public async Task<IActionResult> RemoveStudentClass(string _classCode, int _studentId)
    {
        var _studentClass = await _unitOfWork.StudentClasss.FindAsync(m => m.ClassCode == _classCode && m.StudentId == _studentId);
        _unitOfWork.StudentClasss.Delete(_studentClass);
        _unitOfWork.Save();
        return Ok(true);
    }
    [HttpPut("RemoveAllStudentsFromClass")]
    public async Task<IActionResult> RemoveAllStudentsFromClass(string _classCode)
    {
        var _studentsClass = await _unitOfWork.StudentClasss.FindAllAsync(m => m.ClassCode == _classCode);
        _unitOfWork.StudentClasss.DeleteRange(_studentsClass);
        _unitOfWork.Save();
        return Ok(true);
    }
    [HttpPut("RemoveClass")]
    public async Task<IActionResult> RemoveClass(string _classCode)
    {
        var _studentsClass = await _unitOfWork.StudentClasss.FindAllAsync(m => m.ClassCode == _classCode);
        _unitOfWork.StudentClasss.DeleteRange(_studentsClass);
        var _class = await _unitOfWork.TeacherClasss.FindAsync(m => m.ClassCode == _classCode);
        _unitOfWork.TeacherClasss.Delete(_class);
        _unitOfWork.Save();
        return Ok(true);
    }
    [HttpGet("IsClassAssigned")]
    public IActionResult IsClassAssigned(string classCode)
    {
        var _isStudentAssigned = _unitOfWork.StudentClasss.IsExist(m => m.ClassCode == classCode);
        return Ok(_isStudentAssigned);
    }
    [HttpPost("CreateClass")]
    public IActionResult CreateClass(TeacherClassDto dto)
    {
        var _userId = int.Parse(User.Identity.Name);
        var b = _unitOfWork.TeacherClasss.IsExist(m => m.ClassName == dto.ClassName && m.TeacherId == _userId);
        if (b)
        {
            return BadRequest("Class name already exists.");
        }
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
        if (flag)
        {
            return BadRequest("Error: try again.");
        }
        TeacherClass _class = new TeacherClass()
        {
            ClassCode = _code,
            ClassName = dto.ClassName,
            TeacherId = _userId,
            BookId = dto.BookId
        };
        _unitOfWork.TeacherClasss.Add(_class);
        _unitOfWork.Save();
        return Ok(_class);
    }
    [HttpPut("UpdateClass")]
    public async Task<IActionResult> UpdateClass(string classCode, TeacherClassDto dto)
    {
        var _teacherClass = await _unitOfWork.TeacherClasss.FindAsync(m => m.ClassCode == classCode);
        var _isStudentAssigned = _unitOfWork.StudentClasss.IsExist(m => m.ClassCode == classCode);


        if (_isStudentAssigned)
        {
            if (_teacherClass.BookId != dto.BookId)
            {
                return BadRequest("You can't change the book. The class is already assigned to student(s).");
            }
        }
        _teacherClass.ClassName = dto.ClassName;
        _teacherClass.BookId = dto.BookId;
        _unitOfWork.TeacherClasss.Update(_teacherClass);
        _unitOfWork.Save();
        return Ok(_teacherClass);
    }
    #endregion

    #region Reset_Password
    [HttpPut("ResetStudentPassword")]
    public async Task<IActionResult> ResetStudentPassword(int studentId, string newPassword)
    {
        var _student = await _unitOfWork.Users.FindAsync(m => m.UserId == studentId);
        _student.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
        _unitOfWork.Users.Update(_student);
        _unitOfWork.Save();
        return Ok(true);
    }
    #endregion

    #region Practice_Questions
    [HttpGet("GetTeacherPractices")]
    public async Task<IActionResult> GetTeacherPractices(int practiceTypeId)
    {
        var _userId = int.Parse(User.Identity.Name);
        var _teacherPractices = await GetTeacherPracticesAsync(_userId, practiceTypeId);
        return Ok(_teacherPractices);
    }

    [HttpGet("GetTeacherPracticesByBookId")]
    public async Task<IActionResult> GetTeacherPracticesByBookId(int practiceTypeId, int bookId)
    {
        var _userId = int.Parse(User.Identity.Name);
        var res = await _unitOfWork.Practices.FindAllAsync(m => m.TeacherId == _userId && m.PracticeTypeId == practiceTypeId && m.BookId == bookId);
        return Ok(res);
    }
    [HttpGet("GetTeacherPracticesSharedOnly")]
    public async Task<IActionResult> GetTeacherPracticesSharedOnly(int practiceTypeId)
    {
        var _userId = int.Parse(User.Identity.Name);
        var res = GetSharedPracticesForTeacherWithAdminAsync(_userId, practiceTypeId);
        return Ok(res);
    }
    [HttpGet("GetTeacherPracticesSharedOnlyByBookId")]
    public async Task<IActionResult> GetTeacherPracticesSharedOnlyByBookId(int practiceTypeId,int bookId)
    {
        var _userId = int.Parse(User.Identity.Name);
        var res = GetSharedPracticesForTeacherWithAdminByBookidAsync(_userId, practiceTypeId,bookId);

        return Ok(res);
    }
    [HttpPut("DeletePractice")]
    public IActionResult DeletePractice(int practiceCode)
    {
        var _userId = int.Parse(User.Identity.Name);
        var _practice = _unitOfWork.Practices.Find(m => m.PracticeCode == practiceCode && m.TeacherId == _userId);
        if (_practice == null)
        {
            return BadRequest("You are not the author of this practice.");
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
    [HttpPost("CreatePracticeWithGlobalFlag")]
    public async Task<IActionResult> CreatePracticeWithGlobalFlag(PracticesWithGlobalDto dto)
    {
        var _userId = int.Parse(User.Identity.Name);

        if (IsTitleExist(dto.Practice.PracticeTitle, dto.Practice.BookId))
        {
            return BadRequest("The title of this practice already exists.");
        }
        string _noteCateg = _unitOfWork.PracticeTypes.Find(m => m.TypeId == dto.Practice.PracticeTypeId).TypeName;
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
            IsGlobal=dto.Practice.IsGlobal,
            DurationByMin = dto.Practice.DurationByMin,
        };
        _unitOfWork.Practices.Add(practice);
        #endregion
        #region AddQuestions
        List<Question> _questions = new List<Question>();
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

            //if (_countQuestionFiles != _countAnswerFiles)
            //{
            //    return BadRequest("Differenct numebr of Question and Answer files");
            //}
            if (_countQuestionFiles > 0 && _countAnswerFiles > 0)
            {
                questionFilesName = _appService.Uploade(q.QuestionFiles, "Files");
                answerFilesNames = _appService.Uploade(q.AnswerFiles, "Files");
            }
            Question question = new Question()
            {
                TeacherId = _userId,
                PracticeCode = _newPracticeCode,
                BookId = dto.Practice.BookId,
                QuestionTypeId = q.QuestionTypeId,
                // Dificulty = q.Dificulty,
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
            };
            _questions.Add(question);

        }
        #endregion
        await _unitOfWork.Questions.AddRangeAsync(_questions);
        _unitOfWork.Save();
        return Ok("Success");
    }
    [HttpPost("CreatePractice")]
    public async Task<IActionResult> CreatePractice(PracticesDto dto)
    {
        var _userId = int.Parse(User.Identity.Name);

        if (IsTitleExist(dto.Practice.PracticeTitle, dto.Practice.BookId))
        {
            return BadRequest("The title of this practice already exists.");
        }
        string _noteCateg = _unitOfWork.PracticeTypes.Find(m => m.TypeId == dto.Practice.PracticeTypeId).TypeName;
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
        };
        _unitOfWork.Practices.Add(practice);
        #endregion
        #region AddQuestions
        List<Question> _questions = new List<Question>();
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

            //if (_countQuestionFiles != _countAnswerFiles)
            //{
            //    return BadRequest("Differenct numebr of Question and Answer files");
            //}
            if (_countQuestionFiles > 0 && _countAnswerFiles > 0)
            {
                questionFilesName = _appService.Uploade(q.QuestionFiles, "Files");
                answerFilesNames = _appService.Uploade(q.AnswerFiles, "Files");
            }
            Question question = new Question()
            {
                TeacherId = _userId,
                PracticeCode = _newPracticeCode,
                BookId = dto.Practice.BookId,
                QuestionTypeId = q.QuestionTypeId,
                // Dificulty = q.Dificulty,
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
            };
            _questions.Add(question);

        }
        #endregion
        await _unitOfWork.Questions.AddRangeAsync(_questions);
        _unitOfWork.Save();
        return Ok("Success");
    }

    [HttpPost("CopyPractice")]
    public IActionResult CopyPractice(int practiceCode)
    {
        return Ok(_unitOfWork.CopyPractice(practiceCode));
    }



    [HttpGet("GetPracticesGlobal")]
    public async Task<IActionResult> GetPracticesGlobal(int practiceTypeId, int bookId)
    {
        return Ok(await _unitOfWork.Practices.FindAllAsync(m => m.IsGlobal == true && m.BookId == bookId));
    }
    [HttpGet("GetPractices")]
    public async Task<IActionResult> GetPractices(int practiceTypeId)
    {
        var _userId = int.Parse(User.Identity.Name);

        // Fetch teacher-specific practices
        var _teacherPractices = await GetTeacherPracticesAsync(_userId, practiceTypeId);

        // Fetch user books
        var _userBooksIds = await GetUserBookIdsAsync(_userId);

        // Fetch global practices
        var _globalPractices = await GetGlobalPracticesforbookidsAsync(_userBooksIds, practiceTypeId);


        // Merge teacher and global practices
        var mergedPractices = (_teacherPractices ?? Enumerable.Empty<Practice>())
            .Concat(_globalPractices ?? Enumerable.Empty<Practice>())
            .DistinctBy(m => m.Id)  // Assuming `Id` uniquely identifies practices
            .ToList();

        var _teacher = await _unitOfWork.Users.FindAsync(m => m.UserId == _userId);
        if (_teacher?.AdminId == null)
        {
            return Ok(mergedPractices);
        }

        // Fetch teachers under the same admin if any
        var _teachers = await _unitOfWork.Users.FindAllAsync(
            m => m.AdminId == _teacher.AdminId && m.RoleId == 2
        );
        var _teachersIds = _teachers.Select(m => m.UserId).ToList();

        if (!_teachersIds.Any())
        {
            return Ok(mergedPractices);
        }

        // Fetch additional practices from teachers with the same admin
        var _teachersWithSameAdminPractices = await _unitOfWork.Practices.FindAllAsync(
            m => _teachersIds.Contains((int)m.TeacherId) &&
                 _userBooksIds.Contains((int)m.BookId) &&
                 m.PracticeTypeId == practiceTypeId
        );

        // Final merge
        var finalPractices = mergedPractices
            .Concat(_teachersWithSameAdminPractices ?? Enumerable.Empty<Practice>())
            .DistinctBy(m => m.Id)
            .ToList();

        return Ok(finalPractices);
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

    [HttpGet("IsPracticeTitleExist")]
    public IActionResult IsPracticeTitleExist(string title, int bookId)
    {
        bool b = IsTitleExist(title, bookId);
        return Ok(b);
    }
    #endregion

    #region Result_Attend


    [HttpGet("GetAttendance")]
    public async Task<IActionResult> GetAttendance(string classCode)
    {
        var _userId = int.Parse(User.Identity.Name);
        var res = await _unitOfWork.Attendances.FindAllAsync(m => m.ClassCode == classCode);
        return Ok(res);
    }


    [HttpPost("RecordAttend")]
    public IActionResult RecordAttend(AttendRecordDto dto)
    {
        var _userId = int.Parse(User.Identity.Name);
        //var _adminTeacher = _unitOfWork.AdminDets.Find(m => m.TeacherId == _userId);
        //if (_adminTeacher == null)
        //{
        //    return BadRequest("The Teacher is not Assigned to Admin School yet ");
        //}
        foreach (var s in dto.StudentsAttend)
        {
            Attendance attendance = new Attendance()
            {
                TeacherId = _userId,
                //   AdminId = _adminTeacher.SchoolAdminId,
                ClassCode = dto.ClassCode,
                AttendDate = dto.AttendDate,
                Book1 = dto.Book1,
                StudentId = s.StudentId,
                IsAttend = s.IsAttend,
                Note = s.Note
            };
            _unitOfWork.Attendances.Add(attendance);

        }
        _unitOfWork.Save();
        return Ok("done");
    }


    [HttpGet("GetResult")]//Get the Question and answer anc Compare with studetns answers
    public async Task<IActionResult> GetResult(int practiceCode, string classCode)
    {
        var students = await _unitOfWork.StudentClasss.FindAllAsync(m => m.ClassCode == classCode);
        var _studentsIds = students.Select(m => m.StudentId).ToList();
        var _userId = int.Parse(User.Identity.Name);
        var res = await _unitOfWork.StudentsResultV.FindAllAsync(m => m.TeacherId == _userId && _studentsIds.Contains(m.StudentId) && m.PracticeCode == practiceCode);
        return Ok(res);
    }

    #endregion

    #region PrivateMethods
    private bool IsTitleExist(string title, int bookId)
    {
        bool b = _unitOfWork.Practices.IsExist(m => m.PracticeTitle == title && m.BookId == bookId);
        return b;
    }

    private async Task<IEnumerable<ClassStudentsV>> GetClass_Students(string classCodes)
    {
        string _sep = _unitOfWork.AppSettings.GetById(1).Separator1;
        string[] _classes = classCodes.Split(_sep);
        var _classStudents = await _unitOfWork.ClassStudentsV.FindAllAsync(m => _classes.Contains(m.ClassCode));
        return _classStudents;
    }

    private async Task<IEnumerable<Practice>> GetTeacherPracticesAsync(int teacherId, int practiceTypeId)
    {
        var res = await _unitOfWork.Practices.FindAllAsync(m => m.TeacherId == teacherId && m.PracticeTypeId == practiceTypeId);
        return res.OrderBy(m => m.IsGlobal).ThenBy(m => m.BookId).ThenBy(m => m.IsShared).ThenBy(m=> m.TranDt);
    }
    private async Task<List<int>> GetUserBookIdsAsync(int userId)
    {
        var userBooks = await _unitOfWork.UserBooks.FindAllAsync(m => m.UserId == userId);
        return userBooks.Select(m => m.BookId).ToList();
    }

    private async Task<IEnumerable<Practice>> GetGlobalPracticesforbookidsAsync(List<int> userBookIds, int practiceTypeId)
    {
        return await _unitOfWork.Practices.FindAllAsync(
            m => userBookIds.Contains((int)m.BookId) &&
                 m.PracticeTypeId == practiceTypeId &&
                 m.IsGlobal == true
        );
    }
    private async Task<IEnumerable<Practice>> GetSharedPracticesForTeacherWithAdminAsync(int userId, int practiceTypeId)
    {
        var teacher = await _unitOfWork.Users.FindAsync(m => m.UserId == userId);

        if (teacher?.AdminId == null)
        {
            return new List<Practice>();
        }

        var userBookIds = await GetUserBookIdsAsync(userId);

        var teachers = await _unitOfWork.Users.FindAllAsync(m => m.AdminId == teacher.AdminId && m.RoleId == 2);
        if (!teachers.Any())
        {
            return new List<Practice>();
        }

        var teacherIds = teachers.Select(m => m.UserId).ToList();

        var _teachersWithSameAdminPractices = await _unitOfWork.Practices.FindAllAsync(
               m => teacherIds.Contains((int)m.TeacherId) &&
                    userBookIds.Contains((int)m.BookId) &&
                    m.PracticeTypeId == practiceTypeId
           );
        return _teachersWithSameAdminPractices;
    }
    private async Task<IEnumerable<Practice>> GetSharedPracticesForTeacherWithAdminByBookidAsync(int userId, int practiceTypeId, int bookId)
    {
        var teacher = await _unitOfWork.Users.FindAsync(m => m.UserId == userId);

        if (teacher?.AdminId == null)
        {
            return new List<Practice>();
        }

        var userBookIds = await GetUserBookIdsAsync(userId);

        var teachers = await _unitOfWork.Users.FindAllAsync(m => m.AdminId == teacher.AdminId && m.RoleId == 2);
        if (!teachers.Any())
        {
            return new List<Practice>();
        }

        var teacherIds = teachers.Select(m => m.UserId).ToList();

        var _teachersWithSameAdminPractices = await _unitOfWork.Practices.FindAllAsync(
               m => teacherIds.Contains((int)m.TeacherId) &&
                    userBookIds.Contains((int)m.BookId) &&
                    m.PracticeTypeId == practiceTypeId && m.BookId == bookId
           );
        return _teachersWithSameAdminPractices;
    }

    #endregion
}
