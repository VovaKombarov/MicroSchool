using IdentityModel.AspNetCore.OAuth2Introspection;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;

namespace ParentApi.Utilities
{
    /// <summary>
    /// Обработчик аутентификации.
    /// </summary>
    public class AuthenticationHandler
    {
        #region Fields

        /// <summary>
        /// Обьект конфигурации.
        /// </summary>
        private readonly ConfigurationManager _configuration;

        /// <summary>
        /// Логгер.
        /// </summary>
        private readonly ILogger _logger;

        #endregion Fields

        #region Constructors
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="configuration">Обьект конфигурации.</param>
        public AuthenticationHandler(ConfigurationManager configuration)
        {
            _configuration = configuration;
            _logger = _CreateLogger();
        }

        #endregion Constructors

        #region Utilities 

        /// <summary>
        /// Создание логгера.
        /// </summary>
        /// <returns>Логгер.</returns>
        private ILogger _CreateLogger()
        {
            var logger = LoggerFactory.Create(config =>
            {
                config.AddConsole();
            }).CreateLogger("AuthenticationTypeCustomizer");

            return logger;
        }

        #endregion Utilities

        #region Methods

        /// <summary>
        /// Аутентификация по Reference token.
        /// </summary>
        /// <param name="options">Опции IdenitityServer.</param>
        public void ReferenseTokenAuthentication(IdentityServerAuthenticationOptions options)
        {
            options.Authority = _configuration["IdentityServer:Authority"];
            options.ApiName = _configuration["AllowedScopes:Teacher"];
            options.ApiSecret = _configuration["Secret:secret"];

            // Отключаем при разработке, чтобы не было ошибок.
            options.RequireHttpsMetadata = false;

            options.Events = new OAuth2IntrospectionEvents()
            {
                OnAuthenticationFailed = c =>
                {
                    _logger.LogError("Response token Authentication failed");
                    return Task.CompletedTask;
                },
            };
        }

        /// <summary>
        /// Аутентификация по jwt токену.
        /// </summary>
        /// <param name="options">Опции IdenitityServer.</param>
        public void JwtBerarerAuthentication(JwtBearerOptions options)
        {
            options.Authority = _configuration["IdentityServer:Authority"];
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false
            };

            // Отключаем при разработке, чтобы не было ошибок.
            options.RequireHttpsMetadata = false;

            options.Events = new JwtBearerEvents()
            {
                OnAuthenticationFailed = c =>
                {
                    _logger.LogError("JWT Authentication failed");
                    return Task.CompletedTask;
                },
            };
        }

        #endregion Methods
    }
}
