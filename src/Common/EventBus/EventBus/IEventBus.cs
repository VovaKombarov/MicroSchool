namespace Common.EventBus
{
    /// <summary>
    /// Интерфейс для работы с брокером сообщений.
    /// </summary>
    public interface IEventBus
    {
        /// <summary>
        /// Публикация события интеграции.
        /// </summary>
        /// <param name="event">Событие интеграции.</param>
        void Publish(IntegrationEvent @event);

        /// <summary>
        /// Подписка на событие интеграции.
        /// </summary>
        /// <typeparam name="T">Обобщенный тип для события интеграции.</typeparam>
        /// <typeparam name="TH">Обобщенный тип для обработчика события интеграции.</typeparam>
        void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>;

        /// <summary>
        /// Отписка от события интеграции.
        /// </summary>
        /// <typeparam name="T">Обобщенный тип для события интеграции.</typeparam>
        /// <typeparam name="TH">Обобщенный тип для обработчика события интеграции.</typeparam>
        void UnSubscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>;
    }
}
