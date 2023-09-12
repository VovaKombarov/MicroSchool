using Common.ErrorResponse;
using System.Collections;
using System.Net;

namespace Common.Api
{
    /// <summary>
    /// Общий класс для проверки результата действия на null, коллекцию на пустоту, 
    /// строку на null или пустоту.
    /// </summary>
    public class ResultHandler
    {
        #region Constants 

        public const string ITEM_NOT_FOUND = "Элемент не найден!";

        public const string EMPTY_COLLECTION = "Пустая коллекция!";

        public const string EMPTY_STRING = "Пустая строка!";

        #endregion Constants 

        #region Methods

        /// <summary>
        /// Проверяет результат на null и коллекцию на пустоту.
        /// </summary>
        /// <typeparam name="T">Тип проверяемого результата.</typeparam>
        /// <param name="result">Результат.</param>
        /// <exception cref="HttpStatusException">В случае если результат равен null или коллекция пуста, 
        /// тогда прокидывает необходимое исключение.</exception>
        public static void CheckResult<T>(T result)
        {
            if (result == null)
            {
                throw new HttpStatusException(
                    HttpStatusCode.NotFound,
                    ITEM_NOT_FOUND);
            }

            var enumerable = result as IEnumerable;
            if (enumerable != null)
            {
                if (!enumerable.Cast<object>().Any())
                {
                    throw new HttpStatusException(
                        HttpStatusCode.OK,
                        EMPTY_COLLECTION);
                }
            }
        }

        /// <summary>
        /// Проверка строки на null или пустоту.
        /// </summary>
        /// <param name="str">Строка для проверки.</param>
        /// <exception cref="HttpStatusException">В случае если строка равна null или пуста, 
        /// прокидываем нужное исключение.</exception>
        public static void CheckString(string str)
        {
            if (String.IsNullOrEmpty(str))
            {
                throw new HttpStatusException(
                   HttpStatusCode.BadRequest,
                   EMPTY_STRING);
            }
        }

        #endregion Methods
    }
}
