using Common.ErrorResponse;
using IdentityApi.Models;
using IdentityApi.ViewModels;
using IdentityModel.Client;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace IdentityApi.Services
{
    /// <summary>
    /// Реализация сервиса идентификации.
    /// </summary>
    public class IdentityService : IIdentityService
    {
        #region Fields

        /// <summary>
        /// Обьект управления пользователем.
        /// </summary>
        private readonly UserManager<User> _userManager;

        /// <summary>
        /// Обьект конфигурации.
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Обьект моделирующий ответ от конечной точки обнаружения OpenID Connect.
        /// </summary>
        private DiscoveryDocumentResponse _discDocument;

        /// <summary>
        /// Логгер.
        /// </summary>
        private readonly ILogger<IdentityService> _logger;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="userManager">Обьект управления пользователем.</param>
        /// <param name="configuration">Обьект конфигурации.</param>
        /// <param name="logger">Логгер.</param>
        public IdentityService(
            UserManager<User> userManager, 
            IConfiguration configuration,
            ILogger<IdentityService> logger)
        {
            _userManager = userManager;
            _configuration = configuration;
            _logger = logger;
        }

        #endregion Constructors

        #region Utilities

        /// <summary>
        /// Отзыв токена.
        /// </summary>
        /// <param name="accessToken">Токен доступа.</param>
        /// <returns>Ответ с отзывом токена.</returns>
        private async Task<TokenRevocationResponse> _RevokeToken(string accessToken)
        {
            var client = new HttpClient();

            var response = await client.RevokeTokenAsync(
                new TokenRevocationRequest
                {
                    Address = _configuration["IdentityServer:revocation_endpoint"],
                    ClientId = _configuration["ClientIds:Teacher"],
                    ClientSecret = _configuration["Secret:Secret"],
                    Token = accessToken
                });

            return response;
        }

        /// <summary>
        /// Mapping обьект UserViewModel на User.
        /// </summary>
        /// <param name="userViewModel">Пользователь.</param>
        /// <returns>Пользователь типа User.</returns>
        private User _CreateUser(UserViewModel userViewModel)
        {
            return new User()
            {
                Email = userViewModel.Email,
                UserName = userViewModel.Name
            };
        }

        #endregion Utilities

        #region Methods 

        /// <summary>
        /// Отзыв токена.
        /// </summary>
        /// <param name="accessToken">Токен доступа.</param>
        /// <returns>Ответ при отзыве токена.</returns>
        public async Task<TokenRevocationResponse> RevokeToken(string accessToken)
        {
            TokenRevocationResponse response = await _RevokeToken(accessToken);

            if (response.IsError)
            {
                _logger.LogError(response.Error);
                throw new HttpStatusException(
                   HttpStatusCode.InternalServerError);
            }

            return response;
        }

        /// <summary>
        /// Асинхронная проверка существования юзера.
        /// </summary>
        /// <param name="userViewModel">Пользователь.</param>
        /// <returns>Существует или нет.</returns>
        public async Task<bool> UserExistsAsync(UserViewModel userViewModel)
        {
            try
            {
                User? user = await _userManager.FindByEmailAsync(
                    userViewModel.Email);
                return user != null ? true : false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new HttpStatusException(
                    HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Асинхронное создание нового пользователя.
        /// </summary>
        /// <param name="userViewModel">Пользователь.</param>
        /// <returns>Результат выполнения операции.</returns>
        public async Task CreateUserAsync(UserViewModel userViewModel)
        {
            try
            {
                await _userManager.CreateAsync(_CreateUser(userViewModel));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new HttpStatusException(
                    HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Асинхронное получение токена доступа.
        /// </summary>
        /// <param name="userViewModel">Пользователь.</param>
        /// <returns>Ответ с токеном.</returns>
        public async Task<TokenResponse> GetTokenAsync(UserViewModel userViewModel)
        {
            using (var client = new HttpClient())
            {
                _discDocument = await client.GetDiscoveryDocumentAsync(
                    _configuration["IdentityServer:Openid-configuration"]);

                if (_discDocument.IsError)
                {
                    _logger.LogError(_discDocument.Error);
                    throw new HttpStatusException(
                        HttpStatusCode.InternalServerError);
                }
            }

            using (var client = new HttpClient())
            {
                TokenResponse tokenResponse = await client.RequestClientCredentialsTokenAsync(
                    new ClientCredentialsTokenRequest
                    {
                        Address = _discDocument.TokenEndpoint,
                        ClientId = _configuration["ClientIds:Teacher"],
                        Scope = _configuration["AllowedScopes:Teacher"],
                        ClientSecret = _configuration["Secret:Secret"]
                    });
                if (tokenResponse.IsError)
                {
                    _logger.LogError(tokenResponse.Error);
                    throw new HttpStatusException(
                        HttpStatusCode.InternalServerError);
                }

                return tokenResponse;
            }
        }

        /// <summary>
        /// Проверка пользователя, который пришел с клиента.
        /// </summary>
        /// <param name="userViewModel">Пользователь.</param>
        /// <returns>Пройдена проверка или нет.</returns>
        public bool CheckUserViewModel(UserViewModel userViewModel)
        {
            if (String.IsNullOrEmpty(userViewModel.Email) ||
                 String.IsNullOrEmpty(userViewModel.Name))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Получение токена из заголовков запроса.
        /// </summary>
        /// <param name="httpContext">Контекст http.</param>
        /// <returns>токен в виде строки.</returns>
        public string GetTokenFromRequestHeaders(HttpContext httpContext)
        {
            return httpContext.Request.Headers["Authorization"]
              .FirstOrDefault();  
        }

        #endregion Methods

    }
}
