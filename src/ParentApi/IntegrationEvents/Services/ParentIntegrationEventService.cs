using Common.Api.Extensions;
using Microsoft.Extensions.Logging;
using ParentApi.Data;
using ParentApi.Data.Specifications;
using ParentApi.IntegrationEvents.Events;
using ParentApi.Models;
using ParentApi.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ParentApi.IntegrationEvents.Services
{
    /// <summary>
    /// Сервис интеграции родителя.
    /// </summary>
    public class ParentIntegrationEventService : IParentIntegrationEventService
    {
        #region Fields

        /// <summary>
        /// Контекст данных.
        /// </summary>
        private AppDbContext _context;

        /// <summary>
        /// Репозиторий для сущности учитель/класс/предмет.
        /// </summary>
        private IRepository<TeacherClassSubject> _teacherClassSubjectRepo;

        /// <summary>
        /// Репозиторий для сущности урока.
        /// </summary>
        private IRepository<Lesson> _lessonRepo;

        /// <summary>
        /// Репозиторий для сущности родителя.
        /// </summary>
        private IRepository<Parent> _parentRepo;

        /// <summary>
        /// Репозиторий для сущности учителя.
        /// </summary>
        private IRepository<Teacher> _teacherRepo;

        /// <summary>
        /// Репозиторий для сущности студента.
        /// </summary>
        private IRepository<Student> _studentRepo;

        /// <summary>
        /// Репозиторий для сущности домашней работы.
        /// </summary>
        private IRepository<Homework> _homeworkRepo;

        /// <summary>
        /// Репозиторий для сущности студента на уроке.
        /// </summary>
        private IRepository<StudentInLesson> _studentInLessonRepo;

        /// <summary>
        /// Репозиторий для сущности готовой работы.
        /// </summary>
        private IRepository<CompletedHomework> _completedHomeworkRepo;

        /// <summary>
        /// Репозиторий для сущности статуса домашней работы.
        /// </summary>
        private IRepository<HomeworkStatus> _homeworkStatusRepo;

        /// <summary>
        /// Репозиторий для сущности статуса прогресса домашней работы.
        /// </summary>
        private IRepository<HomeworkProgressStatus> _homeworkProgressStatusRepo;

        /// <summary>
        /// Репозиторий для сущности встреч учителя и родителя.
        /// </summary>
        private IRepository<TeacherParentMeeting> _teacherParentMeetingRepo;

        /// <summary>
        /// Логгер.
        /// </summary>
        private readonly ILogger<ParentIntegrationEventService> _logger;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="context">Контекст данных.</param>
        /// <param name="teacherClassSubjectRepo">Репозиторий для сущности учитель/класс/предмет.</param>
        /// <param name="lessonRepo">Репозиторий для сущности урока.</param>
        /// <param name="parentRepo">Репозиторий для сущности родителя.</param>
        /// <param name="teacherRpo">Репозиторий для сущности учителя.</param>
        /// <param name="studentRepo">Репозиторий для сущности студента.</param>
        /// <param name="studentInLessonRepo">Репозиторий для сущности студента на уроке.</param>
        /// <param name="completedHomeworkRepo">Репозиторий для сущности готовой работы.</param>
        /// <param name="homeworkStatusRepo">Репозиторий для сущности статуса домашней работы.</param>
        /// <param name="homeworkProgressStatusRepo">Репозиторий для сущности статуса прогресса домашней работы.</param>
        /// <param name="teacherParentMeetingRepo">Репозиторий для сущности встреч учителя и родителя.</param>
        /// <param name="homeworkRepo">Репозиторий для сущности домашней работы.</param>
        /// <param name="logger">Логгер.</param>
        public ParentIntegrationEventService(
          AppDbContext context,
          IRepository<TeacherClassSubject> teacherClassSubjectRepo,
          IRepository<Lesson> lessonRepo,
          IRepository<Parent> parentRepo,
          IRepository<Teacher> teacherRpo,
          IRepository<Student> studentRepo,
          IRepository<StudentInLesson> studentInLessonRepo,
          IRepository<CompletedHomework> completedHomeworkRepo,
          IRepository<HomeworkStatus> homeworkStatusRepo,
          IRepository<HomeworkProgressStatus> homeworkProgressStatusRepo,
          IRepository<TeacherParentMeeting> teacherParentMeetingRepo,
          IRepository<Homework> homeworkRepo,
          ILogger<ParentIntegrationEventService> logger)
        {
            _context = context;
            _teacherClassSubjectRepo = teacherClassSubjectRepo;
            _lessonRepo = lessonRepo;
            _parentRepo = parentRepo;
            _studentRepo = studentRepo;
            _teacherRepo = teacherRpo;
            _studentInLessonRepo = studentInLessonRepo;
            _completedHomeworkRepo = completedHomeworkRepo;
            _homeworkStatusRepo = homeworkStatusRepo;
            _homeworkProgressStatusRepo = homeworkProgressStatusRepo;
            _teacherParentMeetingRepo = teacherParentMeetingRepo;
            _homeworkRepo = homeworkRepo;
            _logger = logger;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Асинхронное создание урока.
        /// </summary>
        /// <param name="event">Событие интеграции.</param>
        /// <returns>Результат выполнения операции.</returns>
        public async Task CreateLessonAsync(CreateLessonEvent @event)
        {
            _logger.LogIntegrationEventStart(@event);

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    List<Student> students = await _studentRepo
                        .GetListAsync(new StudentSpecification(@event.ClassId));

                    TeacherClassSubject teacherClassSubject =
                        await _teacherClassSubjectRepo.GetItemAsync(
                            new TeacherClassSubjectSpecification(
                                @event.TeacherId, @event.ClassId, @event.TeacherId));

                    Lesson lesson = new Lesson()
                    {
                        TeacherClassSubject = teacherClassSubject,
                        LessonDT = @event.LessonDateTime,
                        Theme = @event.Theme
                    };


                    await _lessonRepo.AddAsync(lesson);
                    await _lessonRepo.SaveChangesAsync();

                    foreach (Student student in students)
                    {
                        StudentInLesson studentInLesson = new StudentInLesson()
                        {
                            Lesson = lesson,
                            Student = student
                        };

                        await _studentInLessonRepo.AddAsync(studentInLesson);
                    }

                    await _studentInLessonRepo.SaveChangesAsync();
                    transaction.Commit();

                    _logger.LogIntegrationEventSuccess(@event);


                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _logger.LogIntegrationEventError(@event, ex);
                }
            }
        }


        /// <summary>
        /// Асинхронное создание встречи родителя и учителя.
        /// </summary>
        /// <param name="event">Событие интеграции.</param>
        /// <returns>Результат выполнения операции.</returns>
        public async Task CreateTeacherParentMeetingAsync(CreateTeacherParentMeetingEvent @event)
        {
            _logger.LogIntegrationEventStart(@event);

            try
            {
                Parent parent = await _parentRepo.GetItemAsync(
                    new ParentSpecification(@event.ParentId));
                Student student = await _studentRepo.GetItemAsync(
                    new StudentSpecification(@event.StudentId));
                Teacher teacher = await _teacherRepo.GetItemAsync(
                    new TeacherSpecification(@event.TeacherId));

                TeacherParentMeeting teacherParentMeeting = new TeacherParentMeeting()
                {
                    Parent = parent,
                    Student = student,
                    Teacher = teacher,
                    MeetingDT = @event.MeetingDT,
                    TeacherInitiative = true
                };

                await _teacherParentMeetingRepo.AddAsync(teacherParentMeeting);
                await _teacherParentMeetingRepo.SaveChangesAsync();

                _logger.LogIntegrationEventSuccess(@event);

            }
            catch (Exception ex)
            {
                _logger.LogIntegrationEventError(@event, ex);
            }
        }

        /// <summary>
        /// Асинхронное создание домашней работы.
        /// </summary>
        /// <param name="event">Событие интеграции.</param>
        /// <returns>Результат выполнения операции.</returns>
        public async Task CreateHomeworkAsync(CreateHomeworkEvent @event)
        {
            _logger.LogIntegrationEventStart(@event);

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    Lesson lesson = await _lessonRepo.GetItemAsync(
                        new LessonSpecification(@event.LessonId));

                    List<StudentInLesson> studentInLessons = await 
                        _studentInLessonRepo.GetListAsync(
                            new StudentInLessonSpecificationFilterByLesson(
                                @event.LessonId));

                    HomeworkStatus appointedHomeworkStatus = await
                         _homeworkStatusRepo.GetItemAsync(
                             new HomeworkStatusSpecification ((int)HomeworkStatuses.Appointed));

                    Homework homework = new Homework()
                    {
                        Lesson = lesson,
                        StartDT = lesson.LessonDT,
                        FinishDT = @event.FinishDateTime,
                        Howework = @event.Homework
                    };

                    await _homeworkRepo.AddAsync(homework);
                    await _homeworkRepo.SaveChangesAsync();

                    foreach (var studentInLesson in studentInLessons)
                    {
                        CompletedHomework completedHomework = new CompletedHomework()
                        {
                            StudentInLesson = studentInLesson
                        };

                        await _completedHomeworkRepo.AddAsync(completedHomework);

                        HomeworkProgressStatus homeworkProgressStatus =
                            new HomeworkProgressStatus()
                            {
                                HomeworkStatus = appointedHomeworkStatus,
                                StudentInLesson = studentInLesson,
                                StatusSetDT = DateTime.Now
                            };
                        await _homeworkProgressStatusRepo.AddAsync(homeworkProgressStatus);
                    }

                    await _completedHomeworkRepo.SaveChangesAsync();
                    await _homeworkProgressStatusRepo.SaveChangesAsync();

                    _logger.LogIntegrationEventSuccess(@event);
                    transaction.Commit();

                }
                catch (Exception ex)
                {
                    _logger.LogIntegrationEventError(@event, ex);
                }
            }
         }

        /// <summary>
        /// Асинхронное изменение статуса домашней работы.
        /// </summary>
        /// <param name="event">Событие интеграции.</param>
        /// <returns>Результат выполнения операции.</returns>
        public async Task ChangeStatusHomeworkAsync(ChangeStatusHomeworkEvent @event)
        {
            _logger.LogIntegrationEventStart(@event);

            try
            {
                StudentInLesson studentInLesson = await
                _studentInLessonRepo.GetItemAsync(
                    new StudentInLessonSpecification(
                        @event.StudentId, @event.LessonId));

                HomeworkStatus homeworkStatus = await
                    _homeworkStatusRepo.GetItemAsync(
                        new HomeworkStatusSpecification(@event.HomeworkStatusId));

                HomeworkProgressStatus homeworkProgressStatus = new HomeworkProgressStatus()
                {
                    HomeworkStatus = homeworkStatus,
                    StudentInLesson = studentInLesson,
                    StatusSetDT = DateTime.Now
                };

                await _homeworkProgressStatusRepo.AddAsync(homeworkProgressStatus);
                await _homeworkProgressStatusRepo.SaveChangesAsync();

                _logger.LogIntegrationEventSuccess(@event);
            }
            catch (Exception ex)
            {
                _logger.LogIntegrationEventError(@event, ex);
            }   
        }

        /// <summary>
        /// Асинхронное оценивание домашней работы.
        /// </summary>
        /// <param name="event">Событие интеграции.</param>
        /// <returns>Результат выполнения операции.</returns>
        public async Task GradeHomeworkAsync(GradeHomeworkEvent @event)
        {
            _logger.LogIntegrationEventStart(@event);

            try
            {
                StudentInLesson studentInLesson = await _studentInLessonRepo
                     .GetItemAsync(new StudentInLessonSpecification(
                         @event.StudentId, @event.LessonId));

                CompletedHomework completedHomework = await _completedHomeworkRepo
                    .GetItemAsync(new CompletedHomeworkSpecification(studentInLesson.Id));

                completedHomework.Grade = @event.Grade;
                _completedHomeworkRepo.Update(completedHomework);

                await  _completedHomeworkRepo.SaveChangesAsync();

                _logger.LogIntegrationEventSuccess(@event);
            }
            catch(Exception ex)
            {
                _logger.LogIntegrationEventError(@event, ex);
            }
        }

        /// <summary>
        /// Асинхронное создание замечания.
        /// </summary>
        /// <param name="event">Событие интеграции.</param>
        /// <returns>Результат выполнения операции.</returns>
        public async Task CreateCommentAsync(CreateCommentEvent @event)
        {
            _logger.LogIntegrationEventStart(@event);

            try
            {
                StudentInLesson studentInLesson = await _studentInLessonRepo.GetItemAsync(
                   new StudentInLessonSpecification(@event.StudentId, @event.LessonId));

                studentInLesson.Comment = @event.Comment;

                _studentInLessonRepo.Update(studentInLesson);

                await _studentInLessonRepo.SaveChangesAsync();

                _logger.LogIntegrationEventSuccess(@event);
            }
            catch (Exception ex)
            {
                _logger.LogIntegrationEventError(@event, ex);
            }
        }

        /// <summary>
        /// Асинхронное оценивание студента.
        /// </summary>
        /// <param name="event">Событие интеграции.</param>
        /// <returns>Результат выполнения операции.</returns>
        public async Task GradeStudentInLessonAsync(GradeStudentInLessonEvent @event)
        {
            _logger.LogIntegrationEventStart(@event);

            try
            {
                StudentInLesson studentInLesson =
                    await _studentInLessonRepo.GetItemAsync(
                        new StudentInLessonSpecification(@event.StudentId, @event.LessonId));

                studentInLesson.Grade = @event.Grade;

                _studentInLessonRepo.Update(studentInLesson);

                await _studentInLessonRepo.SaveChangesAsync();

                _logger.LogIntegrationEventSuccess(@event);
            }
            catch (Exception ex)
            {
                _logger.LogIntegrationEventError(@event, ex);
            }
        }

        /// <summary>
        /// Асинхронное удаление встречи учителя и родителя.
        /// </summary>
        /// <param name="event">Событие интеграции.</param>
        /// <returns>Результат выполнения операции.</returns>
        public async Task RemoveTeacherParentMeetingAsync(RemoveTeacherParentMeetingEvent @event)
        {
            _logger.LogIntegrationEventStart(@event);

            try
            {
                TeacherParentMeeting teacherParentMeeting =
                    await _teacherParentMeetingRepo.GetItemAsync(
                        new TeacherParentMeetingSpecification(@event.TeacherParentMeetingId));

                _teacherParentMeetingRepo.Remove(teacherParentMeeting);

                await _teacherParentMeetingRepo.SaveChangesAsync();

                _logger.LogIntegrationEventSuccess(@event);
            }
            catch (Exception ex)
            {
                _logger.LogIntegrationEventError(@event, ex);
            }
        }

        #endregion Methods
    }
}
