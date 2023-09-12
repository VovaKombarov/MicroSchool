
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Transactions;

namespace Common.EventBus
{
    public class EventBus : IEventBus, IDisposable
    {
        #region Constants 

        const string EXCHANGE = "direct_trigger";

        #endregion Constants

        #region Fields

        private readonly IConnection _connection;
        private IModel _channel;
        private readonly IConfiguration _configuration;
        private readonly IEventBusSubscriptionManager _eventBusSubscriptionManager;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<EventBus> _logger;
        private string _queueName;

        #endregion Fields

        #region Constructors 

        public EventBus(
            IConfiguration configuration, 
            IEventBusSubscriptionManager eventBusSubscriptionManager, 
            IServiceProvider serviceProvider,
            ILogger<EventBus> logger)
        {
            _configuration = configuration ?? 
                throw new ArgumentException(nameof(configuration));
            _eventBusSubscriptionManager = eventBusSubscriptionManager ?? 
                throw new ArgumentException(nameof(eventBusSubscriptionManager));
            _serviceProvider = serviceProvider ?? 
                throw new ArgumentException(nameof(serviceProvider));
            _logger = logger ?? 
                throw new ArgumentException(nameof(logger));

            ConnectionFactory factory = _CreateConnectionFactory();

            try
            {
                _connection = factory.CreateConnection();
                _logger.LogInformation("Создание подключения");

                _CreateConsumerChannel();

            }
            catch (Exception ex)
            {
                _logger.LogError("Ошибка в инициализации брокера", ex);
            } 
        }

        #endregion Constructors

        #region Utilities

        private void _CreateConsumerChannel()
        {
            _channel = _connection.CreateModel();
            _logger.LogInformation("Создание канала для публикации сообщений.");

            _queueName = _channel.QueueDeclare(durable: true).QueueName;
            _logger.LogInformation($"Декларирование очереди {_queueName}");

            _channel.ExchangeDeclare(
                exchange: EXCHANGE, type: ExchangeType.Direct);
            _logger.LogInformation($"Декларирование обменника {EXCHANGE}");

            _channel.CallbackException += _channel_CallbackException;
        }

        private void _channel_CallbackException(object? sender, CallbackExceptionEventArgs ex)
        {
            _channel.Dispose();
            _CreateConsumerChannel();
            _logger.LogWarning(ex.Exception, "Пересоздаем канал");
            _StartBasicConsume();
        }


        private ConnectionFactory _CreateConnectionFactory()
        {
            return new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQHost"],
                Port = int.Parse(_configuration["RabbitMQPort"]),
                DispatchConsumersAsync = true
            };
        }

        private void _SendMessage(IntegrationEvent @event)
        {
            var eventName = @event.GetType().Name;

            var body = JsonSerializer.SerializeToUtf8Bytes(
                @event, @event.GetType(), new JsonSerializerOptions
                {
                    WriteIndented = true
                });

            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true;
            _logger.LogInformation($"Флаг для сообщений - постоянные");

            _channel.BasicPublish(
                exchange: EXCHANGE,
                routingKey: eventName,
                basicProperties: properties,
                body: body);

            _logger.LogInformation($"Публикация сообщения  {eventName}");
        }

        private void _StartBasicConsume()
        {
            _logger.LogTrace("Создание получателя");
            if (_channel != null)
            {
                var consumer = new AsyncEventingBasicConsumer(_channel);

                consumer.Received += _Consumer_Received;

                _channel.BasicConsume(
                    queue: _queueName, autoAck: false, consumer: consumer);
            }
            else
            {
                _logger.LogError("Канал равен null");
            }

        }

        private async Task _Consumer_Received(object sender, BasicDeliverEventArgs eventArgs)
        {
            string eventName = eventArgs.RoutingKey;

            List<Type> handlers = _eventBusSubscriptionManager
                .GetEventHandlersTypesByEventName(eventName);

            var eventType = _eventBusSubscriptionManager.GetEventHandlerTypeByName(eventName);

            foreach (var handler in handlers)
            {
                string message = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
                await _InvokeHandle(message, eventType, handler);
            }

            _channel.BasicAck(eventArgs.DeliveryTag, multiple: false);
            _logger.LogInformation("Подтверждение доставки сообщения.");
        }


        private async Task _InvokeHandle(string message, Type eventType, Type eventHadlerType)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();

            try
            {
                var integrationEvent = JsonSerializer.Deserialize(
                    message, eventType, new JsonSerializerOptions()
               {
                   PropertyNameCaseInsensitive = true
               });

                var handler = scope.ServiceProvider.GetService(eventHadlerType);

                string str = string.Empty;

                var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);
                await Task.Yield();
                await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent });

            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при выполнении метода Handle {eventType} {eventHadlerType} ", ex);
            }
        }

        #endregion Utilities

        #region Methods

        public void Publish(IntegrationEvent @event) 
        {
            if (_connection.IsOpen)
            {
                _SendMessage(@event);
            }
            else
            {
                _logger.LogError("Подключение не открыто");
            }
        }

        public void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            _eventBusSubscriptionManager.AddSubscription<T, TH>();
            _logger.LogInformation("Добавление подписок");

            _channel.QueueBind(queue: _queueName,
                      exchange: EXCHANGE,
                      routingKey: typeof(T).Name);
            _logger.LogInformation($"Связывание очереди по нужному ключу маршрутизации {typeof(T).Name} " );

            _StartBasicConsume();
        }

        public void UnSubscribe<T, TH>()
           where T : IntegrationEvent
           where TH : IIntegrationEventHandler<T>
        {
            _eventBusSubscriptionManager.RemoveSubscription<T, TH>();
            _logger.LogTrace("Удаление подписок");
        }

        public void Dispose()
        {
            if (_channel != null)
            {
                _channel.Close();
                _connection.Close();
            }
        }

        #endregion Methods
    }
}
