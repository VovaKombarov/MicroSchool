using System.Net;

namespace Common.ErrorResponse
{
    /// <summary>
    /// Исключение типа HttpStatus.
    /// </summary>
    public class HttpStatusException : Exception
    {
        #region Properties

        /// <summary>
        /// Код статуса http.
        /// </summary>
        public HttpStatusCode Status { get; private set; }

        /// <summary>
        /// Сообщение.
        /// </summary>
        public new string? Message { get; private set; }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="status">Код статуса http.</param>
        public HttpStatusException(
            HttpStatusCode status) : base()
        {
            Status = status;
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="status">Код статуса http.</param>
        /// <param name="message">Сообщение.</param>
        public HttpStatusException(
            HttpStatusCode status,
            string message) : base()
        {
            Status = status;
            Message = message;
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="status">Код статуса http.</param>
        /// <param name="message">Сообщение.</param>
        /// <param name="innerException">внутреннее исключение.</param>
        public HttpStatusException(
            HttpStatusCode status, 
            string message, 
            Exception innerException) 
                : base(message, innerException)
        {
            Status = status;
        }

        #endregion Constructors
    }
}
