using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Net;

namespace Common.TestsUtils
{
    /// <summary>
    /// Конвертер ActionResult.
    /// </summary>
    public static class ActionResultConverter
    {
        #region Constants 

        private const string STATUS_CODE = "StatusCode";

        #endregion Constants

        #region Methods

        /// <summary>
        /// Получает статус кода http.
        /// </summary>
        /// <typeparam name="T">Обощенный тип для результата.</typeparam>
        /// <param name="actionResult">Обьект ActionResult.</param>
        /// <returns>Статус кода http.</returns>
        public static HttpStatusCode GetStatusCode<T>(ActionResult<T> actionResult)
        {
            IConvertToActionResult convertToActionResult = actionResult;
            var actionResultWithStatusCode = convertToActionResult.Convert() as IStatusCodeActionResult;
            return (HttpStatusCode)actionResultWithStatusCode?.StatusCode;
        }

        /// <summary>
        /// Получает статус кода http.
        /// </summary>
        /// <param name="result">Результат запроса.</param>
        /// <returns>Статус кода http.</returns>
        public static HttpStatusCode GetStatusCode(IActionResult result)
        {
            return (HttpStatusCode)result
               .GetType()
               .GetProperty(STATUS_CODE)
               .GetValue(result, null);
        }

        #endregion Methods
    }
}
