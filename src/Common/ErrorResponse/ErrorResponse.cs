using System.Net;

namespace Common.ErrorResponse
{
    public class ErrorResponse : BaseErrorResponse
    {
        #region Properties

        public string? Message { get; private set; }

        #endregion Properties

        #region Constructors 

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
