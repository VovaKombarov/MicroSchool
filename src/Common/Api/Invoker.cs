using Common.Api.Extensions;
using Common.ErrorResponse;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Common.Api
{
    /// <summary>
    /// Общая обертка для всех асинхронных вызовов на контроллерах. 
    /// Данный класс нужен. 
    /// 1) Унифицированная проверка результата, 
    /// 2) Если ошибка, логгируем ошибку. 
    /// 3) Прокидываем нужное исключение.
    /// </summary>
    public static class Invoker
    {
        #region Fields

        /// <summary>
        /// Обьект логгера.
        /// </summary>
        private static ILogger _logger;

        #endregion Fields

        #region Methods

        /// <summary>
        /// Инициализация логгера.
        /// </summary>
        /// <param name="logger">Логгер.</param>
        public static void InitLogger(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Асинхронное выполнение метода.
        /// </summary>
        /// <param name="task">Асинхронная операция.</param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException">Необходимое исключение в случае ошибки.</exception>
        public static async Task InvokeAsync(Task task)
        {
            try
            {
                await task;
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw new HttpStatusException(
                    HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Асинхронное выполнение метода.
        /// </summary>
        /// <typeparam name="T">Обощенный тип результата.</typeparam>
        /// <param name="task">Асинхронная операция.</param>
        /// <returns>Результат выполнение асинхронной операции.</returns>
        /// <exception cref="HttpStatusException">Необходимое исключение в случае ошибки.</exception>
        public static async Task<T> InvokeAsync<T>(Task<T> task)
        {
            try
            {
                T result = await task;
                ResultHandler.CheckResult(result);
                return result;
            }
            catch (HttpStatusException ex)
            {
                _logger.LogException(ex);
                throw;
            }

            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw new HttpStatusException(
                    HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Асинхронное выполнение метода без проверки результата.
        /// </summary>
        /// <typeparam name="T">Обобщенный тип результата.</typeparam>
        /// <param name="task">Асинхронная операция.</param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException">Необходимое исключение в случае ошибки.</exception>
        public static async Task<T> InvokeWithoutCheckResultAsync<T>(
            Task<T> task)
        {
            try
            {
                return await task;
            }

            catch (Exception ex)
            {
                _logger.LogException(ex);
                throw new HttpStatusException(
                    HttpStatusCode.InternalServerError);
            }
        }

        #endregion Methods
    }
}
