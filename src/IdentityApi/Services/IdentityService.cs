using Azure;
using Azure.Core;
using IdentityApi.Models;
using IdentityApi.ViewModels;
using IdentityModel.Client;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.IIS.Core;
using Common.ErrorResponse;
using System.Net;
using Microsoft.Extensions.Options;
using IdentityApi.Utilities;

namespace IdentityApi.Services
{
    public class IdentityService : IIdentityService
    {
        #region Fields

        private readonly UserManager<User> _userManager;

        private readonly IConfiguration _configuration;

        private DiscoveryDocumentResponse _discDocument;

        private readonly ILogger<IdentityService> _logger;

        private readonly IOptions<Dictionary<string, string>> _options;

        #endregion Fields

        #region Constructors

        public IdentityService(
            UserManager<User> userManager, 
            IConfiguration configuration,
            ILogger<IdentityService> logger,
            IOptions<Dictionary<string, string>> options)
        {
            _userManager = userManager;
            _configuration = configuration;
            _logger = logger;
            _options = options;
        }

        #endregion Constructors

        #region Utilities

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
                        Scope = _configuration["AllowedScopes:TeacherApi"],
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

        public bool CheckUserViewModel(UserViewModel userViewModel)
        {
            if (String.IsNullOrEmpty(userViewModel.Email) ||
                 String.IsNullOrEmpty(userViewModel.Name))
            {
                return false;
            }

            return true;
        }

        public string GetTokenFromRequestHeaders(HttpContext httpContext)
        {
            return httpContext.Request.Headers["Authorization"]
              .FirstOrDefault();  
        }

        #endregion Methods

    }
}
