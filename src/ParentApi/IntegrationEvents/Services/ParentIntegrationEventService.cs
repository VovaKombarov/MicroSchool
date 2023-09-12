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
    public class ParentIntegrationEventService : IParentIntegrationEventService
    {
        #region Fields

        private AppDbContext _context;
        private IRepository<TeacherClassSubject> _teacherClassSubjectRepo;
        private IRepository<Lesson> _lessonRepo;
        private IRepository<Parent> _parentRepo;
        private IRepository<Teacher> _teacherRepo;
        private IRepository<Student> _studentRepo;
        private IRepository<Homework> _homeworkRepo;
        private IRepository<StudentInLesson> _studentInLessonRepo;
        private IRepository<CompletedHomework> _completedHomeworkRepo;
        private IRepository<HomeworkStatus> _homeworkStatusRepo;
        private IRepository<HomeworkProgressStatus> _homeworkProgressStatusRepo;
        private IRepository<TeacherParentMeeting> _teacherParentMeetingRepo;
        private readonly ILogger<ParentIntegrationEventService> _logger;

        #endregion Fields

        #region Constructors

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
