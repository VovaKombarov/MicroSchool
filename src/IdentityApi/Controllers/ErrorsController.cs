using Common.ErrorResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;


namespace IdentityApi.Controllers
{
    [AllowAnonymous]
    // Необходим, иначе ломается swagger
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorsController : ControllerBase
    {
        #region Constants

        private const string INTERNAL_SERVER_ERROR = "Внутренняя ошибка сервера";

        #endregion Constants

        #region Utilities

        private BaseErrorResponse _ErrorResponseOnHttpStatusException(
            HttpStatusException httpStatusException)
        {
            if (httpStatusException.Message != null)
            {
                return new ErrorResponse(
                    httpStatusException.Status,
                    httpStatusException.Message);
            }

            return new BaseErrorResponse(httpStatusException.Status);
        }

        #endregion Utilities

        #region Methods

        [Route("/error")]
        public BaseErrorResponse Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = context?.Error;

            HttpStatusException httpStatusException = exception as HttpStatusException;
            if (httpStatusException != null)
            {
                return _ErrorResponseOnHttpStatusException(httpStatusException);
            }

            return new ErrorResponse(
                HttpStatusCode.InternalServerError,
                INTERNAL_SERVER_ERROR);
        }

        #endregion Methods
    }
}
