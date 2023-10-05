using System.Net;

namespace Common.ErrorResponse
{
    /// <summary>
    /// Ответ в случае ошибки.
    /// </summary>
    public class ErrorResponse : BaseErrorResponse
    {
        #region Properties

        /// <summary>
        /// Сообщение.
        /// </summary>
        public string? Message { get; private set; }

        #endregion Properties

        #region Constructors 

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="status">Статус http.</param>
        /// <param name="message">Сообщение.</param>
        public ErrorResponse(
            HttpStatusCode status, 
            string message) 
                : base(status)
        {
            Message = message;
        }

        #endregion Constructors
    }
}
