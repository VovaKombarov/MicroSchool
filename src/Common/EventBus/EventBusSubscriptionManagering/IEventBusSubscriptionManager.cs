namespace Common.EventBus
{
    /// <summary>
    /// Интерфейс управления 
    /// подписок/отписок обработчиков событий на событии интеграции.
    /// </summary>
    public interface IEventBusSubscriptionManager
    {
        /// <summary>
        /// Добавляет подписку.
        /// </summary>
        /// <typeparam name="T">Обобщенный тип события интеграции.</typeparam>
        /// <typeparam name="TH">Обощенный тип обработчика события для события интеграции.</typeparam>
        void AddSubscription<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>;

        /// <summary>
        /// Удаляет подписку.
        /// </summary>
        /// <typeparam name="T">Обобщенный тип события интеграции.</typeparam>
        /// <typeparam name="TH">Обощенный тип обработчика события для события интеграции.</typeparam>
        void RemoveSubscription<T, TH>()
             where T : IntegrationEvent
             where TH : IIntegrationEventHandler<T>;

        /// <summary>
        /// Получает коллекцию типов обработчиков событий по имени события интеграции.
        /// </summary>
        /// <param name="eventName">Имя события интеграции.</param>
        /// <returns>Коллекцию типов обработчиков событий по имени события интеграции.</returns>
        List<Type> GetEventHandlersTypesByEventName(
            string eventName);

        /// <summary>
        /// Получает тип обработчика событий интеграции по имени события интеграции.
        /// </summary>
        /// <param name="eventName"></param>
        /// <returns>Тип обработчика событий по имени события интеграции.</returns>
        Type GetEventHandlerTypeByName(string eventName);

        /// <summary>
        /// Очищает коллекцию обработчиков.
        /// </summary>
        public void ClearHandlers();

        /// <summary>
        /// Событие на удаление подписки.
        /// </summary>
        event EventHandler<string> OnEventRemoved;
    }
}
