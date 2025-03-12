
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelperModels;
using static System.Reflection.Metadata.BlobBuilder;


namespace Entities
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Practice> Practices { get; }
        IRepository<PracticeQuestionsStudent> PracticeQuestionsStudents { get; }
      
        IRepository<PracticesAssignStudent> PracticesAssignStudents { get; }

        IRepository<Log> Logs { get; }


        IRepository<Attendance> Attendances { get; }

        IRepository<Notification> Notifications { get; }
        IRepository<Country> Countries { get; }
        IRepository<TbTimeZone> TbTimeZones { get; }


        IRepository<Book> Books { get; }
        IRepository<BookTopic> BookTopics { get; }
        IRepository<CodePatch> CodePatchs { get; }
        IRepository<BookPrint> BookPrints { get; }
        IRepository<BookGameStudent> BookGameStudents { get; }

        IRepository<Question> Questions { get; }
        IRepository<QuestionType> QuestionTypes { get; }

        IRepository<PracticeType> PracticeTypes { get; }

        IRepository<User> Users { get; }

        IRepository<Role> Roles { get; }
        IRepository<UserRole> UserRoles { get; }
        IRepository<RefreshToken> RefreshTokens { get; }


        IRepository<AppSetting> AppSettings { get; }
        IRepository<SysPage> SysPages { get; }
        IRepository<RolePage> RolePages { get; }

        IRepository<UpdateLevel> UpdateLevels { get; }
        IRepository<TeacherCode> TeacherCodes { get; }
       
        IRepository<TeacherClass> TeacherClasss { get; }
        IRepository<StudentClass> StudentClasss { get; }
        IRepository<UserBook> UserBooks { get; }
        IRepository<BookService> BookServices { get; }
        IRepository<PlatformOs> PlatformOss { get; }
        IRepository<BookLink> BookLinks { get; }

        int Save();
        DateTime GetServerDate();
        bool CopyPractice(int practiceCode);
        //views

        IRepository<PagesV> PagesV { get; }
        IRepository<ClassStudentsV> ClassStudentsV { get; }
        IRepository<AdminTeachersBooksV> AdminTeachersBooksV { get; }
        IRepository<AdminTeachersV> AdminTeachersV { get; }
        IRepository<StudentsClassessBooksV> StudentsClassessBooksV { get; }
        IRepository<TeachersClassessBooksV> TeachersClassessBooksV { get; }
         
        IRepository<UsersV> UsersV { get; }
        IRepository<LogV> LogV { get; }
        IRepository<StudentsResultV> StudentsResultV { get; }
        IRepository<AdminBooksV> AdminBooksV { get; }
        

    }
}
