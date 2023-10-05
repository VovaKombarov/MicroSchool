using ParentApi.IntegrationEvents.Events;
using System.Threading.Tasks;

namespace ParentApi.IntegrationEvents.Services
{
    /// <summary>
    /// Интерфейс для сервиса интеграции родителя.
    /// </summary>
    public interface IParentIntegrationEventService 
    {
        /// <summary>
        /// Асинхронное создание урока.
        /// </summary>
        /// <param name="event">Событие интеграции.</param>
        /// <returns>Результат выполнения операции.</returns>
        Task CreateLessonAsync(CreateLessonEvent @event);

        /// <summary>
        /// Асинхронное создание встречи родителя и учителя.
        /// </summary>
        /// <param name="event">Событие интеграции.</param>
        /// <returns>Результат выполнения операции.</returns>
        Task CreateTeacherParentMeetingAsync(CreateTeacherParentMeetingEvent @event);

        /// <summary>
        /// Асинхронное создание домашней работы.
        /// </summary>
        /// <param name="event">Событие интеграции.</param>
        /// <returns>Результат выполнения операции.</returns>
        Task CreateHomeworkAsync(CreateHomeworkEvent @event);

        /// <summary>
        /// Асинхронное изменение статуса домашней работы.
        /// </summary>
        /// <param name="event">Событие интеграции.</param>
        /// <returns>Результат выполнения операции.</returns>
        Task ChangeStatusHomeworkAsync(ChangeStatusHomeworkEvent @event);

        /// <summary>
        /// Асинхронное оценивание домашней работы.
        /// </summary>
        /// <param name="event">Событие интеграции.</param>
        /// <returns>Результат выполнения операции.</returns>
        Task GradeHomeworkAsync(GradeHomeworkEvent @event);

        /// <summary>
        /// Асинхронное создание замечания.
        /// </summary>
        /// <param name="event">Событие интеграции.</param>
        /// <returns>Результат выполнения операции.</returns>
        Task CreateCommentAsync(CreateCommentEvent @event);

        /// <summary>
        /// Асинхронное оценивание студента.
        /// </summary>
        /// <param name="event">Событие интеграции.</param>
        /// <returns>Результат выполнения операции.</returns>
        Task GradeStudentInLessonAsync(GradeStudentInLessonEvent @event);

        /// <summary>
        /// Асинхронное удаление встречи учителя и родителя.
        /// </summary>
        /// <param name="event">Событие интеграции.</param>
        /// <returns>Результат выполнения операции.</returns>
        Task RemoveTeacherParentMeetingAsync(RemoveTeacherParentMeetingEvent @event);
    }
}

