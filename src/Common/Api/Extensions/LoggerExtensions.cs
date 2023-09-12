using Common.EventBus;
using Microsoft.Extensions.Logging;

namespace Common.Api.Extensions
{
    /// <summary>
    /// Расширяет возможности логгера.
    /// Основная задача, улучшение логгирования событий интеграции. 
    /// </summary>
    public static class LoggerExtensions
    {
        #region Methods 

        /// <summary>
        /// Логгирование начала события интеграции. 
        /// </summary>
        /// <param name="logger">Логгер.</param>
        /// <param name="event">Событие интеграции.</param>
        public static void LogIntegrationEventStart(
           this ILogger logger,
           IntegrationEvent @event)
        {
            logger.LogInformation(
                $"Обработка события интеграции: {@event.Id} - {@event}");
        }

        /// <summary>
        /// Логгирование успешного выполнения события интеграции.
        /// </summary>
        /// <param name="logger">Логгер.</param>
        /// <param name="event">Событие интеграции.</param>
        public static void LogIntegrationEventSuccess(
          this ILogger logger,
          IntegrationEvent @event)
        {
            logger.LogInformation(
                $"Событие интеграции успешно: {@event.Id} - {@event}");
        }

        /// <summary>
        /// Логгирование при ошибке в событии интеграции.
        /// </summary>
        /// <param name="logger">Логгер.</param>
        /// <param name="event">Событие интеграции.</param>
        /// <param name="exception">Исключение.</param>
        public static void LogIntegrationEventError(
            this ILogger logger,
            IntegrationEvent @event,
            Exception exception)
        {
            logger.LogError(
                $"Ошибка в cобытие интеграции {@event.Id} - {@event} ", exception);
        }

        /// <summary>
        /// Логгирование исключения.
        /// </summary>
        /// <param name="logger">Логгер.</param>
        /// <param name="exception">Исключение.</param>
        public static void LogException(this ILogger logger, Exception exception)
        {
            logger.LogError(
                exception.Message, exception.InnerException, exception.StackTrace);
        }

        #endregion Methods
    }
}
