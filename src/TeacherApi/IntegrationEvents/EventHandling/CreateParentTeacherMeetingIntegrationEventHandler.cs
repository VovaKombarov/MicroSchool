using Common.EventBus;
using System.Diagnostics;
using System.Threading.Tasks;
using TeacherApi.IntegrationEvents.Events;
using TeacherApi.IntegrationEvents.Services;

namespace TeacherApi.IntegrationEvents
{

    /// <summary>
    /// Обработчик события создания встречи родителя и учителя.
    /// </summary>
    public class CreateParentTeacherMeetingIntegrationEventHandler :
        IIntegrationEventHandler<CreateParentTeacherMeetingEvent>
    {
        #region Fields 

        /// <summary>
        /// Обьект сервиса для событий интеграций.
        /// </summary>
        private readonly ITeacherIntegrationEventService _teacherIntegrationEventService;

        #endregion Fields

        #region Constructors 

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="teacherIntegrationEventService"></param>
        public CreateParentTeacherMeetingIntegrationEventHandler(
           ITeacherIntegrationEventService teacherIntegrationEventService)
        {
            _teacherIntegrationEventService = teacherIntegrationEventService;
        }

        #endregion Constructors

        #region Methods 

        /// <summary>
        /// Обработчик.
        /// </summary>
        /// <param name="event">Событие интеграции.</param>
        /// <returns>Результат выполнения операции.</returns>
        public async Task Handle(CreateParentTeacherMeetingEvent @event)
        {
            await _teacherIntegrationEventService.CreateParentTeacherMeetingAsync(@event);
        }

        #endregion Methods
    }
}
