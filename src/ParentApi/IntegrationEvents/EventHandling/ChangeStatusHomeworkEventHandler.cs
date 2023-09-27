using Common.EventBus;
using ParentApi.IntegrationEvents.Events;
using ParentApi.IntegrationEvents.Services;
using System.Threading.Tasks;

namespace ParentApi.IntegrationEvents.EventHandling
{
    /// <summary>
    /// Обработчик события изменения статуса домашней работы.
    /// </summary>
    public class ChangeStatusHomeworkEventHandler : 
        IIntegrationEventHandler<ChangeStatusHomeworkEvent>
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
        public ChangeStatusHomeworkEventHandler(
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
        public async Task Handle(ChangeStatusHomeworkEvent @event)
        {
            await _parentIntegrationEventService.ChangeStatusHomeworkAsync(@event);
        }

        #endregion Methods
    }
}
