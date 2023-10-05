using IdentityApi.ViewModels;
using IdentityModel.Client;

namespace IdentityApi.Services
{
    /// <summary>
    /// Интерфейс сервиса идентификации.
    /// </summary>
    public interface IIdentityService
    {
        /// <summary>
        /// Отзыв токена.
        /// </summary>
        /// <param name="accessToken">Токен доступа.</param>
        /// <returns>Ответ при отзыве токена.</returns>
        Task<TokenRevocationResponse> RevokeToken(string accessToken);

        /// <summary>
        /// Асинхронная проверка существования юзера.
        /// </summary>
        /// <param name="userViewModel">Пользователь.</param>
        /// <returns>Существует или нет.</returns>
        Task<bool> UserExistsAsync(UserViewModel userViewModel);

        /// <summary>
        /// Асинхронное создание нового пользователя.
        /// </summary>
        /// <param name="userViewModel">Пользователь.</param>
        /// <returns>Результат выполнения операции.</returns>
        Task CreateUserAsync(UserViewModel userViewModel);

        /// <summary>
        /// Асинхронное получение токена доступа.
        /// </summary>
        /// <param name="userViewModel">Пользователь.</param>
        /// <returns>Ответ с токеном.</returns>
        Task<TokenResponse> GetTokenAsync(UserViewModel userViewModel);

        /// <summary>
        /// Проверка пользователя, который пришел с клиента.
        /// </summary>
        /// <param name="userViewModel">Пользователь.</param>
        /// <returns>Пройдена проверка или нет.</returns>
        bool CheckUserViewModel(UserViewModel userViewModel);

        /// <summary>
        /// Получение токена из заголовков запроса.
        /// </summary>
        /// <param name="httpContext">Контекст http.</param>
        /// <returns>токен в виде строки.</returns>
        string GetTokenFromRequestHeaders(HttpContext httpContext);
    }
}
