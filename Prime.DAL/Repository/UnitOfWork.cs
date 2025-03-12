using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;
using static DAL.AppDbcontext;
using HelperModels;

namespace DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        protected private AppDbcontext _context;
        public UnitOfWork(AppDbcontext context)
        {
            _context = context;
            Practices = new Repository<Practice>(context);
            PracticeQuestionsStudents = new Repository<PracticeQuestionsStudent>(context);
           

            PracticesAssignStudents = new Repository<PracticesAssignStudent>(context);
            Logs = new Repository<Log>(context);
            Notifications = new Repository<Notification>(context);
            Countries = new Repository<Country>(context);
            TbTimeZones = new Repository<TbTimeZone>(context);

            Books = new Repository<Book>(context);
            BookTopics = new Repository<BookTopic>(context);
            BookGameStudents = new Repository<BookGameStudent>(context);
            BookPrints = new Repository<BookPrint>(context);
            CodePatchs = new Repository<CodePatch>(context);

            Questions = new Repository<Question>(context);
            QuestionTypes = new Repository<QuestionType>(context);
            PracticeTypes = new Repository<PracticeType>(context);

            Users = new Repository<User>(context);
            Roles = new Repository<Role>(context);
            UserRoles = new Repository<UserRole>(context);
            Attendances = new Repository<Attendance>(context);
            RefreshTokens = new Repository<RefreshToken>(context);
            AppSettings = new Repository<AppSetting>(context);
            SysPages = new Repository<SysPage>(context);
            RolePages = new Repository<RolePage>(context);
            UpdateLevels = new Repository<UpdateLevel>(context);
            TeacherCodes = new Repository<TeacherCode>(context);
           
            TeacherClasss = new Repository<TeacherClass>(context);
            UserBooks = new Repository<UserBook>(context);
            StudentClasss = new Repository<StudentClass>(context);
            BookServices = new Repository<BookService>(context);
            BookLinks = new Repository<BookLink>(context);
            PlatformOss = new Repository<PlatformOs>(context);

            PagesV = new Repository<PagesV>(context);
            ClassStudentsV = new Repository<ClassStudentsV>(context);
            AdminTeachersBooksV = new Repository<AdminTeachersBooksV>(context);
            AdminTeachersV = new Repository<AdminTeachersV>(context);
            UsersV = new Repository<UsersV>(context);
            LogV = new Repository<LogV>(context);
            StudentsClassessBooksV = new Repository<StudentsClassessBooksV>(context);
            TeachersClassessBooksV = new Repository<TeachersClassessBooksV>(context);
            StudentsResultV = new Repository<StudentsResultV>(context);
            AdminBooksV = new Repository<AdminBooksV>(context);
            

        }
        public IRepository<Practice> Practices { get; }
        public IRepository<PracticeQuestionsStudent> PracticeQuestionsStudents { get; }
       
        public IRepository<PracticesAssignStudent> PracticesAssignStudents { get; }

        public IRepository<Log> Logs { get; }
        public IRepository<Notification> Notifications { get; }
        public IRepository<Country> Countries { get; }
        public IRepository<TbTimeZone> TbTimeZones { get; }

        public IRepository<Book> Books { get; }
        public IRepository<BookTopic> BookTopics { get; }
        public IRepository<BookPrint> BookPrints { get; }
        public IRepository<CodePatch> CodePatchs { get; }

        public IRepository<BookGameStudent> BookGameStudents { get; }

        public IRepository<Question> Questions { get; }
        public IRepository<QuestionType> QuestionTypes { get; }
        public IRepository<PracticeType> PracticeTypes { get; }
        public IRepository<User> Users { get; }

        public IRepository<Role> Roles { get; }
        public IRepository<UserRole> UserRoles { get; }
        public IRepository<RolePage> RolePages { get; }
        public IRepository<AppSetting> AppSettings { get; }
        public IRepository<BookService> BookServices { get; }
        public IRepository<PlatformOs> PlatformOss { get; }
        public IRepository<BookLink> BookLinks { get; }

        public void Dispose()
        {
            _context.Dispose();
        }

        public DateTime GetServerDate()
        {
            return _context.GetServerDate();
        }
        public bool CopyPractice(int practiceCode)
        {
            return _context.CopyPractice(practiceCode);
        }
        public int Save()
        {
            return _context.SaveChanges();
        }
        public IRepository<Attendance> Attendances { get; }
        public IRepository<RefreshToken> RefreshTokens { get; }
        public IRepository<SysPage> SysPages { get; }
        public IRepository<UpdateLevel> UpdateLevels { get; }
        public IRepository<TeacherCode> TeacherCodes { get; }
      
        public IRepository<TeacherClass> TeacherClasss { get; }
        public IRepository<StudentClass> StudentClasss { get; }
        public IRepository<UserBook> UserBooks { get; }
        public IRepository<PagesV> PagesV { get; }
        public IRepository<ClassStudentsV> ClassStudentsV { get; }
        public IRepository<AdminTeachersBooksV> AdminTeachersBooksV { get; }
        public IRepository<StudentsClassessBooksV> StudentsClassessBooksV { get; }
        public IRepository<TeachersClassessBooksV> TeachersClassessBooksV { get; }
        public IRepository<AdminTeachersV> AdminTeachersV { get; }
        public IRepository<UsersV> UsersV { get; }
        public IRepository<LogV> LogV { get; }
        public IRepository<StudentsResultV> StudentsResultV { get; }
        public IRepository<AdminBooksV> AdminBooksV { get; }
        

    }
}
