using AutoMapper;
using Common.Api;
using Common.Api.Extensions;
using Common.ErrorResponse;
using Common.EventBus;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using TeacherApi.Data.Dtos;
using TeacherApi.IntegrationEvents;
using TeacherApi.IntegrationEvents.Events;
using TeacherApi.Models;
using TeacherApi.Services;
using TeacherApi.Utilities;

namespace TeacherApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Produces("application/json")]
    [Authorize]
    public class TeachersController : ControllerBase
    {
        #region Fields

        /// <summary>
        /// Маппер.
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Обьект сервиса учителя.
        /// </summary>
        private readonly ITeacherService _teacherService;

        /// <summary>
        /// Словарь опций.
        /// </summary>
        private readonly IOptions<Dictionary<string, string>> _options; 

        /// <summary>
        /// Обьект брокера.
        /// </summary>
        private readonly IEventBus _eventBus;

        /// <summary>
        /// Логгер.
        /// </summary>
        private readonly ILogger _logger;
     
        #endregion Fields

        #region Constructors

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="teacherService">Сервис учителя.</param>
        /// <param name="mapper">Маппер.</param>
        /// <param name="eventBus">Брокер.</param>
        /// <param name="options">Опции.</param>
        /// <param name="logger">Логгер.</param>
        public TeachersController(
            ITeacherService teacherService,
            IMapper mapper,
            IEventBus eventBus,
            IOptions<Dictionary<string, string>> options,
            ILogger logger)
        {
            _mapper = mapper;
            _teacherService = teacherService;
            _eventBus = eventBus;
            _options = options;
            _logger = logger;

            Invoker.InitLogger(_logger);
        }

        #endregion Constructors

        #region Methods 

        /// <summary>
        /// Возвращает коллекцию студентов.
        /// </summary>
        /// <param name="classId"> Идентификатор класса.</param>
        /// <returns>Коллекцию студентов.</returns>
        /// <remarks>
        /// Коллекция студентов возвращается по идентификатору класса.
        /// </remarks>
        /// <response code="200">Returns a list of students</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Empty collection</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        public async Task<ActionResult<List<StudentReadDto>>> GetStudents(
            int classId)
        {
            await Invoker.InvokeAsync(
                _teacherService.ClassExistsAsync(classId));

            List<Student> students = 
                await Invoker.InvokeAsync(
                    _teacherService.GetStudentsByClassIdAsync(classId));

            return new List<StudentReadDto>(
                _mapper.Map<List<StudentReadDto>>(students));
        }

        /// <summary>
        /// Возвращает коллекцию родителей.
        /// </summary>
        /// <remarks>
        /// Коллекция родителей возвращается по идентификатору студента.
        /// </remarks>
        /// <param name="studentId">Идентификатор студента.</param>
        /// <returns>Коллекция родителей.</returns>
        /// <response code="200">Returns a list of parents</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Empty collection</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        public async Task<ActionResult<List<ParentReadDto>>> GetParents(
            int studentId)
        {
            await Invoker.InvokeAsync(
                _teacherService.StudentExistsAsync(studentId));

            List<Parent> parents = 
                await Invoker.InvokeAsync(
                    _teacherService.GetParentsByStudentIdAsync(studentId));

            return new List<ParentReadDto>(
                _mapper.Map<List<ParentReadDto>>(parents));
        }

        /// <summary>
        /// Возвращает статус домашней работы.
        /// </summary>
        /// <param name="lessonId">Идентификатор урока.</param>
        /// <param name="studentId">Идентификатор студента.</param>
        /// <returns>Статус домашней работы.</returns>
        /// <remarks>
        /// Статус домашней работы получается для конкретного студента и 
        /// по конкретному уроку на котором была задана домашняя работа.
        /// </remarks>
        /// <response code="200">Returns homework status.</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not found (student or lesson).</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        public async Task<ActionResult<string>> GetHomeworkStatus(
           int lessonId, int studentId)
        {
            await Invoker.InvokeAsync(
                _teacherService.LessonExistsAsync(lessonId));

            await Invoker.InvokeAsync(
               _teacherService.StudentExistsAsync(studentId));

            await Invoker.InvokeAsync(
                _teacherService.HomeworkExistsAsync(lessonId));

            StudentInLesson studentInLesson = 
                await Invoker.InvokeAsync(
                    _teacherService.StudentInLessonExistsAsync(
                        studentId, lessonId));

            HomeworkProgressStatus homeworkProgressStatus = 
                await Invoker.InvokeAsync(
                    _teacherService.GetHomeworkProgressStatusAsync(
                        studentInLesson.Id));

            return Ok(homeworkProgressStatus.HomeworkStatus);
        }

        /// <summary>
        /// Возвращает готовую домашнюю работу.
        /// </summary>
        /// <param name="lessonId">Идентификатор урока.</param>
        /// <param name="studentId">Идентификатор студента.</param>
        /// <returns>Готовую домашнюю работу.</returns>
        /// <remarks>
        /// Готовая домашняя работа конкретного студента по конкретному уроку.
        /// </remarks>
        /// <response code="200">Returns completed homework.</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not found (student or lesson or homework or studentInLesson).</response>
        /// <response code="400">HomeworkStatus incorrect or completedhomework is empty</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        public async Task<ActionResult<CompletedHomeworkReadDto>> GetCompletedHomework(
          int lessonId, int studentId)
        {
            await Invoker.InvokeAsync(
                _teacherService.LessonExistsAsync(lessonId));

            await Invoker.InvokeAsync(
                _teacherService.StudentExistsAsync(studentId));

            await Invoker.InvokeAsync(
                _teacherService.HomeworkExistsAsync(lessonId));

            StudentInLesson studentInLesson = 
                await Invoker.InvokeAsync(
                    _teacherService.StudentInLessonExistsAsync(
                        studentId, lessonId));

            HomeworkProgressStatus homeworkProgressStatus = 
                await Invoker.InvokeAsync(
                    _teacherService.GetHomeworkProgressStatusAsync(
                        studentInLesson.Id));

            if (homeworkProgressStatus.HomeworkStatus.Id != 
                (int)HomeworkStatuses.Completed)
            {
                throw new HttpStatusException(
                    HttpStatusCode.BadRequest,
                   _options.Value[MessageKey.HomeworkStatusNotValid.ToString()]); 
            }

            CompletedHomework completedHomework = 
                await Invoker.InvokeAsync(
                    _teacherService.GetCompletedHomeworkByStudentInLessonIdAsync(
                        studentInLesson.Id));

            ResultHandler.CheckString(completedHomework.Work);

            return Ok(_mapper.Map<CompletedHomeworkReadDto>(
                completedHomework));
        }


        /// <summary>
        /// Создает урок.
        /// </summary>
        /// <param name="teacherId">Идентификатор учителя.</param>
        /// <param name="classId">Идентификатор класса.</param>
        /// <param name="subjectId">Идентификатор предмета.</param>
        /// <param name="theme">Тема урока.</param>
        /// <param name="lessonDateTime">Время урока.</param>
        /// <returns>Результат выполнения операции.</returns>
        /// <remarks>
        /// Создает урок по идентификатору учителя, по идентификатору класса и идентификатору предмета.
        /// Тема урока не должна быть пустой.
        /// Время урока не может быть меньше текущего времени.
        /// </remarks>
        /// <response code="200">New lesson is create.</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not found (teacher or class or subject).</response>
        /// <response code="400">Theme is null or empty, lessonDateTime is less current datetime.</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        public async Task<ActionResult> CreateLesson(
           int teacherId,
           int classId,
           int subjectId,
           string theme,
           DateTime lessonDateTime)
        {
            await Invoker.InvokeAsync(
                _teacherService.TeacherExistsAsync(teacherId));

            await Invoker.InvokeAsync(
                _teacherService.ClassExistsAsync(classId));

            await Invoker.InvokeAsync(
                _teacherService.SubjectExistsAsync(subjectId));

            await Invoker.InvokeAsync(
                _teacherService.TeacherClassSubjectExistsAsync(
                    teacherId, classId, subjectId));

            ResultHandler.CheckString(theme);

            if (lessonDateTime < DateTime.Now)
            {
                throw new HttpStatusException(
                    HttpStatusCode.BadRequest,
                    _options.Value[MessageKey.DateTimeNotValid.ToString()]);
            }

            await Invoker.InvokeAsync(_teacherService.AddLessonAsync(
                      teacherId, classId, subjectId, theme, lessonDateTime));

            CreateLessonEvent @event = new CreateLessonEvent(
                     teacherId, classId, subjectId, theme, lessonDateTime);

            try
            {
                _eventBus.Publish(@event);
            }
            catch(Exception ex)
            {
                _logger.LogIntegrationEventError(@event, ex);
            }

            return Ok();
        }

        /// <summary>
        /// Создает домашнюю работу.
        /// </summary>
        /// <param name="lessonId">Идентификатор урока.</param>
        /// <param name="finishDateTime">Время окончания домашней работы.</param>
        /// <param name="homeWork">Домашняя работа.</param>
        /// <returns>Успешное создание домашней работы.</returns>
        /// <remarks>
        /// Создает домашнюю работу. 
        /// Время окончания домашней работы не может быть меньше текущей даты.
        /// Домашняя работа не может быть пустой.
        /// </remarks>
        /// <response code="200">New homework is create.</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not found lesson.</response>
        /// <response code="400">finishDateTime is less current datetime, homework is empty or null.</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        public async Task<ActionResult> CreateHomework(
            int lessonId,
            DateTime finishDateTime,
            string homeWork)
        {
            await Invoker.InvokeAsync(
                _teacherService.LessonExistsAsync(lessonId));

            if (finishDateTime < DateTime.Now)
            {
                throw new HttpStatusException(
                 HttpStatusCode.BadRequest,
                 _options.Value[MessageKey.DateTimeNotValid.ToString()]);
            }

            ResultHandler.CheckString(homeWork);

            await Invoker.InvokeAsync(
                _teacherService.AddHomeworkAsync(
                    lessonId, finishDateTime, homeWork));

            CreateHomeworkEvent @event = new CreateHomeworkEvent(
                lessonId, finishDateTime, homeWork);

            try
            {
                _eventBus.Publish(@event);
            }
            catch(Exception ex)
            {
                _logger.LogIntegrationEventError(@event, ex);
            }
           
            return Ok();
        }

        /// <summary>
        /// Создает встречу учителя и родителя.
        /// </summary>
        /// <param name="studentId">Идентификатор студента.</param>
        /// <param name="teacherId">Идентификатор учителя.</param>
        /// <param name="parentId">Идентификатор родителя.</param>
        /// <param name="meetingDateTime">Время встречи.</param>
        /// <returns>Успешное создание встречи учителя и родителя.</returns>
        /// <remarks>
        /// Создает встречу учителя и родителя. 
        /// Время встречи не может быть меньше текущей.
        /// </remarks>
        /// <response code="200">New TeacherParentMeeting is create.</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not found student or teacher or parent.</response>
        /// <response code="400">meetingDateTime is less current datetime.</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        public async Task<ActionResult> CreateTeacherParentMeeting(
           int studentId,
           int teacherId,
           int parentId,
           DateTime meetingDateTime)
        {
            await Invoker.InvokeAsync(
                _teacherService.StudentExistsAsync(studentId));

            await Invoker.InvokeAsync(
                _teacherService.TeacherExistsAsync(teacherId));

            await Invoker.InvokeAsync(
                _teacherService.ParentExistsAsync(parentId));


            if (meetingDateTime < DateTime.Now)
            {
                throw new HttpStatusException(
                    HttpStatusCode.BadRequest,
                    _options.Value[MessageKey.DateTimeNotValid.ToString()]);
            }

            await Invoker.InvokeAsync(
                _teacherService.AddTeacherParentMeetingAsync(
                    studentId, teacherId, parentId, meetingDateTime));

            CreateTeacherParentMeetingEvent @event =
                new CreateTeacherParentMeetingEvent(
                    studentId, teacherId, parentId, DateTime.Now);

            try
            {
                _eventBus.Publish(@event);
            }
            catch(Exception ex)
            {
                _logger.LogIntegrationEventError(@event, ex);
            }

            return Ok();
        }

        /// <summary>
        /// Меняет статус домашней работы.
        /// </summary>
        /// <param name="studentId">Идентификатор студента.</param>
        /// <param name="lessonId">Идентификатор урока.</param>
        /// <param name="homeworkStatusId">Идентификатор статуса домашней работы.</param>
        /// <returns>Успешное выполнение операции.</returns>
        /// <remarks>
        /// Меняет статус домашней работы. До того как поменять статус домашней работы,
        /// необходимо проверить наличие урока, студента, студента на уроке, наличие домашней работы,
        /// наличие корректности статуса домашней работы.
        /// </remarks>
        /// <response code="200">Homework status is change.</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not found lesson or student or studentInLesson or homework or homeworkStatus.</response>
        /// <response code="400">meetingDateTime is less current datetime.</response>
        /// <response code="500">Internal server error</response>
        [HttpPut]
        public async Task<ActionResult> ChangeStatusHomework(
            int studentId,
            int lessonId,
            int homeworkStatusId)
        {
            await Invoker.InvokeAsync(
                _teacherService.LessonExistsAsync(lessonId));

            await Invoker.InvokeAsync(
                _teacherService.StudentExistsAsync(studentId));

            await Invoker.InvokeAsync(
                _teacherService.StudentInLessonExistsAsync(
                    studentId, lessonId));

            await Invoker.InvokeAsync(
                _teacherService.HomeworkExistsByLessonIdAsync(lessonId));

            await Invoker.InvokeAsync(
                _teacherService.HomeworkStatusExistsAsync(homeworkStatusId));

            await Invoker.InvokeAsync(
                _teacherService.AddHomeworkProgressStatusAsync(
                    studentId, lessonId, homeworkStatusId));

            ChangeStatusHomeworkEvent @event = new ChangeStatusHomeworkEvent(
                studentId, lessonId, homeworkStatusId);

            try
            {
                _eventBus.Publish(@event);
            }
            catch(Exception ex)
            {
                _logger.LogIntegrationEventError(@event, ex);
            }

            return Ok();
        }

        /// <summary>
        /// Оценить домашнюю работу.
        /// </summary>
        /// <param name="studentId">Идентификатор студента.</param>
        /// <param name="lessonId">Идентификатор урока.</param>
        /// <param name="grade">Оценка.</param>
        /// <returns>Успешное выполнение работы.</returns>
        /// <remarks>
        /// Оценивает домашнюю работу.
        /// </remarks>
        /// <response code="200">Нomework appreciated.</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not found student or lesson or studentInLesson or homework or homeworkProgressStatus.</response>
        /// <response code="400">grade less 2 or grade more 5.</response>
        /// <response code="500">Internal server error</response>
        [HttpPut]
        public async Task<ActionResult> GradeHomework(
            int studentId,
            int lessonId,
            int grade)
        {
            await Invoker.InvokeAsync(
                _teacherService.StudentExistsAsync(studentId));

            await Invoker.InvokeAsync(
                _teacherService.LessonExistsAsync(lessonId));

           StudentInLesson studentInLesson = 
                await Invoker.InvokeAsync(
                    _teacherService.StudentInLessonExistsAsync(
                        studentId, lessonId));

            await Invoker.InvokeAsync(
               _teacherService.HomeworkExistsByLessonIdAsync(lessonId));

           await Invoker.InvokeAsync(
              _teacherService.GetHomeworkProgressStatusAsync(
                  studentInLesson.Id));

            if (grade < 2 || grade > 5)
            {
                throw new HttpStatusException(
                    HttpStatusCode.BadRequest,
                    _options.Value[MessageKey.GradeNotValid.ToString()]);
            }

            await Invoker.InvokeAsync(
                _teacherService.UpdateGradeHomeworkAsync(
                    studentId, lessonId, grade));

            GradeHomeworkEvent @event = new GradeHomeworkEvent(
                studentId, lessonId, grade);

            try
            {
                _eventBus.Publish(@event);
            }
            catch(Exception ex) 
            { 
                _logger.LogIntegrationEventError(@event, ex);
            }
           
            return Ok();
        }

        /// <summary>
        /// Создает замечание по уроку.
        /// </summary>
        /// <param name="studentId">Идентификатор студента.</param>
        /// <param name="lessonId">Идентификатор урока.</param>
        /// <param name="comment">Замечание.</param>
        /// <returns>Успешное выполнение операции.</returns>
        /// <remarks>
        /// Создает замечание по уроку. Необходимо проверить наличие студента, урока и 
        /// присутствие студента на уроке.
        /// </remarks>
        /// <response code="200">New comment is create.</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not found student or lesson or studentInLesson.</response>
        /// <response code="500">Internal server error</response>
        [HttpPut]
        public async Task<ActionResult> CreateComment(
            int studentId,int lessonId, string comment)
        {
            await Invoker.InvokeAsync(
                _teacherService.StudentExistsAsync(studentId));

            await Invoker.InvokeAsync(
                _teacherService.LessonExistsAsync(lessonId));

            await Invoker.InvokeAsync(
                _teacherService.StudentInLessonExistsAsync(
                    studentId, lessonId));

            ResultHandler.CheckString(comment);

            await Invoker.InvokeAsync(_teacherService.UpdateCommentAsync(
                studentId, lessonId, comment));

            CreateCommentEvent @event = new CreateCommentEvent(
                studentId, lessonId, comment);

            try
            {
                _eventBus.Publish(new CreateCommentEvent(
                    studentId, lessonId, comment));
            }
            catch(Exception ex)
            {
                _logger.LogIntegrationEventError(@event, ex);
            }
 
            return Ok();
        }

        /// <summary>
        /// Оценивает студента на уроке.
        /// </summary>
        /// <param name="studentId">Идентификатор студента.</param>
        /// <param name="lessonId">Идентификатор урока.</param>
        /// <param name="grade">Оценка.</param>
        /// <returns>Успешное выполнение урока.</returns>
        /// <remarks>
        /// Оценивает студента на уроке. 
        /// Необходимо наличие студента, урока и присутствие студента на уроке.
        /// Оценка не может быть меньше 2 и больше 5.
        /// </remarks>
        /// <response code="200">New comment is create.</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not found student or lesson or studentInLesson.</response>
        /// <response code="400">grade less 2 or grade more 5.</response>
        /// <response code="500">Internal server error</response>
        [HttpPut]
        public async Task<ActionResult> GradeStudentInLesson(
            int studentId, int lessonId, int grade)
        {
            await Invoker.InvokeAsync(
                _teacherService.StudentExistsAsync(studentId));

            await Invoker.InvokeAsync(
                _teacherService.LessonExistsAsync(lessonId));

            await Invoker.InvokeAsync(
                _teacherService.StudentInLessonExistsAsync(
                    studentId, lessonId));

            if (grade < 2 || grade > 5)
            {
                throw new HttpStatusException(
                    HttpStatusCode.BadRequest,
                    _options.Value[MessageKey.GradeNotValid.ToString()]);
            }

            await Invoker.InvokeAsync(
                _teacherService.UpdateGradeStudentInLessonAsync(
                    studentId, lessonId, grade));

            GradeStudentInLessonEvent @event = new GradeStudentInLessonEvent(
                studentId, lessonId, grade);

            try
            {
                _eventBus.Publish(new GradeStudentInLessonEvent(
                    studentId, lessonId, grade));
            }
            catch (Exception ex)
            {
                _logger.LogIntegrationEventError(@event, ex);
            }

            return Ok();
        }

        /// <summary>
        /// Удаляет встречу учителя и родителя.
        /// </summary>
        /// <param name="teacherParentMeetingId">Идентификатор встречи учителя и родителя.</param>
        /// <returns>Успешное выполнение операции.</returns>
        /// <remarks>
        /// Удаляет встречу учителя и родителя.
        /// Необходимо наличие идентификатора встречи.
        /// </remarks>
        /// <response code="200">TeacherParentMeeting is delete.</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not found teacherParentMeeting.</response>
        /// <response code="500">Internal server error</response>
        [HttpDelete]
        public async Task<ActionResult> RemoveTeacherParentMeeting(
            int teacherParentMeetingId)
        {
            await Invoker.InvokeAsync(
                _teacherService.TeacherParentMeetingExists(
                    teacherParentMeetingId));

            await Invoker.InvokeAsync(
                _teacherService.RemoveTeacherParentMeeting(
                    teacherParentMeetingId));

            RemoveTeacherParentMeetingEvent @event = 
                new RemoveTeacherParentMeetingEvent(teacherParentMeetingId);

            try
            {
                _eventBus.Publish(new RemoveTeacherParentMeetingEvent(
                    teacherParentMeetingId));
            }
            catch (Exception ex)
            {
                _logger.LogIntegrationEventError(@event, ex);
            }

            return Ok();
        }

        #endregion Methods
    }
}
