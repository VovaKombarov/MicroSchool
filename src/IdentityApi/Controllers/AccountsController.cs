using Common.Api;
using Common.ErrorResponse;
using IdentityApi.Services;
using IdentityApi.Utilities;
using IdentityApi.ViewModels;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;

namespace IdentityApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AccountsController : ControllerBase
    {
        #region Fields 

        /// <summary>
        /// Сервис для Identity.
        /// </summary>
        private readonly IIdentityService _identityService;

        /// <summary>
        /// Словарь опций.
        /// </summary>
        private readonly IOptions<Dictionary<string, string>> _options;

        /// <summary>
        /// Логгер.
        /// </summary>
        private readonly ILogger _logger;

        #endregion Fields

        #region Constructors 

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="identityService">Сервис для Identity.</param>
        /// <param name="options">Словарь опций.</param>
        /// <param name="logger">Логгер.</param>
        public AccountsController(
            IIdentityService identityService,
            IOptions<Dictionary<string, string>> options,
            ILogger logger)
        {
            _identityService = identityService;
            _options = options;
            _logger = logger;

            Invoker.InitLogger(_logger);
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Регистрация.
        /// </summary>
        /// <param name="userViewModel">Данные пользователя.</param>
        /// <returns>Токен доступа.</returns>
        /// <remarks>
        ///  Если регистрация прошла успешно, возвращаем token.
        ///</remarks>
        /// <response code="200">Token.</response>
        /// <response code="404">Empty userViewModel or user exists.</response>
        /// <response code="500">Internal server error</response> 
        [HttpGet]
        public async Task<ActionResult<TokenResponse>> SignUp(
            UserViewModel userViewModel)
        {
           if(!_identityService.CheckUserViewModel(userViewModel))
           {
                throw new HttpStatusException(
                    HttpStatusCode.BadRequest,
                    _options.Value[MessageKey.EmptyValue.ToString()]);
            }

            if(await Invoker.InvokeWithoutCheckResultAsync(
                _identityService.UserExistsAsync(userViewModel)))
            {
                throw new HttpStatusException(
                    HttpStatusCode.BadRequest,
                    _options.Value[MessageKey.UserExists.ToString()]);
            };

            await Invoker.InvokeAsync(
                _identityService.CreateUserAsync(userViewModel));

            return await Invoker.InvokeWithoutCheckResultAsync(
                _identityService.GetTokenAsync(userViewModel));
        }

        /// <summary>
        /// Вход пользователя в систему.
        /// </summary>
        /// <param name="userViewModel">Данные пользователя.</param>
        /// <returns>Токен доступа.</returns>
        /// <remarks>
        ///  Если пользователь вошел в систему, возвращаем token.
        ///</remarks>
        /// <response code="200">Token.</response>
        /// <response code="404">Empty userViewModel or user not exists.</response>
        /// <response code="500">Internal server error</response> 
        [HttpGet]
        public async Task<ActionResult<TokenResponse>> SignIn(
            UserViewModel userViewModel)
        {
            if (!_identityService.CheckUserViewModel(userViewModel))
            {
                throw new HttpStatusException(
                    HttpStatusCode.BadRequest,
                    _options.Value[MessageKey.EmptyValue.ToString()]);
            }

            if (!await  Invoker.InvokeAsync(
                _identityService.UserExistsAsync(userViewModel)))
            {
                throw new HttpStatusException(
                    HttpStatusCode.BadRequest,
                   _options.Value[MessageKey.UserNotExists.ToString()]);
            };

            return await Invoker.InvokeWithoutCheckResultAsync(
                _identityService.GetTokenAsync(userViewModel));
        }

        /// <summary>
        /// Выход из системы.
        /// </summary>
        /// <returns>Ответ на отзыв токена.</returns>
        /// <remarks>
        ///  Если пользователь вышел из системы, отзываем token.
        ///</remarks>
        /// <response code="200">TokenRevocationResponse.</response>
        /// <response code="500">AccessToken is null or Internal server error</response> 
        [HttpPost]
        public new async Task<ActionResult<TokenRevocationResponse>> SignOut()
        {
           string accessToken = 
                _identityService.GetTokenFromRequestHeaders(HttpContext);
    
            if (accessToken == null)
            {
                throw new HttpStatusException(
                    HttpStatusCode.InternalServerError);
            }

            return await _identityService.RevokeToken(accessToken);
        }

        #endregion Methods
    }
}
