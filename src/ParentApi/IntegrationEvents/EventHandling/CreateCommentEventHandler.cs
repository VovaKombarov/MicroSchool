using Common.EventBus;
using ParentApi.IntegrationEvents.Events;
using ParentApi.IntegrationEvents.Services;
using System.Threading.Tasks;

namespace ParentApi.IntegrationEvents.EventHandling
{
    /// <summary>
    /// Обработчик события создания замечания.
    /// </summary>
    public class CreateCommentEventHandler : 
        IIntegrationEventHandler<CreateCommentEvent>
    {
        #region Fields

        /// <summary>
        /// Обьект сервиса событий интеграций.
        /// </summary>
        private readonly IParentIntegrationEventService _parentIntegrationEventService;

        #endregion Fields

        #region Constructors 

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="parentIntegrationEventService">Обьект сервиса событий интеграций.</param>
        public CreateCommentEventHandler(
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
        public async Task Handle(CreateCommentEvent @event)
        {
            await _parentIntegrationEventService.CreateCommentAsync(@event);
        }

        #endregion Methods
    }
}
