using Common.EventBus;
using ParentApi.IntegrationEvents.Services;
using System.Threading.Tasks;
using ParentApi.IntegrationEvents.Events;

namespace ParentApi.IntegrationEvents.EventHandling
{
    /// <summary>
    /// Обработчик события создания домашней работы.
    /// </summary>
    public class CreateHomeworkIntegrationEventHandler : IIntegrationEventHandler<CreateHomeworkEvent>
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
        public CreateHomeworkIntegrationEventHandler(
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
        public async Task Handle(CreateHomeworkEvent @event)
        {
            await _parentIntegrationEventService.CreateHomeworkAsync(@event);
        }

        #endregion Methods

    }
}
