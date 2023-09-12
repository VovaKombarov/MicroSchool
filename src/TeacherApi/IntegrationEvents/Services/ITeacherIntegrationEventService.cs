using System.Threading.Tasks;
using TeacherApi.IntegrationEvents.Events;

namespace TeacherApi.IntegrationEvents.Services
{
    public interface ITeacherIntegrationEventService
    {

        Task CreateParentTeacherMeetingAsync(CreateParentTeacherMeetingEvent @event);

        Task RemoveParentTeacherMeetingAsync(RemoveParentTeacherMeetingEvent @event);
    }
}
