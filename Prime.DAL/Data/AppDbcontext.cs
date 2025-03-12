using Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Runtime.CompilerServices;
using HelperModels;
using System.Data;

namespace DAL;
public class AppDbcontext : DbContext
{
    public AppDbcontext(DbContextOptions<AppDbcontext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<PagesV>()
            .ToView("PagesV");
        modelBuilder.Entity<ClassStudentsV>()
           .ToView("ClassStudentsV");
        modelBuilder.Entity<AdminTeachersBooksV>()
                  .ToView("AdminTeachersBooksV");
        modelBuilder.Entity<AdminTeachersV>()
                 .ToView("AdminTeachersV");
        modelBuilder.Entity<ClassStudentsV>()
            .HasKey(e => new { e.UserId, e.ClassCode });
        modelBuilder.Entity<AdminTeachersBooksV>()
            .HasKey(e => new { e.TeacherCode });
        
        modelBuilder.Entity<AdminBooksV>()
                .ToView("AdminBooksV");
        modelBuilder.Entity<UsersV>()
                .ToView("UsersV");
        modelBuilder.Entity<LogV>()
                .ToView("LogV");
        modelBuilder.Entity<StudentsClassessBooksV>()
              .ToView("StudentsClassessBooksV");
        modelBuilder.Entity<TeachersClassessBooksV>()
              .ToView("TeachersClassessBooksV");
        modelBuilder.Entity<StudentsResultV>()
             .ToView("StudentsResultV");

    }
    public DbSet<Practice> Practices { get; set; }
    public DbSet<PracticeQuestionsStudent> PracticeQuestionsStudents { get; set; }

  
    public DbSet<PracticesAssignStudent> PracticesAssignStudents { get; set; }

    public DbSet<Log> Logs { get; set; }

    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<TbTimeZone> TbTimeZons { get; set; }

    public DbSet<Book> Books { get; set; }
    public DbSet<BookTopic> BookTopics { get; set; }
    public DbSet<BookPrint> BookPrints { get; set; }
    public DbSet<CodePatch> CodePatchs { get; set; }
    public DbSet<BookGameStudent> BookGameStudents { get; set; }

    public DbSet<Question> Questions { get; set; }
    public DbSet<QuestionType> QuestionTypes { get; set; }


    public DbSet<PracticeType> PracticeTypes { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Attendance> Attendances { get; set; }

    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public DbSet<AppSetting> AppSettings { get; set; }
    public DbSet<SysPage> SysPages { get; set; }

    public DbSet<UpdateLevel> UpdateLevels { get; set; }
    public DbSet<TeacherCode> TeacherCodes { get; set; }
   
    public DbSet<TeacherClass> TeacherClasss { get; set; }
    public DbSet<UserBook> UserBooks { get; set; }
    public DbSet<StudentClass> StudentClasss { get; set; }
    public DbSet<BookService> BookServices { get; set; }
    public DbSet<BookLink> BookLinks { get; set; }
    public DbSet<PlatformOs> PlatformOss { get; set; }


    public DbSet<RolePage> RolePages { get; set; }
    //views
    public DbSet<PagesV> PagesV { get; set; }
    public DbSet<ClassStudentsV> ClassStudentsV { get; set; }
    public DbSet<AdminTeachersBooksV> AdminTeachersBooksV { get; set; }
    public DbSet<AdminTeachersV> AdminTeachersV { get; set; }
    public DbSet<UsersV> UsersV { get; set; }

    public DbSet<LogV> LogV { get; set; }
    public DbSet<StudentsResultV> StudentsResultV { get; set; }
    
    public DbSet<StudentsClassessBooksV> StudentsClassessBooksV { get; set; }
    public DbSet<TeachersClassessBooksV> TeachersClassessBooksV { get; set; }
    public DbSet<AdminBooksV> AdminBooksV { get; set; }
    

    public bool CopyPractice(int practiceCode)
    {
        try
        {
            var practiceCodeParam = new SqlParameter("@PracticeCode", SqlDbType.Int) { Value = practiceCode };

            // Execute the stored procedure
            Database.ExecuteSqlRaw("EXEC CopyPractice @PracticeCode", practiceCodeParam);
            return true;
        }
        catch
        {
            return false;
        }
    }
    public DateTime GetServerDate()
    {
        string s = "exec dbo.GetServerDate";
        var fs = FormattableStringFactory.Create(s);
        var dateQuery = Database.SqlQuery<DateTime>(fs);
        DateTime serverDate = dateQuery.AsEnumerable().First();
        return serverDate;
    }

}