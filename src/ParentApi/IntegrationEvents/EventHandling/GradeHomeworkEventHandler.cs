using Common.EventBus;
using ParentApi.IntegrationEvents.Events;
using ParentApi.IntegrationEvents.Services;
using System.Threading.Tasks;

namespace ParentApi.IntegrationEvents.EventHandling
{
    public class GradeHomeworkEventHandler : IIntegrationEventHandler<GradeHomeworkEvent>
    {
        private readonly IParentIntegrationEventService _parentIntegrationEventService;
        public GradeHomeworkEventHandler(
            IParentIntegrationEventService parentIntegrationEventService)
        {
            _parentIntegrationEventService = parentIntegrationEventService;
        }

        public async Task Handle(GradeHomeworkEvent @event)
        {
            await _parentIntegrationEventService.GradeHomeworkAsync(@event);
        }
    }
}
