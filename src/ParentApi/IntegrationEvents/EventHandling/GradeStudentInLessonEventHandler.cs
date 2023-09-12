using Common.EventBus;
using ParentApi.IntegrationEvents.Events;
using ParentApi.IntegrationEvents.Services;
using System.Threading.Tasks;

namespace ParentApi.IntegrationEvents.EventHandling
{
    public class GradeStudentInLessonEventHandler : IIntegrationEventHandler<GradeStudentInLessonEvent>
    {
        private readonly IParentIntegrationEventService _parentIntegrationEventService;

        public GradeStudentInLessonEventHandler(
            IParentIntegrationEventService parentIntegrationEventService)
        {
            _parentIntegrationEventService = parentIntegrationEventService;
        }

        public async Task Handle(GradeStudentInLessonEvent @event)
        {
            await _parentIntegrationEventService.GradeStudentInLessonAsync(@event);
        }
    }
   
}
