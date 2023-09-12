using Common.EventBus;
using System.Diagnostics;
using System.Threading.Tasks;
using TeacherApi.IntegrationEvents.Events;
using TeacherApi.IntegrationEvents.Services;

namespace TeacherApi.IntegrationEvents
{
    public class CreateParentTeacherMeetingIntegrationEventHandler :
        IIntegrationEventHandler<CreateParentTeacherMeetingEvent>
    {
        private readonly ITeacherIntegrationEventService _teacherIntegrationEventService;

        public CreateParentTeacherMeetingIntegrationEventHandler(
            ITeacherIntegrationEventService teacherIntegrationEventService)
        {
            _teacherIntegrationEventService = teacherIntegrationEventService;
        }

        public async Task Handle(CreateParentTeacherMeetingEvent @event)
        {
            await _teacherIntegrationEventService.CreateParentTeacherMeetingAsync(@event);

        }
    }
}
