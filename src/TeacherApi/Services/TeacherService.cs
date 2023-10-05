using Common.Api;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeacherApi.Data;
using TeacherApi.Data.Specifications;
using TeacherApi.Models;
using TeacherApi.Utilities;

namespace TeacherApi.Services
{
    /// <summary>
    /// Сервис учителя.
    /// </summary>
    public class TeacherService : ITeacherService
    {
        #region Fields

        /// <summary>
        /// Источник данных.
        /// </summary>
        private AppDbContext _context;

        /// <summary>
        /// Репозиторий для сущности класса.
        /// </summary>
        private readonly IRepository<Class> _classRepo;

        /// <summary>
        /// Репозиторий для сущности студента.
        /// </summary>
        private readonly IRepository<Student> _studentRepo;

        /// <summary>
        /// Репозиторий для сущности предмета.
        /// </summary>
        private readonly IRepository<Subject> _subjectRepo;

        /// <summary>
        /// Репозиторий для сущности учителя.
        /// </summary>
        private readonly IRepository<Teacher> _teacherRepo;

        /// <summary>
        /// Репозиторий для сущности учителя/класса/предмета.
        /// </summary>
        private readonly IRepository<TeacherClassSubject> _teacherClassSubjectRepo;

        /// <summary>
        /// Репозиторий для сущности урока.
        /// </summary>
        private readonly IRepository<Lesson> _lessonRepo;

        /// <summary>
        /// Репозиторий для сущности родителя.
        /// </summary>
        private readonly IRepository<Parent> _parentRepo;

        /// <summary>
        /// Репозиторий для сущности студента на уроке.
        /// </summary>
        private readonly IRepository<StudentInLesson> _studentInLessonRepo;

        /// <summary>
        /// Репозиторий для сущности домашней работы.
        /// </summary>
        private readonly IRepository<Homework> _homeworkRepo;

        /// <summary>
        /// Репозиторий для сущности прогресса домашней работы.
        /// </summary>
        private readonly IRepository<HomeworkProgressStatus>
            _homeworkProgressStatusRepo;

        /// <summary>
        /// Репозиторий для сущности готовой домашней работы.
        /// </summary>
        private readonly IRepository<CompletedHomework> _completedHomeworkRepo;

        /// <summary>
        /// Репозиторий для сущности статуса домашней работы.
        /// </summary>
        private readonly IRepository<HomeworkStatus> _homeWorkStatusRepo;

        /// <summary>
        /// Репозиторий для сущности встречи родителя и учителя.
        /// </summary>
        private readonly IRepository<TeacherParentMeeting> _teacherParentMeetingRepo;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="context">Контекст.</param>
        /// <param name="classRepo">Репозиторий для сущности класса.</param>
        /// <param name="studentRepo">Репозиторий для сущности студента.</param>
        /// <param name="teacherClassSubjectRepo">Репозиторий для сущности учителя/класса/предмета.</param>
        /// <param name="parentRepo">Репозиторий для сущности родителя.</param>
        /// <param name="lessonRepo">Репозиторий для сущности урока.</param>
        /// <param name="studentInLessonRepo">Репозиторий для сущности студента на уроке.</param>
        /// <param name="homeworkRepo">Репозиторий для сущности домашней работы.</param>
        /// <param name="homeworkProgressStatusRepo">Репозиторий для сущности прогресса домашней работы.</param>
        /// <param name="subjectRepo">Репозиторий для сущности предмета.</param>
        /// <param name="teacherRepo">Репозиторий для сущности учителя.</param>
        /// <param name="completedHomeworkRepo">Репозиторий для сущности готовой домашней работы.</param>
        /// <param name="homeworkStatusRepo">Репозиторий для сущности статуса домашней работы.</param>
        /// <param name="teacherParentMeetingRepo">Репозиторий для сущности встречи родителя и учителя.</param>
        public TeacherService(
            AppDbContext context,
            IRepository<Class> classRepo,
            IRepository<Student> studentRepo,
            IRepository<TeacherClassSubject> teacherClassSubjectRepo,
            IRepository<Parent> parentRepo,
            IRepository<Lesson> lessonRepo,
            IRepository<StudentInLesson> studentInLessonRepo,
            IRepository<Homework> homeworkRepo,
            IRepository<HomeworkProgressStatus> homeworkProgressStatusRepo,
            IRepository<Subject> subjectRepo,
            IRepository<Teacher> teacherRepo,
            IRepository<CompletedHomework> completedHomeworkRepo,
            IRepository<HomeworkStatus> homeworkStatusRepo,
            IRepository<TeacherParentMeeting> teacherParentMeetingRepo)
        {
            _context = context;

            _classRepo = classRepo;
            _studentRepo = studentRepo;
            _teacherClassSubjectRepo = teacherClassSubjectRepo;
            _parentRepo = parentRepo;
            _lessonRepo = lessonRepo;
            _studentInLessonRepo = studentInLessonRepo;
            _homeworkRepo = homeworkRepo;
            _homeworkProgressStatusRepo = homeworkProgressStatusRepo;
            _subjectRepo = subjectRepo;
            _teacherRepo = teacherRepo;
            _completedHomeworkRepo = completedHomeworkRepo;
            _teacherParentMeetingRepo = teacherParentMeetingRepo;
            _homeWorkStatusRepo = homeworkStatusRepo;
        }

        #endregion Constructors

        #region Utilities

        /// <summary>
        /// Создание обьекта урока.
        /// </summary>
        /// <param name="lessonDateTime">Время урока.</param>
        /// <param name="teacherClassSubject">Обьект учитель/класс/предмет </param>
        /// <param name="theme">Тема урока.</param>
        /// <returns>Обьект урока.</returns>
        private Lesson _CreateLesson(
            DateTime lessonDateTime,
            TeacherClassSubject teacherClassSubject,
            string theme)
        {
            return new Lesson()
            {
                LessonDT = lessonDateTime,
                TeacherClassSubject = teacherClassSubject,
                Theme = theme
            };
        }

        /// <summary>
        /// Создание обьекта домашней работы.
        /// </summary>
        /// <param name="lesson">Урок.</param>
        /// <param name="finishDT">Время окончания урока.</param>
        /// <param name="homeWork">Домашняя работа.</param>
        /// <returns>Обьект домащней работы.</returns>
        private Homework _CreateHomework(
            Lesson lesson,
            DateTime finishDT,
            string homeWork)
        {
            return new Homework()
            {
                Lesson = lesson,
                StartDT = lesson.LessonDT,
                FinishDT = finishDT,
                Howework = homeWork
            };  
        }

        /// <summary>
        /// Создание обьекта студент на уроке.
        /// </summary>
        /// <param name="lesson">Урок.</param>
        /// <param name="student">Студент.</param>
        /// <returns>Обьект студента на уроке.</returns>
        private StudentInLesson _CreateStudentInLesson(
            Lesson lesson,
            Student student)
        {
            return new StudentInLesson()
            {
                Lesson = lesson,
                Student = student
            };
        }

        /// <summary>
        /// Создание обьекта готовой домашней работы.
        /// </summary>
        /// <param name="studentInLesson">Студент на уроке.</param>
        /// <returns>Обьект домашней работы.</returns>
        private CompletedHomework _CreateCompletedHomework(
            StudentInLesson studentInLesson)
        {
            return new CompletedHomework()
            {
                StudentInLesson = studentInLesson
            };
        }

        /// <summary>
        /// Создание обьекта встречи учителя и родителя.
        /// </summary>
        /// <param name="student">Студент.</param>
        /// <param name="teacher">Учитель.</param>
        /// <param name="parent">Родитель.</param>
        /// <param name="meetingDT">Время встречи.</param>
        /// <returns>Встреча учителя и родителя.</returns>
        private TeacherParentMeeting _CreateTeacherParentMeeting(
            Student student,
            Teacher teacher,
            Parent parent,
            DateTime meetingDT)
        {
            return new TeacherParentMeeting()
            {
                Student = student,
                Parent = parent,
                Teacher = teacher,
                TeacherInitiative = true,
                MeetingDT = meetingDT
            };
        }

        /// <summary>
        /// Создание обьекта прогресса домашней работы.
        /// </summary>
        /// <param name="studentInLesson">Студент на уроке.</param>
        /// <param name="homeworkStatus">Статус домашней работы.</param>
        /// <returns>Обьект статуса домашней работы.</returns>
        private HomeworkProgressStatus _CreateHomeworkProgressStatus(
           StudentInLesson studentInLesson,
           HomeworkStatus homeworkStatus)
        {
            HomeworkProgressStatus homeworkProgressStatus =
                new HomeworkProgressStatus()
                {
                    HomeworkStatus = homeworkStatus,
                    StudentInLesson = studentInLesson,
                    StatusSetDT = DateTime.Now
                };

            return homeworkProgressStatus;
        }

        /// <summary>
        /// Асинхронный возврат коллекции студентов на уроке.
        /// </summary>
        /// <param name="lessonId">Идентификатор студента на уроке.</param>
        /// <returns>Коллекция студентов на уроке.</returns>
        private async Task<List<StudentInLesson>> _GetStudentsInLessonByLessonId(
            int lessonId)
        {
            return await _studentInLessonRepo.GetListAsync(
                new StudentInLessonSpecification(lessonId));
        }

        #endregion Utilities

        #region Methods

        /// <summary>
        /// Асинхронное проверка существования класса.
        /// </summary>
        /// <param name="classId">Идентификатор класса.</param>
        /// <returns>Класс.</returns>
        public async Task<Class> ClassExistsAsync(int classId)
        {
            return await _classRepo.GetItemAsync(
                new ClassSpecification(classId));
        }

        /// <summary>
        /// Асинхронное проверка существования студента.
        /// </summary>
        /// <param name="studentId">Идентификатор студента.</param>
        /// <returns>Студент.</returns>
        public async Task<Student> StudentExistsAsync(int studentId)
        {
            return await _studentRepo.GetItemAsync(
                new StudentSpecification(studentId));
        }

        /// <summary>
        /// Асинхронное проверка существования учителя.
        /// </summary>
        /// <param name="teacherId">Идентификатор учителя.</param>
        /// <returns>Учитель.</returns>
        public async Task<Teacher> TeacherExistsAsync(int teacherId)
        {
            return await _teacherRepo.GetItemAsync(
                new TeacherSpecification(teacherId));
        }

        /// <summary>
        /// Асинхронное проверка существования предмета.
        /// </summary>
        /// <param name="subjectId">Идентификатор предмета.</param>
        /// <returns>Предмет.</returns>
        public async Task<Subject> SubjectExistsAsync(int subjectId)
        {
            return await _subjectRepo.GetItemAsync(
                new SubjectSpecification(subjectId));
        }

        /// <summary>
        /// Асинхронное проверка существования родителя.
        /// </summary>
        /// <param name="parentId">Идентификатор родителя.</param>
        /// <returns>Родитель.</returns>
        public async Task<Parent> ParentExistsAsync(int parentId)
        {
            return await _parentRepo.GetItemAsync(
                new ParentSpecification(parentId));
        }

        /// <summary>
        /// Асинхронное проверка существования встречи родителя и учителя.
        /// </summary>
        /// <param name="teacherParentMeetingId">Идентификатор встречи родителя и учителя.</param>
        /// <returns>Встреча родителя и учителя.</returns>
        public async Task<TeacherParentMeeting> TeacherParentMeetingExists(
          int teacherParentMeetingId)
        {
            return await _teacherParentMeetingRepo.GetItemAsync(
                new TeacherParentMeetingSpecification(teacherParentMeetingId));
        }

        /// <summary>
        /// Асинхронное проверка существования статуса домашней работы.
        /// </summary>
        /// <param name="homeworkStatusId">Идентификатор статуса домашней работы.</param>
        /// <returns>Статус домашней работы.</returns>
        public async Task<HomeworkStatus> HomeworkStatusExistsAsync(int homeworkStatusId)
        {
            return await _homeWorkStatusRepo.GetItemAsync(
                new HomeworkStatusSpecification(homeworkStatusId));
        }

        /// <summary>
        /// Асинхронное проверка существования урока.
        /// </summary>
        /// <param name="lessonId">Идентификатор урока.</param>
        /// <returns>Урок.</returns>
        public async Task<Lesson> LessonExistsAsync(int lessonId)
        {
            return await _lessonRepo.GetItemAsync(
                new LessonSpecification(lessonId));
        }

        /// <summary>
        /// Асинхронное проверка существования домашней работы.
        /// </summary>
        /// <param name="homeworkId">Идентификатор домашней работы.</param>
        /// <returns>Домашняя работа.</returns>
        public async Task<Homework> HomeworkExistsAsync(int homeworkId)
        {
            return await _homeworkRepo.GetItemAsync(
                new HomeworkSpecification(homeworkId));
        }

        /// <summary>
        /// Асинхронное проверка обьекта учитель/класс/предмет.
        /// </summary>
        /// <param name="teacherId">Идентификатор учителя.</param>
        /// <param name="classId">Идентификатор класса.</param>
        /// <param name="subjectId">Идентификатор предмета.</param>
        /// <returns>обьекта учитель/класс/предмет.</returns>
        public async Task<TeacherClassSubject> TeacherClassSubjectExistsAsync(
            int teacherId,
            int classId,
            int subjectId)
        {
            return await _teacherClassSubjectRepo.GetItemAsync(
                new TeacherClassSubjectSpecification(teacherId, classId, subjectId));
        }

        /// <summary>
        /// Асинхронное проверка студента на уроке.
        /// </summary>
        /// <param name="studentId">Идентификатор студента.</param>
        /// <param name="lessonId">Идентификатор урока.</param>
        /// <returns>Студента на уроке.</returns>
        public async Task<StudentInLesson> StudentInLessonExistsAsync(
            int studentId, int lessonId)
        {
            return await _studentInLessonRepo.GetItemAsync(
                new StudentInLessonSpecification(studentId, lessonId));
        }

        /// <summary>
        /// Асинхронное проверка существования домашней работы по идентификатру урока.
        /// </summary>
        /// <param name="lessonId">Идентификатор урока.</param>
        /// <returns>Домашняя работа.</returns>
        public async Task<Homework> HomeworkExistsByLessonIdAsync(int lessonId)
        {
            return await _homeworkRepo.GetItemAsync(
                new HomeworkSpecificationByLesson(lessonId));
        }

        /// <summary>
        /// Асинхронное получение списка студентов по идентификатору класса.
        /// </summary>
        /// <param name="classId">Идентификатор класса.</param>
        /// <returns>Коллекция студентов.</returns>
        public async Task<List<Student>> GetStudentsByClassIdAsync(int classId)
        {
            return await _studentRepo.GetListAsync(
                new StudentsSpecification(classId));
        }

        /// <summary>
        /// Получение коллекции родителей по идентификатору студента.
        /// </summary>
        /// <param name="studentId">Идентификатор студента.</param>
        /// <returns>Коллекция родителей.</returns>
        public async Task<List<Parent>> GetParentsByStudentIdAsync(int studentId)
        {
            return await _parentRepo.GetListAsync(
                new ParentsSpecification(studentId));
        }

        /// <summary>
        /// Асинхронное получение статуса прогресса домашней работы.
        /// </summary>
        /// <param name="studentInLessonId">Идентификатор студента на уроке.</param>
        /// <returns>Статус прогресса домашней работы.</returns>
        public async Task<HomeworkProgressStatus> GetHomeworkProgressStatusAsync(
            int studentInLessonId)
        {
            return await _homeworkProgressStatusRepo.GetItemAsync(
                new HomeworkProgressStatusSpecification(studentInLessonId));
        }

        /// <summary>
        /// Асинхронное получение готовой домашней работы по идентификатору студента на уроке.
        /// </summary>
        /// <param name="studentInLessonId">Идентификатор студента на уроке.</param>
        /// <returns></returns>
        public async Task<CompletedHomework> GetCompletedHomeworkByStudentInLessonIdAsync(
            int studentInLessonId)
        {
            return await _completedHomeworkRepo.GetItemAsync(
                new CompletedHomeworkSpecification(studentInLessonId));
        }

       
        /// <summary>
        /// Асинхронное добавление урока.
        /// </summary>
        /// <param name="teacherId">Идентификатор урока.</param>
        /// <param name="classId">Идентификатор класса.</param>
        /// <param name="subjectId">Идентификатор предмета.</param>
        /// <param name="theme">Тема урока.</param>
        /// <param name="lessonDateTime">Время урока.</param>
        /// <returns>Результат выполнения операции.</returns>
        public async Task AddLessonAsync(
            int teacherId,
            int classId,
            int subjectId,
            string theme,
            DateTime lessonDateTime)
        {
            List<Student> students;
            TeacherClassSubject teacherClassSubject;

            students = await GetStudentsByClassIdAsync(classId);
            ResultHandler.CheckResult(students);

            teacherClassSubject = await TeacherClassSubjectExistsAsync(
                teacherId, classId, subjectId);

            ResultHandler.CheckResult(teacherClassSubject);

            Lesson lesson = _CreateLesson(
                lessonDateTime, teacherClassSubject, theme);

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _lessonRepo.AddAsync(lesson);
                    await _lessonRepo.SaveChangesAsync();

                    foreach (Student student in students)
                    {
                        StudentInLesson studentInLesson =
                            _CreateStudentInLesson(lesson, student);

                        await _studentInLessonRepo.AddAsync(studentInLesson);
                    }

                    await _studentInLessonRepo.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// Асинхронное добавление домашней работы.
        /// </summary>
        /// <param name="lessonId"><Идентификатор урока./param>
        /// <param name="finishDateTime">Время окончания урок</param>
        /// <param name="homeWork">Домашняя работа.</param>
        /// <returns>Результат выполнения операции.</returns>
        public async Task AddHomeworkAsync(
            int lessonId,
            DateTime finishDateTime,
            string homeWork)
        {
            Lesson lesson;
            List<StudentInLesson> studentsInLesson;
            HomeworkStatus appointedHomeworkStatus;

            lesson = await LessonExistsAsync(lessonId);
            ResultHandler.CheckResult(lesson);

            studentsInLesson = await _GetStudentsInLessonByLessonId(lessonId);
            ResultHandler.CheckResult(studentsInLesson);

            HomeworkStatusSpecification homeworkStatusSpecification = 
                new HomeworkStatusSpecification((int)HomeworkStatuses.Appointed);

            appointedHomeworkStatus = 
                await _homeWorkStatusRepo.GetItemAsync(
                    homeworkStatusSpecification);

            ResultHandler.CheckResult(appointedHomeworkStatus);

            Homework homework = _CreateHomework(
                lesson, finishDateTime, homeWork);

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _homeworkRepo.AddAsync(homework);
                    await _homeworkRepo.SaveChangesAsync();

                    foreach (var studentInLesson in studentsInLesson)
                    {
                        CompletedHomework completedHomework =
                            _CreateCompletedHomework(studentInLesson);

                        await _completedHomeworkRepo.AddAsync(completedHomework);

                        HomeworkProgressStatus homeworkProgressStatus =
                             _CreateHomeworkProgressStatus(
                                 studentInLesson, appointedHomeworkStatus);

                        await _homeworkProgressStatusRepo.AddAsync(
                            homeworkProgressStatus);
                    }

                    await _completedHomeworkRepo.SaveChangesAsync();
                    await _homeworkProgressStatusRepo.SaveChangesAsync();

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// Асинхронное добавление встречи учителя и родителя.
        /// </summary>
        /// <param name="studentId">Идентификатор студента.</param>
        /// <param name="teacherId">Идентификатор учителя.</param>
        /// <param name="parentId">Идентификатор родителя.</param>
        /// <param name="meetingDT">Время встречи родителя и учителя.</param>
        /// <returns>Результат выполнения операции.</returns>
        public async Task AddTeacherParentMeetingAsync(
            int studentId, 
            int teacherId, 
            int parentId, 
            DateTime meetingDT)
        {
            Student student;
            Teacher teacher;
            Parent parent;
            TeacherParentMeeting teacherParentMeeting;

            student = await StudentExistsAsync(studentId);
            ResultHandler.CheckResult(student);

            teacher = await TeacherExistsAsync(teacherId);
            ResultHandler.CheckResult(teacher);

            parent = await ParentExistsAsync(parentId);
            ResultHandler.CheckResult(parent);

            teacherParentMeeting = _CreateTeacherParentMeeting(
                student, teacher, parent, meetingDT);

            await _teacherParentMeetingRepo.AddAsync(teacherParentMeeting);
            await _teacherParentMeetingRepo.SaveChangesAsync();
        }

        /// <summary>
        /// Асинхронное добавления статуса прогресса домашней работы. 
        /// </summary>
        /// <param name="studentId">Идентификатор студента.</param>
        /// <param name="lessonId">Идентификатор урока.</param>
        /// <param name="homeworkStatusId">Статус домашней работы.</param>
        /// <returns>Результат выполнения операции.</returns>
        public async Task AddHomeworkProgressStatusAsync(
            int studentId, int lessonId, int homeworkStatusId)
        {
            StudentInLesson studentInLesson;
            HomeworkStatus homeworkStatuses;
            HomeworkProgressStatus homeworkProgressStatus;

            studentInLesson = await StudentInLessonExistsAsync(
                studentId, lessonId);
            ResultHandler.CheckResult(studentInLesson);

            homeworkStatuses = await HomeworkStatusExistsAsync(homeworkStatusId);
            ResultHandler.CheckResult(homeworkStatusId);

            homeworkProgressStatus = _CreateHomeworkProgressStatus(
                studentInLesson, homeworkStatuses);

            await _homeworkProgressStatusRepo.AddAsync(homeworkProgressStatus);
            await _homeworkProgressStatusRepo.SaveChangesAsync(); 
        }

        /// <summary>
        /// Асинхронное обновление оценки домашней работы.
        /// </summary>
        /// <param name="studentId">Идентификатор студента.</param>
        /// <param name="lessonId">Идентификатор урока.</param>
        /// <param name="grade">Оценка.</param>
        /// <returns>Результат выполнения операции.</returns>
        public async Task UpdateGradeHomeworkAsync(int studentId, int lessonId, int grade)
        {
            Student student;
            Lesson lesson;
            StudentInLesson studentInLesson;
            CompletedHomework completedHomework;

            lesson = await LessonExistsAsync(lessonId);
            ResultHandler.CheckResult(lesson);

            student = await StudentExistsAsync(studentId);
            ResultHandler.CheckResult(student);

            studentInLesson = await StudentInLessonExistsAsync(
                studentId, lessonId);
            ResultHandler.CheckResult(studentInLesson);

            completedHomework = await GetCompletedHomeworkByStudentInLessonIdAsync(
                studentInLesson.Id);
            ResultHandler.CheckResult(completedHomework);

            completedHomework.Grade = grade;
            _completedHomeworkRepo.Update(completedHomework);

             await _completedHomeworkRepo.SaveChangesAsync();
        }

        /// <summary>
        /// Асинхронное обновление оценки домашней работы.
        /// </summary>
        /// <param name="studentId">Идентификатор студента.</param>
        /// <param name="lessonId">Идентификатор урока.</param>
        /// <param name="grade">Оценка.</param>
        /// <returns>Результат выполнения операции.</returns>
        public async Task UpdateGradeStudentInLessonAsync(
            int studentId, int lessonId, int grade)
        {
            Student student;
            Lesson lesson;
            StudentInLesson studentInLesson;

            lesson = await LessonExistsAsync(lessonId);
            ResultHandler.CheckResult(lesson);

            student = await StudentExistsAsync(studentId);
            ResultHandler.CheckResult(student);

            studentInLesson = await StudentInLessonExistsAsync(
                studentId, lessonId);
            ResultHandler.CheckResult(studentInLesson);

            studentInLesson.Grade = grade;
            _studentInLessonRepo.Update(studentInLesson);

            await _studentInLessonRepo.SaveChangesAsync();
        }

        /// <summary>
        /// Асинхронное обновление замечания.
        /// </summary>
        /// <param name="studentId">Идентификатор студента.</param>
        /// <param name="lessonId">Идентификатор урока.</param>
        /// <param name="comment">Замечание.</param>
        /// <returns>Результат выполнения операции.</returns>
        public async Task UpdateCommentAsync(
            int studentId, int lessonId, string comment)
        {
            Student student;
            Lesson lesson;
            StudentInLesson studentInLesson;

            lesson = await LessonExistsAsync(lessonId);
            ResultHandler.CheckResult(lesson);

            student = await StudentExistsAsync(studentId);
            ResultHandler.CheckResult(student);

            studentInLesson = await StudentInLessonExistsAsync(
                studentId, lessonId);
            ResultHandler.CheckResult(studentInLesson);

            studentInLesson.Comment = comment;
            _studentInLessonRepo.Update(studentInLesson);

             await _studentInLessonRepo.SaveChangesAsync();
        }

        /// <summary>
        /// Удаление встречи учителя и родителя.
        /// </summary>
        /// <param name="teacherParentMeetingId">Встречя учителя и родителя.</param>
        /// <returns>Результат выполнения операции.</returns>
        public async Task RemoveTeacherParentMeeting(int teacherParentMeetingId)
        {
            TeacherParentMeetingSpecification teacherParentMeetingSpecification = 
                new TeacherParentMeetingSpecification(teacherParentMeetingId);

            TeacherParentMeeting teacherParentMeeting =
                await _teacherParentMeetingRepo.GetItemAsync(
                    teacherParentMeetingSpecification);

            ResultHandler.CheckResult(teacherParentMeeting);

            _teacherParentMeetingRepo.Remove(teacherParentMeeting);

            await _teacherParentMeetingRepo.SaveChangesAsync();  
        }

        #endregion Methods
    }
}
