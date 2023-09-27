using Common.EventBus;
using ParentApi.IntegrationEvents.Events;
using ParentApi.IntegrationEvents.Services;
using System.Threading.Tasks;

namespace ParentApi.IntegrationEvents.EventHandling
{
    /// <summary>
    /// Обработчик события оценивания домащней работы.
    /// </summary>
    public class GradeHomeworkEventHandler : IIntegrationEventHandler<GradeHomeworkEvent>
    {
        #region Fields

        private readonly IParentIntegrationEventService _parentIntegrationEventService;

        #endregion Fields

        #region Constructors 

        public GradeHomeworkEventHandler(
            IParentIntegrationEventService parentIntegrationEventService)
        {
            _parentIntegrationEventService = parentIntegrationEventService;
        }

        #endregion Constructors

        #region Methods 

        public async Task Handle(GradeHomeworkEvent @event)
        {
            await _parentIntegrationEventService.GradeHomeworkAsync(@event);
        }

        #endregion Methods
    }
}
