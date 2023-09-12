using System.Net;

namespace Common.ErrorResponse
{
    public class HttpStatusException : Exception
    {
        #region Properties

        public HttpStatusCode Status { get; private set; }

        public new string? Message { get; private set; }

        #endregion Properties

        #region Constructors

        public HttpStatusException(
            HttpStatusCode status) : base()
        {
            Status = status;
        }

        public HttpStatusException(
            HttpStatusCode status,
            string message) : base()
        {
            Status = status;
            Message = message;
        }

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
