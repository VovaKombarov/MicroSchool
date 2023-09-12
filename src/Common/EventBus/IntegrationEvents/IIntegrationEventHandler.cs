namespace Common.EventBus
{
    public interface IIntegrationEventHandler
    {

    }

    public interface IIntegrationEventHandler<in TIntegrationEvent> : 
        IIntegrationEventHandler where TIntegrationEvent : IntegrationEvent
    {
        public Task Handle(TIntegrationEvent @event);
    }
}
