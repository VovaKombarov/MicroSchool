using System.Net;

namespace Common.ErrorResponse
{
    public class BaseErrorResponse
    {
        #region Properties 

        public HttpStatusCode Status { get; private set; }

        #endregion Properties

        #region Constructors

        public BaseErrorResponse(HttpStatusCode status)
        {
            Status = status;
        }

        #endregion Constructors
    }
}
