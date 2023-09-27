using Common.EventBus;
using ParentApi.IntegrationEvents.Services;
using System.Threading.Tasks;

namespace ParentApi.IntegrationEvents
{
    /// <summary>
    /// Обработчик события создания встречи учителя и родителя.
    /// </summary>
    public class CreateTeacherParentMeetingIntegrationEventHandler : 
        IIntegrationEventHandler<CreateTeacherParentMeetingEvent>
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
        public CreateTeacherParentMeetingIntegrationEventHandler(
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
        public async Task Handle(CreateTeacherParentMeetingEvent @event)
        {
            await _parentIntegrationEventService.CreateTeacherParentMeetingAsync(@event);
        }

        #endregion Methods
    }
}
