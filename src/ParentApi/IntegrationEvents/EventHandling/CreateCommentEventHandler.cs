using Common.EventBus;
using ParentApi.IntegrationEvents.Events;
using ParentApi.IntegrationEvents.Services;
using System.Threading.Tasks;

namespace ParentApi.IntegrationEvents.EventHandling
{
    public class CreateCommentEventHandler : IIntegrationEventHandler<CreateCommentEvent>
    {
        private readonly IParentIntegrationEventService _parentIntegrationEventService;

        public CreateCommentEventHandler(
            IParentIntegrationEventService parentIntegrationEventService)
        {
            _parentIntegrationEventService = parentIntegrationEventService;
        }

        public async Task Handle(CreateCommentEvent @event)
        {
            await _parentIntegrationEventService.CreateCommentAsync(@event);
        }
    }
}
