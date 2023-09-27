using Common.EventBus;
using ParentApi.IntegrationEvents.Events;
using ParentApi.IntegrationEvents.Services;
using System.Threading.Tasks;

namespace ParentApi.IntegrationEvents.EventHandling
{
    /// <summary>
    /// Обработчик события создания урока.
    /// </summary>
    public class CreateLessonIntegrationEventHandler : IIntegrationEventHandler<CreateLessonEvent>
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
        public CreateLessonIntegrationEventHandler(
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
        public async Task Handle(CreateLessonEvent @event)
        {
            await _parentIntegrationEventService.CreateLessonAsync(@event);
        }

        #endregion Methods
    }
}
