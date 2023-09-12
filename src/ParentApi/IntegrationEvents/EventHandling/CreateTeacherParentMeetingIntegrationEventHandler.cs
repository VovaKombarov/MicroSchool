using Common.EventBus;
using ParentApi.IntegrationEvents.Services;
using System.Threading.Tasks;

namespace ParentApi.IntegrationEvents
{
    public class CreateTeacherParentMeetingIntegrationEventHandler : 
        IIntegrationEventHandler<CreateTeacherParentMeetingEvent>
    {
        private readonly IParentIntegrationEventService _parentIntegrationEventService;
        public CreateTeacherParentMeetingIntegrationEventHandler(
            IParentIntegrationEventService parentIntegrationEventService)
        {
            _parentIntegrationEventService = parentIntegrationEventService;
        }

        public async Task Handle(CreateTeacherParentMeetingEvent @event)
        {
            await _parentIntegrationEventService.CreateTeacherParentMeetingAsync(@event);
        }
    }
}
