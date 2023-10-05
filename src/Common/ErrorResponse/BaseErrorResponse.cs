using System.Net;

namespace Common.ErrorResponse
{
    /// <summary>
    /// Базовый ответ на ошибку.
    /// </summary>
    public class BaseErrorResponse
    {
        #region Properties 

        /// <summary>
        /// Статус http.
        /// </summary>
        public HttpStatusCode Status { get; private set; }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="status">Статус http.</param>
        public BaseErrorResponse(HttpStatusCode status)
        {
            Status = status;
        }

        #endregion Constructors
    }
}
