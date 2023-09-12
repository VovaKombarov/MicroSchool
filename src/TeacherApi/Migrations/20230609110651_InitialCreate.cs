using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TeacherApi.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "skool");

            migrationBuilder.CreateTable(
                name: "Classes",
                schema: "skool",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Letter = table.Column<string>(type: "nvarchar(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HomeworkStatuses",
                schema: "skool",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeworkStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Parents",
                schema: "skool",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Patronymic = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subjects",
                schema: "skool",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubjectName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Teachers",
                schema: "skool",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Patronymic = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                schema: "skool",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Patronymic = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClassId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Students_Classes_ClassId",
                        column: x => x.ClassId,
                        principalSchema: "skool",
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TeachersClassesSubjects",
                schema: "skool",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeacherId = table.Column<int>(type: "int", nullable: true),
                    ClassId = table.Column<int>(type: "int", nullable: true),
                    SubjectId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeachersClassesSubjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeachersClassesSubjects_Classes_ClassId",
                        column: x => x.ClassId,
                        principalSchema: "skool",
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeachersClassesSubjects_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalSchema: "skool",
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeachersClassesSubjects_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalSchema: "skool",
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ParentStudent",
                schema: "skool",
                columns: table => new
                {
                    ParentsId = table.Column<int>(type: "int", nullable: false),
                    StudentsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParentStudent", x => new { x.ParentsId, x.StudentsId });
                    table.ForeignKey(
                        name: "FK_ParentStudent_Parents_ParentsId",
                        column: x => x.ParentsId,
                        principalSchema: "skool",
                        principalTable: "Parents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentStudent_Students_StudentsId",
                        column: x => x.StudentsId,
                        principalSchema: "skool",
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lessons",
                schema: "skool",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeacherClassSubjectId = table.Column<int>(type: "int", nullable: true),
                    LessonDT = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Theme = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lessons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lessons_TeachersClassesSubjects_TeacherClassSubjectId",
                        column: x => x.TeacherClassSubjectId,
                        principalSchema: "skool",
                        principalTable: "TeachersClassesSubjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Homeworks",
                schema: "skool",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LessonId = table.Column<int>(type: "int", nullable: true),
                    StartDT = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FinishDT = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Howework = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Homeworks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Homeworks_Lessons_LessonId",
                        column: x => x.LessonId,
                        principalSchema: "skool",
                        principalTable: "Lessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StudentsInLessons",
                schema: "skool",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: true),
                    LessonId = table.Column<int>(type: "int", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Grade = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentsInLessons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentsInLessons_Lessons_LessonId",
                        column: x => x.LessonId,
                        principalSchema: "skool",
                        principalTable: "Lessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentsInLessons_Students_StudentId",
                        column: x => x.StudentId,
                        principalSchema: "skool",
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CompletedHomeworks",
                schema: "skool",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentInLessonId = table.Column<int>(type: "int", nullable: true),
                    Work = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Grade = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompletedHomeworks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompletedHomeworks_StudentsInLessons_StudentInLessonId",
                        column: x => x.StudentInLessonId,
                        principalSchema: "skool",
                        principalTable: "StudentsInLessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HomeworkProgressStatuses",
                schema: "skool",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentInLessonId = table.Column<int>(type: "int", nullable: true),
                    StatusSetDT = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HomeworkStatusId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeworkProgressStatuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HomeworkProgressStatuses_HomeworkStatuses_HomeworkStatusId",
                        column: x => x.HomeworkStatusId,
                        principalSchema: "skool",
                        principalTable: "HomeworkStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HomeworkProgressStatuses_StudentsInLessons_StudentInLessonId",
                        column: x => x.StudentInLessonId,
                        principalSchema: "skool",
                        principalTable: "StudentsInLessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TeachersParentsMeetings",
                schema: "skool",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    TeacherId = table.Column<int>(type: "int", nullable: true),
                    StudentId = table.Column<int>(type: "int", nullable: true),
                    MeetingDT = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StudentInLessonId = table.Column<int>(type: "int", nullable: true),
                    TeacherInitiative = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeachersParentsMeetings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeachersParentsMeetings_Parents_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "skool",
                        principalTable: "Parents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeachersParentsMeetings_Students_StudentId",
                        column: x => x.StudentId,
                        principalSchema: "skool",
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeachersParentsMeetings_StudentsInLessons_StudentInLessonId",
                        column: x => x.StudentInLessonId,
                        principalSchema: "skool",
                        principalTable: "StudentsInLessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeachersParentsMeetings_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalSchema: "skool",
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompletedHomeworks_StudentInLessonId",
                schema: "skool",
                table: "CompletedHomeworks",
                column: "StudentInLessonId");

            migrationBuilder.CreateIndex(
                name: "IX_HomeworkProgressStatuses_HomeworkStatusId",
                schema: "skool",
                table: "HomeworkProgressStatuses",
                column: "HomeworkStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_HomeworkProgressStatuses_StudentInLessonId",
                schema: "skool",
                table: "HomeworkProgressStatuses",
                column: "StudentInLessonId");

            migrationBuilder.CreateIndex(
                name: "IX_Homeworks_LessonId",
                schema: "skool",
                table: "Homeworks",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_TeacherClassSubjectId",
                schema: "skool",
                table: "Lessons",
                column: "TeacherClassSubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentStudent_StudentsId",
                schema: "skool",
                table: "ParentStudent",
                column: "StudentsId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_ClassId",
                schema: "skool",
                table: "Students",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentsInLessons_LessonId",
                schema: "skool",
                table: "StudentsInLessons",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentsInLessons_StudentId",
                schema: "skool",
                table: "StudentsInLessons",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_TeachersClassesSubjects_ClassId",
                schema: "skool",
                table: "TeachersClassesSubjects",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_TeachersClassesSubjects_SubjectId",
                schema: "skool",
                table: "TeachersClassesSubjects",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_TeachersClassesSubjects_TeacherId",
                schema: "skool",
                table: "TeachersClassesSubjects",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_TeachersParentsMeetings_ParentId",
                schema: "skool",
                table: "TeachersParentsMeetings",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_TeachersParentsMeetings_StudentId",
                schema: "skool",
                table: "TeachersParentsMeetings",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_TeachersParentsMeetings_StudentInLessonId",
                schema: "skool",
                table: "TeachersParentsMeetings",
                column: "StudentInLessonId");

            migrationBuilder.CreateIndex(
                name: "IX_TeachersParentsMeetings_TeacherId",
                schema: "skool",
                table: "TeachersParentsMeetings",
                column: "TeacherId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompletedHomeworks",
                schema: "skool");

            migrationBuilder.DropTable(
                name: "HomeworkProgressStatuses",
                schema: "skool");

            migrationBuilder.DropTable(
                name: "Homeworks",
                schema: "skool");

            migrationBuilder.DropTable(
                name: "ParentStudent",
                schema: "skool");

            migrationBuilder.DropTable(
                name: "TeachersParentsMeetings",
                schema: "skool");

            migrationBuilder.DropTable(
                name: "HomeworkStatuses",
                schema: "skool");

            migrationBuilder.DropTable(
                name: "Parents",
                schema: "skool");

            migrationBuilder.DropTable(
                name: "StudentsInLessons",
                schema: "skool");

            migrationBuilder.DropTable(
                name: "Lessons",
                schema: "skool");

            migrationBuilder.DropTable(
                name: "Students",
                schema: "skool");

            migrationBuilder.DropTable(
                name: "TeachersClassesSubjects",
                schema: "skool");

            migrationBuilder.DropTable(
                name: "Classes",
                schema: "skool");

            migrationBuilder.DropTable(
                name: "Subjects",
                schema: "skool");

            migrationBuilder.DropTable(
                name: "Teachers",
                schema: "skool");
        }
    }
}
