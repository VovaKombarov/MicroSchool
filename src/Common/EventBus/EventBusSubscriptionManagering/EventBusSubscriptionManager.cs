using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Common.EventBus
{
    public class EventBusSubscriptionManager : IEventBusSubscriptionManager
    {
        private HashSet<KeyValuePair<string, Type>> _handlersSet =
            new HashSet<KeyValuePair<string, Type>>();

        private List<Type> _types = new List<Type>();
           
        public EventBusSubscriptionManager()
        {

        }

        public void AddSubscription<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = typeof(T).Name;
            var handlerType = typeof(TH);

            _handlersSet.Add(new KeyValuePair<string, Type>(eventName, handlerType));

            if (_types.Any(w => w.Name == eventName))
                return;

            _types.Add(typeof(T));
        }

        public List<Type> GetEventHandlersByEventName(
            string eventName)
        {
            return _handlersSet.Where(w => w.Key == eventName)
                .Select(w => w.Value)
                .ToList();
        }

        public Type GetEventTypeByName(string name) => 
            _types.FirstOrDefault(w => w.Name == name);

        public void RemoveSubscription<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = typeof(T).Name;
            var handlerType = typeof(TH);

            _handlersSet.Remove(new KeyValuePair<string, Type>(eventName, handlerType));
        }

        public void ClearHandlers() => _handlersSet.Clear();
    }
        
}
