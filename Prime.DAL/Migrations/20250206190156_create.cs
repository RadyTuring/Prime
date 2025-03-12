using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prime.DAL.Migrations
{
    /// <inheritdoc />
    public partial class create : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Separator1 = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Separator2 = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    TimeFactor = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    AdminPrefix = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    TeacherPrefix = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    AppVersion = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Attendances",
                columns: table => new
                {
                    AttendanceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdminId = table.Column<int>(type: "int", nullable: true),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    TeacherId = table.Column<int>(type: "int", nullable: false),
                    Book1 = table.Column<int>(type: "int", nullable: false),
                    ClassCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AttendDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsAttend = table.Column<bool>(type: "bit", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendances", x => x.AttendanceId);
                });

            migrationBuilder.CreateTable(
                name: "BookGameStudents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<decimal>(type: "decimal(5,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookGameStudents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BookLinks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlatformOsId = table.Column<int>(type: "int", nullable: false),
                    BookServiceId = table.Column<int>(type: "int", nullable: false),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    Links = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true),
                    TranDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookLinks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BookPrints",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BkCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    AssignDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    BookServices = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IncludeEkit = table.Column<bool>(type: "bit", nullable: false),
                    PatchNumber = table.Column<int>(type: "int", nullable: true),
                    ValidUpToDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsBlocked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookPrints", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BookServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookServices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BookTopics",
                columns: table => new
                {
                    BookTopicId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    Topic = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TopicDesc = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Keywords = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TopicTypeId = table.Column<int>(type: "int", nullable: true),
                    TopicIdParentId = table.Column<int>(type: "int", nullable: true),
                    TopicOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookTopics", x => x.BookTopicId);
                });

            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    RecId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(60)", nullable: true),
                    TranDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.RecId);
                });

            migrationBuilder.CreateTable(
                name: "PlatformOss",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OsName = table.Column<string>(type: "varchar(60)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformOss", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PracticeQuestionsStudents",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PracticesAssignId = table.Column<long>(type: "bigint", nullable: true),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    TeacherId = table.Column<int>(type: "int", nullable: false),
                    PracticeCode = table.Column<int>(type: "int", nullable: false),
                    BookId = table.Column<int>(type: "int", nullable: true),
                    QuestionId = table.Column<long>(type: "bigint", nullable: false),
                    ModelAnswer = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    AnswerMedia = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Score = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    StudentAnswer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentAnswerMedia = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    StudentScore = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    AcutalStudentDurationByMin = table.Column<int>(type: "int", nullable: true),
                    StartDateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    EndDateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    IsFinished = table.Column<bool>(type: "bit", nullable: true),
                    TranDt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PracticeQuestionsStudents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Practices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PracticeCode = table.Column<int>(type: "int", nullable: false),
                    PracticeTypeId = table.Column<int>(type: "int", nullable: false),
                    TeacherId = table.Column<int>(type: "int", maxLength: 20, nullable: true),
                    BookId = table.Column<int>(type: "int", nullable: true),
                    IsShared = table.Column<bool>(type: "bit", nullable: true),
                    IsGlobal = table.Column<bool>(type: "bit", nullable: false),
                    PracticeTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DurationByMin = table.Column<int>(type: "int", maxLength: 100, nullable: true),
                    TranDt = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsSuspend = table.Column<bool>(type: "bit", nullable: true),
                    IsAssigned = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Practices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PracticesAssignStudents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PracticesAssignId = table.Column<long>(type: "bigint", nullable: true),
                    TeacherId = table.Column<int>(type: "int", nullable: true),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    PracticeCode = table.Column<int>(type: "int", nullable: false),
                    PracticeTypeId = table.Column<int>(type: "int", nullable: false),
                    IsStart = table.Column<bool>(type: "bit", nullable: false),
                    AnswerStartDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    AnswerEndDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    QuestionsCount = table.Column<int>(type: "int", nullable: true),
                    TotalScore = table.Column<decimal>(type: "decimal(8,2)", nullable: true),
                    Score = table.Column<decimal>(type: "decimal(8,2)", nullable: true),
                    IsSubmited = table.Column<bool>(type: "bit", nullable: true),
                    IsEditable = table.Column<bool>(type: "bit", nullable: false),
                    BookId = table.Column<int>(type: "int", nullable: true),
                    IsAutoCheckAnswer = table.Column<bool>(type: "bit", nullable: false),
                    ValidDateFrom = table.Column<DateTime>(type: "datetime", nullable: true),
                    ValidDateTo = table.Column<DateTime>(type: "datetime", nullable: true),
                    DurationByMin = table.Column<int>(type: "int", nullable: false),
                    TranDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PracticesAssignStudents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PracticeTypes",
                columns: table => new
                {
                    TypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PracticeTypes", x => x.TypeId);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    QuestionId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeacherId = table.Column<int>(type: "int", nullable: false),
                    PracticeCode = table.Column<int>(type: "int", nullable: false),
                    BookId = table.Column<int>(type: "int", nullable: true),
                    QuestionTypeId = table.Column<int>(type: "int", nullable: false),
                    ParentQuestionTitle = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    QuestionTitle = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    QuestionDesc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuestionChoices = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    ModelAnswer = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    QuestionMedia = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    AnswerMedia = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Score = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    OrderNo = table.Column<int>(type: "int", nullable: false),
                    IsShared = table.Column<bool>(type: "bit", nullable: false),
                    IsManyChoicesAnswer = table.Column<bool>(type: "bit", nullable: false),
                    DurationByMin = table.Column<int>(type: "int", nullable: false),
                    Dificulty = table.Column<int>(type: "int", nullable: true),
                    TopicId = table.Column<int>(type: "int", nullable: true),
                    Keywords = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TranDt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.QuestionId);
                });

            migrationBuilder.CreateTable(
                name: "QuestionTypes",
                columns: table => new
                {
                    TypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    QuestHeader = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    QuestionDesc = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionTypes", x => x.TypeId);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpiresOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RevokedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RolePages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    PageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "StudentClasss",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    TeacherId = table.Column<int>(type: "int", nullable: false),
                    ClassCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    AssignDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentClasss", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysPages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PgTitle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PgImage = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PgHref = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PgParentId = table.Column<int>(type: "int", maxLength: 50, nullable: true),
                    PgORder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysPages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TbTimeZons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Utcoffset = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeDiff = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TbTimeZons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TeacherCodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    AdminId = table.Column<int>(type: "int", nullable: true),
                    AssignDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    PatchNumber = table.Column<int>(type: "int", nullable: true),
                    ValidUpToDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherCodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UpdateLevels",
                columns: table => new
                {
                    UpdateLevelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UpdateLevelName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UpdateLevels", x => x.UpdateLevelId);
                });

            migrationBuilder.CreateTable(
                name: "UserBooks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    AssignDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBooks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    CountryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountryNameUtc = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    CountryName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Code2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Code3 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TimeDif = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    TbTimeZoneId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.CountryId);
                    table.ForeignKey(
                        name: "FK_Countries_TbTimeZons_TbTimeZoneId",
                        column: x => x.TbTimeZoneId,
                        principalTable: "TbTimeZons",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    BookId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookName = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    PPKFileName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DefaultImage = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: true),
                    GamesFileName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    GamesImage = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: true),
                    Prefix = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    PPkVersion = table.Column<int>(type: "int", nullable: false),
                    UpdateLevelId = table.Column<int>(type: "int", nullable: false),
                    DownloadLink = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                    LinkUrl = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.BookId);
                    table.ForeignKey(
                        name: "FK_Books_UpdateLevels_UpdateLevelId",
                        column: x => x.UpdateLevelId,
                        principalTable: "UpdateLevels",
                        principalColumn: "UpdateLevelId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AdminCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RoleId = table.Column<int>(type: "int", nullable: true),
                    ImageFile = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    IsActiveUser = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    IsOnline = table.Column<bool>(type: "bit", nullable: false),
                    IsNewUser = table.Column<bool>(type: "bit", nullable: false),
                    SchoolName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatchNumber = table.Column<int>(type: "int", nullable: true),
                    AdminId = table.Column<int>(type: "int", nullable: true),
                    TranDt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ThemeId = table.Column<int>(type: "int", nullable: true),
                    Books = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Teachers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Clasess = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BlockLevel = table.Column<int>(type: "int", nullable: true),
                    LogOfflineTimes = table.Column<int>(type: "int", nullable: true),
                    IsRemovedFromSchool = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId");
                });

            migrationBuilder.CreateTable(
                name: "CodePatchs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookId = table.Column<int>(type: "int", nullable: true),
                    PatchDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PatchDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PatchType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberOfCodes = table.Column<int>(type: "int", nullable: false),
                    IsOneAssinged = table.Column<bool>(type: "bit", nullable: true),
                    IsPrinted = table.Column<bool>(type: "bit", nullable: false),
                    InventoryLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodePatchs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CodePatchs_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "BookId");
                });

            migrationBuilder.CreateTable(
                name: "TeacherClasss",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClassName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TeacherId = table.Column<int>(type: "int", nullable: false),
                    BookId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherClasss", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeacherClasss_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FromUserId = table.Column<int>(type: "int", nullable: false),
                    ToUserId = table.Column<int>(type: "int", nullable: false),
                    NoteDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    IsDone = table.Column<bool>(type: "bit", nullable: true),
                    DocNo = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_FromUserId",
                        column: x => x.FromUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_UpdateLevelId",
                table: "Books",
                column: "UpdateLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_CodePatchs_BookId",
                table: "CodePatchs",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_TbTimeZoneId",
                table: "Countries",
                column: "TbTimeZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_FromUserId",
                table: "Notifications",
                column: "FromUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherClasss_BookId",
                table: "TeacherClasss",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CountryId",
                table: "Users",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppSettings");

            migrationBuilder.DropTable(
                name: "Attendances");

            migrationBuilder.DropTable(
                name: "BookGameStudents");

            migrationBuilder.DropTable(
                name: "BookLinks");

            migrationBuilder.DropTable(
                name: "BookPrints");

            migrationBuilder.DropTable(
                name: "BookServices");

            migrationBuilder.DropTable(
                name: "BookTopics");

            migrationBuilder.DropTable(
                name: "CodePatchs");

            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "PlatformOss");

            migrationBuilder.DropTable(
                name: "PracticeQuestionsStudents");

            migrationBuilder.DropTable(
                name: "Practices");

            migrationBuilder.DropTable(
                name: "PracticesAssignStudents");

            migrationBuilder.DropTable(
                name: "PracticeTypes");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "QuestionTypes");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "RolePages");

            migrationBuilder.DropTable(
                name: "StudentClasss");

            migrationBuilder.DropTable(
                name: "SysPages");

            migrationBuilder.DropTable(
                name: "TeacherClasss");

            migrationBuilder.DropTable(
                name: "TeacherCodes");

            migrationBuilder.DropTable(
                name: "UserBooks");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "UpdateLevels");

            migrationBuilder.DropTable(
                name: "TbTimeZons");
        }
    }
}
