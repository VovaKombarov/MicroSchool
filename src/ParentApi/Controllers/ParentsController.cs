using AutoMapper;
using Common.Api;
using Common.Api.Extensions;
using Common.ErrorResponse;
using Common.EventBus;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ParentApi.Data.Dtos;
using ParentApi.IntegrationEvents.Events;
using ParentApi.Models;
using ParentApi.Services;
using ParentApi.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ParentApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class ParentsController : Controller
    {
        #region Fields

        /// <summary>
        /// Обьект маппера.
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Сервис родителя.
        /// </summary>
        private readonly IParentService _parentService;

        /// <summary>
        /// Набор опций.
        /// </summary>
        private readonly IOptions<Dictionary<string, string>> _options;

        /// <summary>
        /// Брокер.
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
        /// <param name="mapper">Обьект маппера.</param>
        /// <param name="parentService">Сервис родителя.</param>
        /// <param name="eventBus">Брокер.</param>
        /// <param name="options">Набор опций.</param>
        /// <param name="logger">Логгер.</param>
        public ParentsController(
            IMapper mapper, 
            IParentService parentService, 
            IEventBus eventBus,
            IOptions<Dictionary<string, string>> options, 
            ILogger logger)
        {
            _mapper = mapper;
            _parentService = parentService;
            _options = options; 
            _eventBus = eventBus;
            _logger = logger;

            Invoker.InitLogger(_logger);
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Возвращает коллекцию замечаний.
        /// </summary>
        /// <param name="studentId">Идентификатор студента.</param>
        /// <returns>Коллекцию замечаний.</returns>
        /// <remarks>
        /// Возвращает коллекцию замечаний по идентификатору студента. 
        /// </remarks>
        /// <response code="200">Returns a list of comments</response>
        /// <response code="400">Not found student or studentInLesson</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Empty collection</response>
        /// <response code="500">Internal server error</response> 

        [HttpGet]
        public async Task<ActionResult<List<CommentReadDto>>> GetComments(
           int studentId)
        {
            await Invoker.InvokeAsync(
                _parentService.StudentExistsAsync(studentId));

            List<StudentInLesson> studentInLessons =
                await Invoker.InvokeAsync(_parentService.GetStudentInLessonsAsync(
                    studentId));

            if (studentInLessons.All(w => string.IsNullOrEmpty(w.Comment)))
            {
                throw new HttpStatusException(
                    HttpStatusCode.BadRequest,
                    _options.Value[MessageKey.CommentMissing.ToString()]);
            }

           List<CommentReadDto> commentReadDtos = 
                _mapper.Map<List<CommentReadDto>>(studentInLessons
                    .Where(w => !string.IsNullOrEmpty(w.Comment))
                    .ToList());

            return new List<CommentReadDto>(commentReadDtos);
        }

        /// <summary>
        /// Возвращает замечание.
        /// </summary>
        /// <param name="studentId">Идентификатор студента.</param>
        /// <param name="lessonId">Идентификатор урока.</param>
        /// <returns>Замечание.</returns>
        /// <remarks>
        /// Возвращает замечание по идентификатору студента и идентификатору урока. 
        /// </remarks>
        /// <response code="200">Returns comment.</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not found student or studentInLesson or lesson.</response>
        /// <response code="400">Comment is null or empty.</response>
        /// <response code="500">Internal server error</response> 
        [HttpGet]
        public async Task<ActionResult<CommentReadDto>> GetComment(
            int studentId, int lessonId)
        {
            await Invoker.InvokeAsync(
                _parentService.StudentExistsAsync(studentId));

            await Invoker.InvokeAsync(
                _parentService.LessonExistsAsync(studentId));

            StudentInLesson studentInLesson = 
                await Invoker.InvokeAsync(
                    _parentService.StudentInLessonExistsAsync(
                        studentId, lessonId));

            if (String.IsNullOrEmpty(studentInLesson.Comment))
            {
                throw new HttpStatusException(
                    HttpStatusCode.BadRequest,
                    _options.Value[MessageKey.CommentMissing.ToString()]);
            }

            return Ok(_mapper.Map<CommentReadDto>(studentInLesson));
        }

        /// <summary>
        /// Возвращает оценку.
        /// </summary>
        /// <param name="studentId">Идентификатор студента.</param>
        /// <param name="lessonId">Идентификатор урока.</param>
        /// <returns>Возвращает оценку.</returns>
        /// <remarks>
        /// Возвращает оценку по идентификатору студента и идентификатору урока. 
        /// </remarks>
        /// <response code="200">Returns grade.</response>
        /// <response code="404">Not found student or studentInLesson or lesson or completedHomework.</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="400">Grade is null or grade less 2 or grade more 5.</response>
        /// <response code="500">Internal server error</response> 
        [HttpGet]
        public async Task<ActionResult<int>> GetGrade(
            int studentId, int lessonId)
        {
          await Invoker.InvokeAsync(
               _parentService.StudentExistsAsync(studentId));

            await Invoker.InvokeAsync(
                _parentService.LessonExistsAsync(lessonId));

            await Invoker.InvokeAsync(
                _parentService.StudentInLessonExistsAsync(studentId, lessonId));

            CompletedHomework completedHomework =
                await Invoker.InvokeAsync(
                    _parentService.GetCompletedHomeworkAsync(studentId, lessonId));

            if (completedHomework.Grade == null)
            {
                throw new HttpStatusException(
                    HttpStatusCode.BadRequest, _options.Value[
                        MessageKey.NoRating.ToString()]);
            }

            if (completedHomework.Grade < 2 || completedHomework.Grade > 5)
            {
                throw new HttpStatusException(HttpStatusCode.BadRequest,
                    _options.Value[MessageKey.GradeNotValid.ToString()]);
            }

            return Ok(completedHomework.Grade);
        }

        /// <summary>
        /// Возвращает оценки.
        /// </summary>
        /// <param name="studentId">Идентификатор студента.</param>
        /// <param name="subjectId">Идентификатор предмета.</param>
        /// <returns>Коллекцию оценок.</returns>
        /// <remarks>
        /// Возвращает коллекцию оценок. 
        /// </remarks>
        /// <response code="200">Returns a list of grades.</response>
        /// <response code="404">Not found student or subject.</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="400">Grade is null or grade less 2 or grade more 5.</response>
        /// <response code="500">Internal server error</response> 
        [HttpGet]
        public async Task<ActionResult<List<int>>> GetGrades(
           int studentId, int subjectId)
        {
            await Invoker.InvokeAsync(_parentService.StudentExistsAsync(studentId));

            await Invoker.InvokeAsync(_parentService.SubjectExistsAsync(subjectId));

            List<int> grades = await Invoker.InvokeAsync(
                _parentService.GetGradesAsync(studentId, subjectId));

            return Ok(grades);
        }

        /// <summary>
        /// Создает встречу родителя и учителя.
        /// </summary>
        /// <param name="parentId">Идентификатор родителя.</param>
        /// <param name="teacherId">Идентификатор учителя.</param>
        /// <param name="studentId">Идентификатор студента.</param>
        /// <param name="meetingDateTime">Время встречи.</param>
        /// <returns>Успешное выполнение операции.</returns>
        /// <remarks>
        /// Создает новую встречу учителя и родителя. Время встречи должно быть меньше текущего.
        /// Если встреча для конкретного учителя, конкретного родителя по конкретному студенту уже существует, 
        /// то новую встречу создать не получится.
        /// </remarks>
        /// <response code="200">Create new teacherParentMeeting.</response>
        /// <response code="404">Not found student or teacher or parent.</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="400">MeetingDateTime is less DateTime.Now or teacherParentsMeeting exists.</response>
        /// <response code="500">Internal server error</response> 
        [HttpPost]
        public async Task<ActionResult> CreateParentTeacherMeeting(
            int parentId, 
            int teacherId, 
            int studentId, 
            DateTime meetingDateTime)
        {
            await Invoker.InvokeAsync(
                _parentService.StudentExistsAsync(studentId));

            await Invoker.InvokeAsync(
                _parentService.TeacherExistsAsync(teacherId));

            await Invoker.InvokeAsync(
                _parentService.ParentExistsAsync(parentId));

            if (meetingDateTime < DateTime.Now)
            {
                throw new HttpStatusException(
                    HttpStatusCode.BadRequest,
                    _options.Value[MessageKey.DateTimeNotValid.ToString()]);
            }

            TeacherParentMeeting meeting = 
                await Invoker.InvokeWithoutCheckResultAsync(
                    _parentService.TeacherParentMeetingExistsAsync(
                        parentId, teacherId, studentId, meetingDateTime));

            if (meeting != null)
            {
                throw new HttpStatusException(
                    HttpStatusCode.BadRequest,
                    _options.Value[MessageKey.DuplicateItem.ToString()]);
            }

            await Invoker.InvokeAsync(
                _parentService.AddTeacherParentMeetingAsync(
                    studentId, teacherId, parentId, meetingDateTime));

            CreateParentTeacherMeetingEvent @event =
                new CreateParentTeacherMeetingEvent(
                    parentId, teacherId, studentId, meetingDateTime);

            try
            {
                _eventBus.Publish(@event);
            }
            catch (Exception ex)
            {
                _logger.LogIntegrationEventError(@event, ex);
            }

            return Ok();
        }

        /// <summary>
        /// Удаляет встречу родителя и учителя.
        /// </summary>
        /// <param name="teacherParentMeetingId">
        /// Идентификатор встречи учителя и родителя.</param>
        /// <returns>Успешное выполнение операции.</returns>
        /// <remarks>
        /// Удаляет встречу учителя и родителя.
        /// </remarks>
        /// <response code="200">Remove teacherParentMeeting.</response>
        /// <response code="404">Not found teacherParentMeeting.</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal server error</response> 
        [HttpDelete]
        public async Task<ActionResult> RemoveParentTeacherMeeting(
            int teacherParentMeetingId)
        {
            await Invoker.InvokeAsync(
                _parentService.TeacherParentMeetingExistsAsync(
                    teacherParentMeetingId));

            await Invoker.InvokeAsync(
                _parentService.RemoveParentTeacherMeetingAsync(
                    teacherParentMeetingId));

            RemoveTeacherParentMeetingEvent @event = 
                new RemoveTeacherParentMeetingEvent(teacherParentMeetingId);

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

        #endregion Methods
    }
}


