using Common.EventBus;
using ParentApi.IntegrationEvents.Events;
using ParentApi.IntegrationEvents.Services;
using System.Threading.Tasks;

namespace ParentApi.IntegrationEvents.EventHandling
{
    public class CreateLessonIntegrationEventHandler : IIntegrationEventHandler<CreateLessonEvent>
    {
        private readonly IParentIntegrationEventService _parentIntegrationEventService;

        public CreateLessonIntegrationEventHandler(
            IParentIntegrationEventService parentIntegrationEventService)
        {
            _parentIntegrationEventService = parentIntegrationEventService;
        }

        public async Task Handle(CreateLessonEvent @event)
        {
            await _parentIntegrationEventService.CreateLessonAsync(@event);
        }
    }
}
