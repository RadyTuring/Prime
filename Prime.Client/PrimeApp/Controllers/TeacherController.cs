using ApiCall;
using Entities;
using Microsoft.AspNetCore.Mvc;

namespace Prime.Client.Controllers
{
    public class TeacherController : BaseController
    {
        public TeacherController(IAppService api) : base(api)
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        //

        public IActionResult GetClasses(int id)
        {
            var res = _api.Get<TeacherClass>("Teacher/GetClassesofTeacher?id=" + id);
            return res != null ? View(res) : Problem("classes are null.");
        }
        public IActionResult Getstudents(int id  )
        {
            var res = _api.Get<ClassStudentsV>("Teacher/GetStudentsOfClass?classId=" + id);
            return res != null ? View(res) : Problem("classes are null.");
        }
        
        public IActionResult GetStudentAttendance(int id)
        {
            var res = _api.Get<Attendance>("Student/GetStudentAttendance?_userId=" + id);
            return res != null ? View(res) : Problem("classes are null.");
        }
        public IActionResult GetStudentResult(int id)
        {
           
            var res = _api.Get<PracticesAssignStudent>("Teacher/GetStudentResult?_userId=" + id);
            return res != null ? View(res) : Problem("classes are null.");
        }
    }
}
