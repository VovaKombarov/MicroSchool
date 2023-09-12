using IdentityApi.ViewModels;
using IdentityModel.Client;

namespace IdentityApi.Services
{
    public interface IIdentityService
    {
   
        Task<TokenRevocationResponse> RevokeToken(string accessToken);

        Task<bool> UserExistsAsync(UserViewModel userViewModel);

        Task CreateUserAsync(UserViewModel userViewModel);

        Task<TokenResponse> GetTokenAsync(UserViewModel userViewModel);

        bool CheckUserViewModel(UserViewModel userViewModel);

        string GetTokenFromRequestHeaders(HttpContext httpContext);
    }
}
