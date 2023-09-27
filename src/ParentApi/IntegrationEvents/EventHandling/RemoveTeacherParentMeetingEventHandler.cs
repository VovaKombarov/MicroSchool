using Common.EventBus;
using ParentApi.IntegrationEvents.Events;
using ParentApi.IntegrationEvents.Services;
using System.Threading.Tasks;

namespace ParentApi.IntegrationEvents.EventHandling
{
    /// <summary>
    /// Обработчик события удаления встречи учителя и родителя.
    /// </summary>
    public class RemoveTeacherParentMeetingEventHandler : IIntegrationEventHandler<RemoveTeacherParentMeetingEvent>
    {
        #region Fields

        /// <summary>
        /// Обьект сервиса интеграционных событий.
        /// </summary>
        private readonly IParentIntegrationEventService _parentIntegrationEventService;

        #endregion Fields

        #region Constructors 

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="parentIntegrationEventService">Обьект сервиса интеграционных событий.</param>
        public RemoveTeacherParentMeetingEventHandler(
          IParentIntegrationEventService parentIntegrationEventService)
        {
            _parentIntegrationEventService = parentIntegrationEventService;
        }

        #endregion Constructors

        #region Methods 

        /// <summary>
        /// Обработчик события.
        /// </summary>
        /// <param name="event">Событие интеграции.</param>
        /// <returns>Результат выполнения операции.</returns>
        public async Task Handle(RemoveTeacherParentMeetingEvent @event)
        {
            await _parentIntegrationEventService.RemoveTeacherParentMeetingAsync(@event);
        }

        #endregion Methods
    }
}
