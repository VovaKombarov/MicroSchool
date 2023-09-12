namespace Common.EventBus
{
    public interface IEventBusSubscriptionManager
    {
        void AddSubscription<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>;

        void RemoveSubscription<T, TH>()
             where T : IntegrationEvent
             where TH : IIntegrationEventHandler<T>;

        List<Type> GetEventHandlersByEventName(
            string eventName);

        Type GetEventTypeByName(string name);

        public void ClearHandlers();
    }
}
