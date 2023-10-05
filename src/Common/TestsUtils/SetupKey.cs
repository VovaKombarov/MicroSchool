namespace Common.TestsUtils
{
    /// <summary>
    /// Ключ настройки тестового метода.
    /// </summary>
    public enum SetupKey
    {
        /// <summary>
        /// Внутренняя ошибка сервера.
        /// </summary>
        InternalServerError,

        /// <summary>
        /// Элемент не найден.
        /// </summary>
        NotFound,

        /// <summary>
        /// Возврат значения.
        /// </summary>
        ReturnsValue,

        /// <summary>
        /// Ok.
        /// </summary>
        Ok,

        /// <summary>
        /// Пустая коллекция.
        /// </summary>
        EmptyCollection,

        /// <summary>
        /// Не пустая коллекция.
        /// </summary>
        NotEmptyCollection
    }
}
