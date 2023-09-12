using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ParentApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "skool");

            migrationBuilder.CreateTable(
                name: "classes",
                schema: "skool",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    number = table.Column<int>(type: "integer", nullable: false),
                    letter = table.Column<char>(type: "character(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_classes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "homeworkstatuses",
                schema: "skool",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_homeworkstatuses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "parents",
                schema: "skool",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    patronymic = table.Column<string>(type: "text", nullable: false),
                    surname = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_parents", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "subjects",
                schema: "skool",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    subjectname = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_subjects", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "teachers",
                schema: "skool",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    patronymic = table.Column<string>(type: "text", nullable: false),
                    surname = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_teachers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "students",
                schema: "skool",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    patronymic = table.Column<string>(type: "text", nullable: false),
                    surname = table.Column<string>(type: "text", nullable: false),
                    birthdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    classid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_students", x => x.id);
                    table.ForeignKey(
                        name: "fk_students_classes_classid",
                        column: x => x.classid,
                        principalSchema: "skool",
                        principalTable: "classes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "teachersclassessubjects",
                schema: "skool",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    teacherid = table.Column<int>(type: "integer", nullable: false),
                    classid = table.Column<int>(type: "integer", nullable: false),
                    subjectid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_teachersclassessubjects", x => x.id);
                    table.ForeignKey(
                        name: "fk_teachersclassessubjects_classes_classid",
                        column: x => x.classid,
                        principalSchema: "skool",
                        principalTable: "classes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_teachersclassessubjects_subjects_subjectid",
                        column: x => x.subjectid,
                        principalSchema: "skool",
                        principalTable: "subjects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_teachersclassessubjects_teachers_teacherid",
                        column: x => x.teacherid,
                        principalSchema: "skool",
                        principalTable: "teachers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "parentstudent",
                schema: "skool",
                columns: table => new
                {
                    parentsid = table.Column<int>(type: "integer", nullable: false),
                    studentsid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_parentstudent", x => new { x.parentsid, x.studentsid });
                    table.ForeignKey(
                        name: "fk_parentstudent_parents_parentsid",
                        column: x => x.parentsid,
                        principalSchema: "skool",
                        principalTable: "parents",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_parentstudent_students_studentsid",
                        column: x => x.studentsid,
                        principalSchema: "skool",
                        principalTable: "students",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "lessons",
                schema: "skool",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    teacherclasssubjectid = table.Column<int>(type: "integer", nullable: false),
                    lessondt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    theme = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_lessons", x => x.id);
                    table.ForeignKey(
                        name: "fk_lessons_teachersclassessubjects_teacherclasssubjectid",
                        column: x => x.teacherclasssubjectid,
                        principalSchema: "skool",
                        principalTable: "teachersclassessubjects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "homeworks",
                schema: "skool",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    lessonid = table.Column<int>(type: "integer", nullable: false),
                    startdt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    finishdt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    howework = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_homeworks", x => x.id);
                    table.ForeignKey(
                        name: "fk_homeworks_lessons_lessonid",
                        column: x => x.lessonid,
                        principalSchema: "skool",
                        principalTable: "lessons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "studentsinlessons",
                schema: "skool",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    studentid = table.Column<int>(type: "integer", nullable: false),
                    lessonid = table.Column<int>(type: "integer", nullable: false),
                    comment = table.Column<string>(type: "text", nullable: true),
                    grade = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_studentsinlessons", x => x.id);
                    table.ForeignKey(
                        name: "fk_studentsinlessons_lessons_lessonid",
                        column: x => x.lessonid,
                        principalSchema: "skool",
                        principalTable: "lessons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_studentsinlessons_students_studentid",
                        column: x => x.studentid,
                        principalSchema: "skool",
                        principalTable: "students",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "completedhomeworks",
                schema: "skool",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    studentinlessonid = table.Column<int>(type: "integer", nullable: false),
                    work = table.Column<string>(type: "text", nullable: true),
                    grade = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_completedhomeworks", x => x.id);
                    table.ForeignKey(
                        name: "fk_completedhomeworks_studentsinlessons_studentinlessonid",
                        column: x => x.studentinlessonid,
                        principalSchema: "skool",
                        principalTable: "studentsinlessons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "homeworkprogressstatuses",
                schema: "skool",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    studentinlessonid = table.Column<int>(type: "integer", nullable: false),
                    statussetdt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    homeworkstatusid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_homeworkprogressstatuses", x => x.id);
                    table.ForeignKey(
                        name: "fk_homeworkprogressstatuses_homeworkstatuses_homeworkstatusid",
                        column: x => x.homeworkstatusid,
                        principalSchema: "skool",
                        principalTable: "homeworkstatuses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_homeworkprogressstatuses_studentsinlessons_studentinlessonid",
                        column: x => x.studentinlessonid,
                        principalSchema: "skool",
                        principalTable: "studentsinlessons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "teachersparentsmeetings",
                schema: "skool",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    parentid = table.Column<int>(type: "integer", nullable: false),
                    teacherid = table.Column<int>(type: "integer", nullable: false),
                    studentid = table.Column<int>(type: "integer", nullable: false),
                    meetingdt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    studentinlessonid = table.Column<int>(type: "integer", nullable: true),
                    teacherinitiative = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_teachersparentsmeetings", x => x.id);
                    table.ForeignKey(
                        name: "fk_teachersparentsmeetings_parents_parentid",
                        column: x => x.parentid,
                        principalSchema: "skool",
                        principalTable: "parents",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_teachersparentsmeetings_students_studentid",
                        column: x => x.studentid,
                        principalSchema: "skool",
                        principalTable: "students",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_teachersparentsmeetings_studentsinlessons_studentinlessonid",
                        column: x => x.studentinlessonid,
                        principalSchema: "skool",
                        principalTable: "studentsinlessons",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_teachersparentsmeetings_teachers_teacherid",
                        column: x => x.teacherid,
                        principalSchema: "skool",
                        principalTable: "teachers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_completedhomeworks_studentinlessonid",
                schema: "skool",
                table: "completedhomeworks",
                column: "studentinlessonid");

            migrationBuilder.CreateIndex(
                name: "ix_homeworkprogressstatuses_homeworkstatusid",
                schema: "skool",
                table: "homeworkprogressstatuses",
                column: "homeworkstatusid");

            migrationBuilder.CreateIndex(
                name: "ix_homeworkprogressstatuses_studentinlessonid",
                schema: "skool",
                table: "homeworkprogressstatuses",
                column: "studentinlessonid");

            migrationBuilder.CreateIndex(
                name: "ix_homeworks_lessonid",
                schema: "skool",
                table: "homeworks",
                column: "lessonid");

            migrationBuilder.CreateIndex(
                name: "ix_lessons_teacherclasssubjectid",
                schema: "skool",
                table: "lessons",
                column: "teacherclasssubjectid");

            migrationBuilder.CreateIndex(
                name: "ix_parentstudent_studentsid",
                schema: "skool",
                table: "parentstudent",
                column: "studentsid");

            migrationBuilder.CreateIndex(
                name: "ix_students_classid",
                schema: "skool",
                table: "students",
                column: "classid");

            migrationBuilder.CreateIndex(
                name: "ix_studentsinlessons_lessonid",
                schema: "skool",
                table: "studentsinlessons",
                column: "lessonid");

            migrationBuilder.CreateIndex(
                name: "ix_studentsinlessons_studentid",
                schema: "skool",
                table: "studentsinlessons",
                column: "studentid");

            migrationBuilder.CreateIndex(
                name: "ix_teachersclassessubjects_classid",
                schema: "skool",
                table: "teachersclassessubjects",
                column: "classid");

            migrationBuilder.CreateIndex(
                name: "ix_teachersclassessubjects_subjectid",
                schema: "skool",
                table: "teachersclassessubjects",
                column: "subjectid");

            migrationBuilder.CreateIndex(
                name: "ix_teachersclassessubjects_teacherid",
                schema: "skool",
                table: "teachersclassessubjects",
                column: "teacherid");

            migrationBuilder.CreateIndex(
                name: "ix_teachersparentsmeetings_parentid",
                schema: "skool",
                table: "teachersparentsmeetings",
                column: "parentid");

            migrationBuilder.CreateIndex(
                name: "ix_teachersparentsmeetings_studentid",
                schema: "skool",
                table: "teachersparentsmeetings",
                column: "studentid");

            migrationBuilder.CreateIndex(
                name: "ix_teachersparentsmeetings_studentinlessonid",
                schema: "skool",
                table: "teachersparentsmeetings",
                column: "studentinlessonid");

            migrationBuilder.CreateIndex(
                name: "ix_teachersparentsmeetings_teacherid",
                schema: "skool",
                table: "teachersparentsmeetings",
                column: "teacherid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "completedhomeworks",
                schema: "skool");

            migrationBuilder.DropTable(
                name: "homeworkprogressstatuses",
                schema: "skool");

            migrationBuilder.DropTable(
                name: "homeworks",
                schema: "skool");

            migrationBuilder.DropTable(
                name: "parentstudent",
                schema: "skool");

            migrationBuilder.DropTable(
                name: "teachersparentsmeetings",
                schema: "skool");

            migrationBuilder.DropTable(
                name: "homeworkstatuses",
                schema: "skool");

            migrationBuilder.DropTable(
                name: "parents",
                schema: "skool");

            migrationBuilder.DropTable(
                name: "studentsinlessons",
                schema: "skool");

            migrationBuilder.DropTable(
                name: "lessons",
                schema: "skool");

            migrationBuilder.DropTable(
                name: "students",
                schema: "skool");

            migrationBuilder.DropTable(
                name: "teachersclassessubjects",
                schema: "skool");

            migrationBuilder.DropTable(
                name: "classes",
                schema: "skool");

            migrationBuilder.DropTable(
                name: "subjects",
                schema: "skool");

            migrationBuilder.DropTable(
                name: "teachers",
                schema: "skool");
        }
    }
}
