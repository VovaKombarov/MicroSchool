namespace Common.EventBus
{
    /// <summary>
    /// Интерфейс для EventHandler события интеграции.
    /// </summary>
    public interface IIntegrationEventHandler
    {

    }

    /// <summary>
    /// Интерфейс для EventHandler события интеграции.
    /// </summary>
    /// <typeparam name="TIntegrationEvent">
    /// Обобщенный тип обработчика события, тип является контрвариантным.</typeparam>
    public interface IIntegrationEventHandler<in TIntegrationEvent> : 
        IIntegrationEventHandler where TIntegrationEvent : IntegrationEvent
    {
        public Task Handle(TIntegrationEvent @event);
    }
}
