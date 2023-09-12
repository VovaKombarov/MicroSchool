using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Net;

namespace Common.TestsUtils
{
    public static class ActionResultConverter
    {
        #region Constants 

        private const string STATUS_CODE = "StatusCode";

        #endregion Constants

        #region Methods

        public static HttpStatusCode GetStatusCode<T>(ActionResult<T> actionResult)
        {
            IConvertToActionResult convertToActionResult = actionResult;
            var actionResultWithStatusCode = convertToActionResult.Convert() as IStatusCodeActionResult;
            return (HttpStatusCode)actionResultWithStatusCode?.StatusCode;
        }

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
