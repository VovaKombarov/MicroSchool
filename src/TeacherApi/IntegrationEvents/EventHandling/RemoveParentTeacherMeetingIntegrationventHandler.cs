using Common.EventBus;
using System.Threading.Tasks;
using TeacherApi.IntegrationEvents.Events;
using TeacherApi.IntegrationEvents.Services;

namespace TeacherApi.IntegrationEvents.EventHandling
{
    public class RemoveParentTeacherMeetingIntegrationEventHandler :
        IIntegrationEventHandler<RemoveParentTeacherMeetingEvent>
    {
        private readonly ITeacherIntegrationEventService _teacherIntegrationEventService;

        public RemoveParentTeacherMeetingIntegrationEventHandler(ITeacherIntegrationEventService teacherIntegrationEventService)
        {
            _teacherIntegrationEventService = teacherIntegrationEventService;
        }

        public async Task Handle(RemoveParentTeacherMeetingEvent @event)
        {
            await _teacherIntegrationEventService.RemoveParentTeacherMeetingAsync(@event);

        }
    }
}
