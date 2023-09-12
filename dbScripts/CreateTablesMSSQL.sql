USE master;

-- Удаляем базу, если она есть
IF DB_ID('microschool') IS NOT NULL DROP DATABASE microschool;


 --Удаляем схему, если такая есть
IF DB_ID('skool') IS NOT NULL DROP SCHEMA skool; 

IF @@ERROR = 3702 
   RAISERROR('Базу данных нельзя удалить, так как все еще есть открытые соединения.', 127, 127) WITH NOWAIT, LOG;


-- Создаем базу
CREATE DATABASE microschool;
GO

USE microschool;
GO

CREATE SCHEMA skool AUTHORIZATION dbo;
GO

-- Создаем таблицу классов
CREATE TABLE skool.Classes
(
	Id       INT			NOT NULL IDENTITY(1,1),
	Number   INT			NOT NULL,
	Letter   NVARCHAR(1)    NOT NULL,
		CONSTRAINT PK_Classes PRIMARY KEY(Id),
);

CREATE TABLE skool.Students
(
	Id			INT					NOT NULL IDENTITY(1,1),
	Name		NVARCHAR(MAX)		NOT NULL, 
	Patronymic	NVARCHAR(MAX)		NOT NULL,
	Surname     NVARCHAR(max)		NOT NULL,
	BirthDate   DATE				NOT NULL,
	ClassId		INT                 NOT NULL,
		CONSTRAINT PK_Students PRIMARY KEY(Id),
		CONSTRAINT FK_Students_Classes_ClassId 
			FOREIGN KEY (ClassId) 
			REFERENCES skool.Classes(Id),
		CONSTRAINT CHK_BirthDate CHECK(BirthDate <= CURRENT_TIMESTAMP)
)

CREATE TABLE skool.Parents
(
	Id			INT					NOT NULL IDENTITY(1,1),
	Name		NVARCHAR(MAX)		NOT NULL,
	Patronymic	NVARCHAR(MAX)		NOT NULL,
	Surname     NVARCHAR(max)		NOT NULL,
		CONSTRAINT PK_Parents PRIMARY KEY(Id),
)

CREATE TABLE skool.ParentStudent
( 
	ParentsId	INT		NOT NULL,
	StudentsId   INT     NOT NULL,
		CONSTRAINT PK_ParentStudent PRIMARY KEY (ParentsId, StudentsId),
		CONSTRAINT FK_ParentStudent_Parents_ParentsId 
			FOREIGN KEY (ParentsId) 
			REFERENCES skool.Parents(Id),
		CONSTRAINT FK_ParentStudent_Students_StudentsId 
			FOREIGN KEY (StudentsId) 
			REFERENCES skool.Students(Id)	
)

CREATE TABLE skool.Teachers
(
	Id			INT					NOT NULL IDENTITY(1,1),
	Name		NVARCHAR(MAX)		NOT NULL,
	Patronymic	NVARCHAR(MAX)		NOT NULL,
	Surname     NVARCHAR(max)		NOT NULL,
		CONSTRAINT PK_Teachers PRIMARY KEY(Id),
)


CREATE TABLE skool.Subjects
(
	Id				INT					NOT NULL IDENTITY(1,1),
	SubjectName		NVARCHAR(MAX)		NOT NULL,
		CONSTRAINT PK_Subjects PRIMARY KEY(Id),
)


CREATE TABLE skool.TeachersClassesSubjects
( 
    Id			INT		NOT NULL IDENTITY(1,1),
	TeacherId	INT		NOT NULL,
	ClassId		INT     NOT NULL,
	SubjectId  INT     NOT NULL,
		CONSTRAINT PK_TeachersClassesSubjects PRIMARY KEY (Id),
		CONSTRAINT FK_TeachersClassesSubjects_Teachers_TeacherId 
			FOREIGN KEY (TeacherId) 
			REFERENCES skool.Teachers(Id),
		CONSTRAINT FK_TeachersClassesSubjects_Classes_ClassId 
			FOREIGN KEY (ClassId) 
			REFERENCES skool.Classes(Id),
		CONSTRAINT FK_TeachersClassesSubjects_Subjects_SubjectId 
			FOREIGN KEY (SubjectId) 
			REFERENCES skool.Subjects(Id),	
)

CREATE TABLE skool.Lessons
(
	Id							INT					NOT NULL IDENTITY(1,1),
	TeacherClassSubjectId		INT                 NOT NULL,
	LessonDT                    datetime2(7)        NOT NULL,
	Theme						nvarchar(max)       NOT NULL,
		CONSTRAINT PK_Lessons PRIMARY KEY(Id),
		CONSTRAINT FK_Lessons_TeachersClassesSubjects_TeacherClassSubjectId
			FOREIGN KEY (TeacherClassSubjectId) 
			REFERENCES skool.TeachersClassesSubjects(Id)
)

CREATE TABLE skool.Homeworks
(
	Id							INT					NOT NULL IDENTITY(1,1),
	LessonId					INT                 NOT NULL,
	StartDT                     datetime2(7)        NOT NULL,
	FinishDT                    dateTime2(7)        NOT NULL,
	Homework                    nvarchar(max)       NOT NULL,
		CONSTRAINT PK_Homeworks PRIMARY KEY(Id),
		CONSTRAINT FK_Homeworks_Lessons_LessonId 
			FOREIGN KEY (LessonId) 
			REFERENCES skool.Lessons(Id)
)

CREATE TABLE skool.HomeworkStatuses
(
	Id				INT					NOT NULL IDENTITY(1,1),
	Status			NVARCHAR(MAX)		NOT NULL,
		CONSTRAINT PK_HomeworkStatuses PRIMARY KEY(Id),
)

CREATE TABLE skool.StudentsInLessons
(
	Id				INT					NOT NULL IDENTITY(1,1),
	LessonId		INT					NOT NULL,
	StudentId       INT					NOT NULL,
	Grade			INT					NULL,
	Comment			NVARCHAR(MAX)		NOT NULL,
		CONSTRAINT PK_StudentsInLessons PRIMARY KEY(Id),

		CONSTRAINT FK_StudentsInLessons_Lessons_LessonId 
			FOREIGN KEY (LessonId) 
			REFERENCES skool.Lessons(Id),

		CONSTRAINT FK_StudentsInLessons_Student_StudentId 
			FOREIGN KEY (StudentId) 
			REFERENCES skool.Students(Id),
)

CREATE TABLE skool.CompletedHomeworks
(
	Id					INT					NOT NULL IDENTITY(1,1),
	StudentInLessonId	INT					NOT NULL,
	Grade				INT					NULL,
	Work				NVARCHAR(MAX)		NOT NULL,
		CONSTRAINT PK_CompletedHomeworks PRIMARY KEY(Id),

		CONSTRAINT FK_CompletedHomeworks_StudentsInLessons_StudentInLessonId 
			FOREIGN KEY (StudentInLessonId) 
			REFERENCES skool.StudentsInLessons(Id),
)


CREATE TABLE skool.HomeworkProgressStatuses
(
	Id					INT					NOT NULL IDENTITY(1,1),
	HomeworkStatusId    INT					NOT NULL,
	StudentInLessonId	INT					NOT NULL,
	StatusSetDT			datetime2(7)		NOT NULL,
		CONSTRAINT PK_HomeworkProgressStatuses PRIMARY KEY(Id),
		CONSTRAINT FK_HomeworkProgressStatuses_HomeworkStatuses_HomeworkStatusId
			FOREIGN KEY (HomeworkStatusId) 
			REFERENCES skool.StudentsInLessons(Id),
		CONSTRAINT FK_HomeworkProgressStatuses_StudentsInLessons_StudentInLessonId 
			FOREIGN KEY (StudentInLessonId) 
			REFERENCES skool.HomeworkStatuses(Id),
)


CREATE TABLE skool.TeachersParentsMeetings
(
	Id					INT					NOT NULL IDENTITY(1,1),
	TeacherId           INT                 NOT NULL, 
	ParentId            INT                 NOT NULL,
	StudentId			INT                 NOT NULL,
	MeetingDT			datetime2(7)		NOT NULL,
	StudentInLessonId   INT                 NULL, 
	IsTeacherInitiative BIT                 NOT NULL
		CONSTRAINT PK_TeachersParentsMeetings PRIMARY KEY(Id),
		
		CONSTRAINT FK_TeachersParentsMeetings_Teachers_TeacherId 
			FOREIGN KEY (TeacherId) 
			REFERENCES skool.Teachers(Id),

		CONSTRAINT FK_TeachersParentsMeetings_Parents_ParentId 
			FOREIGN KEY (ParentId) 
			REFERENCES skool.Parents(Id),

		CONSTRAINT FK_TeachersParentsMeetings_Students_StudentId 
			FOREIGN KEY (StudentId) 
			REFERENCES skool.Students(Id),

		CONSTRAINT FK_TeachersParentsMeetings_StudentsInLessons_StudentInLessonId 
			FOREIGN KEY (StudentInLessonId) 
			REFERENCES skool.StudentsInLessons(Id)
)