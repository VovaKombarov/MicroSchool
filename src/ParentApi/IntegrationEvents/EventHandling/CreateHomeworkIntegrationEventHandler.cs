using Common.EventBus;
using ParentApi.IntegrationEvents.Services;
using System.Threading.Tasks;
using ParentApi.IntegrationEvents.Events;

namespace ParentApi.IntegrationEvents.EventHandling
{
    public class CreateHomeworkIntegrationEventHandler : IIntegrationEventHandler<CreateHomeworkEvent>
    {
        private readonly IParentIntegrationEventService _parentIntegrationEventService;

        public CreateHomeworkIntegrationEventHandler(
            IParentIntegrationEventService parentIntegrationEventService)
        {
            _parentIntegrationEventService = parentIntegrationEventService;
        }

        public async Task Handle(CreateHomeworkEvent @event)
        {
            await _parentIntegrationEventService.CreateHomeworkAsync(@event);
        }
}
}
