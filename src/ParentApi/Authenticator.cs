using IdentityModel.AspNetCore.OAuth2Introspection;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;

namespace ParentApi
{
    public class Authenticator
    {
        private readonly ConfigurationManager _configuration;

        private readonly ILogger _logger;

        private ILogger _CreateLogger()
        {
            var logger = LoggerFactory.Create(config =>
            {
                config.AddConsole();
            }).CreateLogger("Authenticator");

            return logger;
        }

        public Authenticator(ConfigurationManager configuration)
        {
            _configuration = configuration;
            _logger = _CreateLogger();
        }

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
    }
}
