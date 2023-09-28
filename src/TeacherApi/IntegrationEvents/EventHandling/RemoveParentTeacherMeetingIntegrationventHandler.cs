using Common.EventBus;
using System.Threading.Tasks;
using TeacherApi.IntegrationEvents.Events;
using TeacherApi.IntegrationEvents.Services;

namespace TeacherApi.IntegrationEvents.EventHandling
{
    /// <summary>
    /// Обработчик события удаления встречи родителя и учителя.
    /// </summary>
    public class RemoveParentTeacherMeetingIntegrationEventHandler :
        IIntegrationEventHandler<RemoveParentTeacherMeetingEvent>
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
        public RemoveParentTeacherMeetingIntegrationEventHandler(
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
        public async Task Handle(RemoveParentTeacherMeetingEvent @event)
        {
            await _teacherIntegrationEventService.RemoveParentTeacherMeetingAsync(@event);
        }

        #endregion Methods
    }
}
