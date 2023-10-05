namespace IdentityApi.Utilities
{
    /// <summary>
    /// Ключ сообщений.
    /// </summary>
    public enum MessageKey
    {
        /// <summary>
        /// Ошибка токена.
        /// </summary>
        TokenError,

        /// <summary>
        /// Ошибка существования пользователя.
        /// </summary>
        UserExistsError,

        /// <summary>
        /// Пользователь существует.
        /// </summary>
        UserExists,

        /// <summary>
        /// Пользователь не существует.
        /// </summary>
        UserNotExists,

        /// <summary>
        /// Пустое значение.
        /// </summary>
        EmptyValue
    }
}
