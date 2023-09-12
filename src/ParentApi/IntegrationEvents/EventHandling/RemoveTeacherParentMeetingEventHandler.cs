using Common.EventBus;
using ParentApi.IntegrationEvents.Events;
using ParentApi.IntegrationEvents.Services;
using System.Threading.Tasks;

namespace ParentApi.IntegrationEvents.EventHandling
{
    public class RemoveTeacherParentMeetingEventHandler : IIntegrationEventHandler<RemoveTeacherParentMeetingEvent>
    {
        private readonly IParentIntegrationEventService _parentIntegrationEventService;

        public RemoveTeacherParentMeetingEventHandler(
            IParentIntegrationEventService parentIntegrationEventService)
        {
            _parentIntegrationEventService = parentIntegrationEventService;
        }

        public async Task Handle(RemoveTeacherParentMeetingEvent @event)
        {
            await _parentIntegrationEventService.RemoveTeacherParentMeetingAsync(@event);
        }
    }
}
