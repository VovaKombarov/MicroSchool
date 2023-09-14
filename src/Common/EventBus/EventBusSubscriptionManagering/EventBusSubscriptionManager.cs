namespace Common.EventBus
{
    /// <summary>
    /// Класс управляет подписками/отписками событий интеграции на необходимые обрработчики.
    /// </summary>
    public class EventBusSubscriptionManager : IEventBusSubscriptionManager
    {
        #region Fields

        /// <summary>
        /// Набор уникальных коллекций пар имя события интеграции/обработчик события интеграции.
        /// </summary>
        private HashSet<KeyValuePair<string, Type>> _handlersSet =
            new HashSet<KeyValuePair<string, Type>>();

        public event EventHandler<string> OnEventRemoved;

        #endregion Fields

        #region Methods

        /// <summary>
        /// Добавляет подписку на событие интеграции.
        /// </summary>
        /// <typeparam name="T">Обощенный тип события интеграции.</typeparam>
        /// <typeparam name="TH">Обощенный тип обработчика события интеграции.</typeparam>
        public void AddSubscription<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            string eventName = typeof(T).Name;
            Type handlerType = typeof(TH);

            _handlersSet.Add(
                new KeyValuePair<string, Type>(eventName, handlerType)); 
        }

        /// <summary>
        /// Получает коллекцию типов обработчиков событий по имени события интеграции.
        /// </summary>
        /// <param name="eventName">Имя события интеграции.</param>
        /// <returns>Коллекцию типов обработчиков событий по имени события интеграции.</returns>
        public List<Type> GetEventHandlersTypesByEventName(
            string eventName)
        {
            return _handlersSet
                .Where(w => w.Key == eventName)
                .Select(w => w.Value)
                .ToList();
        }

        /// <summary>
        /// Получает тип обработчика событий интеграции по имени события интеграции.
        /// </summary>
        /// <param name="eventName">Имя события интеграции.</param>
        /// <returns>Тип обработчика событий по имени события интеграции.</returns>
        public Type GetEventHandlerTypeByName(string name) =>
            _handlersSet.FirstOrDefault(w => w.Key == name).Value;

        /// <summary>
        /// Удаляет подписку.
        /// </summary>
        /// <typeparam name="T">Обобщенный тип события интеграции.</typeparam>
        /// <typeparam name="TH">Обощенный тип обработчика события для события интеграции.</typeparam>
        public void RemoveSubscription<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            string eventName = typeof(T).Name;
            Type handlerType = typeof(TH);

            _handlersSet.Remove(
                new KeyValuePair<string, Type>(eventName, handlerType));
            RaiseOnEventRemoved(eventName);
        }

        /// <summary>
        /// Очищает коллекцию обработчиков.
        /// </summary>
        public void ClearHandlers() => _handlersSet.Clear();

        #endregion Methods

        private void RaiseOnEventRemoved(string eventName)
        {
            var handler = OnEventRemoved;
            handler?.Invoke(this, eventName);
        }
    }

}
