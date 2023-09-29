using Common.Api.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using TeacherApi.Data;
using TeacherApi.Data.Specifications;
using TeacherApi.IntegrationEvents.Events;
using TeacherApi.Models;

namespace TeacherApi.IntegrationEvents.Services
{
    /// <summary>
    /// Сервис учителя для событий интеграции.
    /// </summary>
    public class TeacherIntegrationEventService : ITeacherIntegrationEventService
    {
        #region Fields

        /// <summary>
        /// Логгер.
        /// </summary>
        private readonly ILogger<TeacherIntegrationEventService> _logger;

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
        /// Репозиторий для сущности встречи родителя и учителя.
        /// </summary>
        private IRepository<TeacherParentMeeting> _teacherParentMeetingRepo;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="logger">Логгер.</param>
        /// <param name="parentRepo">Репозиторий для сущности родителя.</param>
        /// <param name="teacherRepo">Репозиторий для сущности учителя.</param>
        /// <param name="studentRepo">Репозиторий для сущности студента.</param>
        /// <param name="teacherParentMeetingRepo">Репозиторий для сущности встречи родителя и учителя.</param>
        public TeacherIntegrationEventService(
            ILogger<TeacherIntegrationEventService> logger,
            IRepository<Parent> parentRepo,
            IRepository<Teacher> teacherRepo,
            IRepository<Student> studentRepo,
            IRepository<TeacherParentMeeting> teacherParentMeetingRepo)
        {
            _parentRepo = parentRepo;
            _teacherRepo = teacherRepo;
            _studentRepo = studentRepo;
            _teacherParentMeetingRepo = teacherParentMeetingRepo;
            _logger = logger;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Создание встречи родителя и учителя.
        /// </summary>
        /// <param name="event">Событие интеграции.</param>
        /// <returns>Результат выполнения операции.</returns>
        public async Task CreateParentTeacherMeetingAsync(CreateParentTeacherMeetingEvent @event)
        {
            _logger.LogIntegrationEventStart(@event);

            try
            {
                Parent parent = await _parentRepo.GetItemAsync(
                    new ParentSpecification(@event.ParentId));

                Student student = await _studentRepo.GetItemAsync(
                    new StudentSpecification(@event.StudentId));

                Teacher teacher = await _teacherRepo.GetItemAsync(
                    new TeacherSpecification(student.Id));

                TeacherParentMeeting teacherParentMeeting = new TeacherParentMeeting()
                {
                    Parent = parent,
                    Student = student,
                    Teacher = teacher,
                    MeetingDT = @event.MeetingDT,
                    TeacherInitiative = false
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
        /// Удаление встречи родителя и учителя.
        /// </summary>
        /// <param name="event">Событие интеграции.</param>
        /// <returns>Результат выполнения операции.</returns>
        public async Task RemoveParentTeacherMeetingAsync(RemoveParentTeacherMeetingEvent @event)
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
