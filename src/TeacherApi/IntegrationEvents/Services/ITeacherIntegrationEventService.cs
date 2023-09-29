using System.Threading.Tasks;
using TeacherApi.IntegrationEvents.Events;

namespace TeacherApi.IntegrationEvents.Services
{
    /// <summary>
    /// Сервис учителя для интеграционных событий.
    /// </summary>
    public interface ITeacherIntegrationEventService
    {
        /// <summary>
        /// Создание встречи родителя и учителя.
        /// </summary>
        /// <param name="event">Событие интеграции.</param>
        /// <returns>Результат выполнения операции.</returns>
        Task CreateParentTeacherMeetingAsync(CreateParentTeacherMeetingEvent @event);

        /// <summary>
        /// Удаление встречи родителя и учителя.
        /// </summary>
        /// <param name="event">Событие интеграции</param>
        /// <returns>Результат выполнения операции.</returns>
        Task RemoveParentTeacherMeetingAsync(RemoveParentTeacherMeetingEvent @event);
    }
}
