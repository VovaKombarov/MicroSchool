using Common.EventBus;
using ParentApi.IntegrationEvents.Events;
using ParentApi.IntegrationEvents.Services;
using System.Threading.Tasks;

namespace ParentApi.IntegrationEvents.EventHandling
{
    public class ChangeStatusHomeworkEventHandler : IIntegrationEventHandler<ChangeStatusHomeworkEvent>
    {
        private readonly IParentIntegrationEventService _parentIntegrationEventService;

        public ChangeStatusHomeworkEventHandler(
            IParentIntegrationEventService parentIntegrationEventService)
        {
            _parentIntegrationEventService = parentIntegrationEventService;
        }

        public async Task Handle(ChangeStatusHomeworkEvent @event)
        {
            await _parentIntegrationEventService.ChangeStatusHomeworkAsync(@event);
        }
    }
}
