namespace IdentityApi.ViewModels
{
    /// <summary>
    /// Пользователь приходящий с клиента.
    /// </summary>
    public class UserViewModel
    {
        /// <summary>
        /// Имя.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Электронная почта.
        /// </summary>
        public string Email { get; set; } 
    }
}
