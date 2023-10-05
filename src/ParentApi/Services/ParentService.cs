using Common.Api;
using ParentApi.Data;
using ParentApi.Data.Specifications;
using ParentApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParentApi.Services
{
    /// <summary>
    /// Сервис родителя.
    /// </summary>
    public class ParentService : IParentService
    {
        #region Fields 

        /// <summary>
        /// Репозиторий для сущности студента.
        /// </summary>
        private readonly IRepository<Student> _studentRepo;

        /// <summary>
        /// Репозиторий для сущности учителя.
        /// </summary>
        private readonly IRepository<Teacher> _teacherRepo;

        /// <summary>
        /// Репозиторий для сущности родителя.
        /// </summary>
        private readonly IRepository<Parent> _parentRepo;

        /// <summary>
        /// Репозиторий для сущности встреч учителя и родителя.
        /// </summary>
        private readonly IRepository<TeacherParentMeeting> _teacherParentMeetingRepo;

        /// <summary>
        /// Репозиторий для сущности урока.
        /// </summary>
        private readonly IRepository<Lesson> _lessonRepo;

        /// <summary>
        /// Репозиторий для сущности студента на уроке.
        /// </summary>
        private readonly IRepository<StudentInLesson>  _studentInLessonRepo;

        /// <summary>
        /// Репозиторий для сущности готовой работы.
        /// </summary>
        private readonly IRepository<CompletedHomework> _completedHomeworkRepo;

        /// <summary>
        /// Репозиторий для сущности предмета.
        /// </summary>
        private readonly IRepository<Subject> _subjectRepo;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="studentRepo">Репозиторий для сущности студента.</param>
        /// <param name="teacherRepo">Репозиторий для сущности учителя.</param>
        /// <param name="parentRepo">Репозиторий для сущности родителя.</param>
        /// <param name="teacherParentMeetingRepo">Репозиторий для сущности встреч учителя и родителя.</param>
        /// <param name="lessonRepo">Репозиторий для сущности урока.</param>
        /// <param name="studentInLessonRepo">Репозиторий для сущности студента на уроке.</param>
        /// <param name="completedHomeworkRepo">Репозиторий для сущности готовой работы.</param>
        /// <param name="subjectRepo">Репозиторий для сущности предмета.</param>
        public ParentService(
            IRepository<Student> studentRepo,
            IRepository<Teacher> teacherRepo,
            IRepository<Parent> parentRepo,
            IRepository<TeacherParentMeeting> teacherParentMeetingRepo,
            IRepository<Lesson> lessonRepo,
            IRepository<StudentInLesson> studentInLessonRepo,
            IRepository<CompletedHomework> completedHomeworkRepo,
            IRepository<Subject> subjectRepo)
        {
            _studentRepo = studentRepo;
            _teacherRepo = teacherRepo;
            _parentRepo = parentRepo;
            _teacherParentMeetingRepo = teacherParentMeetingRepo;
            _lessonRepo = lessonRepo;
            _studentInLessonRepo = studentInLessonRepo;
            _completedHomeworkRepo = completedHomeworkRepo;
            _subjectRepo = subjectRepo;
        }

        #endregion Constructors

        #region Utilities

        /// <summary>
        /// Mapping свойств на сущность TeacherParentMeeting.
        /// </summary>
        /// <param name="student">Студент.</param>
        /// <param name="teacher">Учитель.</param>
        /// <param name="parent">Родитель.</param>
        /// <param name="meetingDT">Время митинга.</param>
        /// <returns>Встречу родителя и учителя.</returns>
        private TeacherParentMeeting _CreateTeacherParentMeeting(
           Student student,
           Teacher teacher,
           Parent parent,
           DateTime meetingDT)
        {
            TeacherParentMeeting teacherParentMeeting =
                new TeacherParentMeeting()
                {
                    Student = student,
                    Parent = parent,
                    Teacher = teacher,
                    TeacherInitiative = false,
                    MeetingDT = meetingDT
                };
            return teacherParentMeeting;
        }

        #endregion Utilities

        #region Methods

        /// <summary>
        /// Асинхронная проверка существования студента.
        /// </summary>
        /// <param name="studentId">Идентификатор студента.</param>
        /// <returns>Студент.</returns>
        public async Task<Student> StudentExistsAsync(int studentId)
        {
            return await _studentRepo.GetItemAsync(
                new StudentSpecification(studentId));
        }

        /// <summary>
        /// Асинхронная проверка существования учителя.
        /// </summary>
        /// <param name="teacherId">Идентификатор учителя.</param>
        /// <returns>Учитель.</returns>
        public async Task<Teacher> TeacherExistsAsync(int teacherId)
        {
            return await _teacherRepo.GetItemAsync(
                new TeacherSpecification(teacherId));
        }

        /// <summary>
        /// Асинхронная проверка существования родителя.
        /// </summary>
        /// <param name="parentId">Идентификатор родителя.</param>
        /// <returns>Родитель.</returns>
        public async Task<Parent> ParentExistsAsync(int parentId)
        {
            return await _parentRepo.GetItemAsync(
                new ParentSpecification(parentId));
        }

        /// <summary>
        /// Асинхронная проверка существования урока.
        /// </summary>
        /// <param name="lessonId">Идентификатор урока.</param>
        /// <returns>Урок.</returns>
        public async Task<Lesson> LessonExistsAsync(int lessonId)
        {
            return await _lessonRepo.GetItemAsync(
                new LessonSpecification(lessonId));
        }

        /// <summary>
        /// Асинхронная проверка существования родителя.
        /// </summary>
        /// <param name="subjectId">Идентификатор предмета.</param>
        /// <returns>Предмет.</returns>
        public async Task<Subject> SubjectExistsAsync(int subjectId)
        {
            return await _subjectRepo.GetItemAsync(
                new SubjectSpecification(subjectId));
        }

        /// <summary>
        /// Асинхронная проверка существования студента на уроке.
        /// </summary>
        /// <param name="studentId">Идентификатор студента.</param>
        /// <param name="lessonId">Идентификатор урока.</param>
        /// <returns>Студент на уроке.</returns>
        public async Task<StudentInLesson> StudentInLessonExistsAsync(
            int studentId, int lessonId)
        {
            return await _studentInLessonRepo.GetItemAsync(
                new StudentInLessonSpecification(studentId, lessonId));
        }

        /// <summary>
        /// Асинхронная проверка существования встречи учителя и родителя.
        /// </summary>
        /// <param name="teacherParentMeetingId">Идентификатор встречи родителя и учителя.</param>
        /// <returns>Встреча родителя и учителя.</returns>
        public async Task<TeacherParentMeeting> TeacherParentMeetingExistsAsync(
           int teacherParentMeetingId)
        {
            return await _teacherParentMeetingRepo.GetItemAsync(
                new TeacherParentMeetingSpecification(teacherParentMeetingId));
        }

        /// <summary>
        /// Асинхронная проверка существования встречи учителя и родителя.
        /// </summary>
        /// <param name="studentId">Идентификатор студента.</param>
        /// <param name="teacherId">Идентификатор учителя.</param>
        /// <param name="parentId">Идентификатор родителя.</param>
        /// <param name="meetingDT">Время встречи.</param>
        /// <returns>Встреча родителя и учителя.</returns>
        public async Task<TeacherParentMeeting> TeacherParentMeetingExistsAsync(
            int studentId,
            int teacherId,
            int parentId,
            DateTime meetingDT)
        {
            return await _teacherParentMeetingRepo.GetItemAsync(
                new TeacherParentMeetingSpecification(
                    studentId, teacherId, parentId, meetingDT));
        }

        /// <summary>
        /// Асинхронное получение коллекции студентов на уроке.
        /// </summary>
        /// <param name="studentId">Идентификатор студента.</param>
        /// <returns>Коллекцию студентов на уроке.</returns>
        public async Task<List<StudentInLesson>> GetStudentInLessonsAsync(
            int studentId)
        {
            return await _studentInLessonRepo.GetListAsync(
                new StudentInLessonSpecification(studentId));
        }

        /// <summary>
        /// Добавляет встречу учителя и родителя.
        /// </summary>
        /// <param name="studentId">Идентификатор студента.</param>
        /// <param name="teacherId">Идентификатор учителя.</param>
        /// <param name="parentId">Идентификатор родителя.</param>
        /// <param name="meetingDT">Время встречи.</param>
        /// <returns>Результат выполнения операции.</returns>
        public async Task AddTeacherParentMeetingAsync(
            int studentId, int teacherId, int parentId, DateTime meetingDT)
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
        /// Асинхронный возврат готовой домашней работы.
        /// </summary>
        /// <param name="studentId">Идентификатор студента.</param>
        /// <param name="lessonId">Идентификатор урока.</param>
        /// <returns>Готовая домашняя работа.</returns>
        public async Task<CompletedHomework> GetCompletedHomeworkAsync(
            int studentId, int lessonId)
        {
            StudentInLesson studentInLesson;

            studentInLesson = await _studentInLessonRepo.GetItemAsync(
               new StudentInLessonSpecification(studentId, lessonId));

            ResultHandler.CheckResult(studentInLesson);

            CompletedHomework completedHomework =
                await _completedHomeworkRepo.GetItemAsync(
                    new CompletedHomeworkSpecification(studentInLesson.Id));

            ResultHandler.CheckResult(completedHomework);

            return completedHomework;
        }

        /// <summary>
        /// Асинхронное получение оценок.
        /// </summary>
        /// <param name="studentId">Идентификатор студента.</param>
        /// <param name="subjectId">Идентификатор предмета.</param>
        /// <returns>Коллекция оценок.</returns>
        public async Task<List<int>> GetGradesAsync(
            int studentId, int subjectId)
        {
            Student student;
            Subject subject;

            student = await StudentExistsAsync(studentId);
            ResultHandler.CheckResult(student);

            subject = await SubjectExistsAsync(subjectId);
            ResultHandler.CheckResult(subject);

            List<StudentInLesson> studentInLessons =
                await _studentInLessonRepo.GetListAsync(
                    new StudentInLessonSpecificationFilterBySubject(
                        studentId, subjectId));
                    
            ResultHandler.CheckResult(studentInLessons);

            List<int> grades = studentInLessons
                .Where(w => w.Grade != null)
                .Select(w => (int)w.Grade)
                .ToList();

            return grades;
        }

        /// <summary>
        /// Асинхронное удаление встречи родителя и учителя.
        /// </summary>
        /// <param name="teacherParentMeetingId">Идентификатор встречи родителя и учителя.</param>
        /// <returns>Результат выполнения операции.</returns>
        public async Task RemoveParentTeacherMeetingAsync(
            int teacherParentMeetingId)
        {
            TeacherParentMeeting teacherParentMeeting = 
                await _teacherParentMeetingRepo.GetItemAsync(
                    new TeacherParentMeetingSpecification(
                        teacherParentMeetingId));

            ResultHandler.CheckResult(teacherParentMeeting);

            _teacherParentMeetingRepo.Remove(teacherParentMeeting);

            await _teacherParentMeetingRepo.SaveChangesAsync();   
        }

        #endregion Methods
    }
}
