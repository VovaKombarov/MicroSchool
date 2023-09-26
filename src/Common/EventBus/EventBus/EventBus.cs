using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Common.EventBus
{
    /// <summary>
    /// Брокер сообщений.
    /// </summary>
    public class EventBus : IEventBus, IDisposable
    {
        #region Constants 

        /// <summary>
        /// Имя обменника.
        /// </summary>
        const string EXCHANGE_NAME = "microSchool_exchange";

        /// <summary>
        /// Сообщение на случай того, если данные в конфигурации отсутствует или не заданы.
        /// </summary>
        const string MESSAGE_ITEM_MISSING = "Элемент отсутствует или не задан";

        #endregion Constants

        #region Fields

        /// <summary>
        /// Обьект содержащий набор свойств конфигурации.
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Подключение.
        /// </summary>
        private readonly IConnection _connection;

        /// <summary>
        /// Обьект управления подписками/отписками на события интеграции.
        /// </summary>
        private readonly IEventBusSubscriptionManager _subManager;

        /// <summary>
        /// Логгер.
        /// </summary>
        private readonly ILogger<EventBus> _logger;

        /// <summary>
        /// Имя очереди.
        /// </summary>
        private string _queueName;

        /// <summary>
        /// Канал.
        /// </summary>
        private IModel _channel;

        /// <summary>
        /// Обьект предоставляющий доступ к сервисам.
        /// </summary>
        private readonly IServiceProvider _serviceProvider;

        #endregion Fields

        #region Constructors 

        /// <summary>
        /// Конструктор брокера сообщений.
        /// </summary>
        /// <param name="configuration">Обьект содержащий набор свойств конфигурации.</param>
        /// <param name="subManager">Обьект управления подписками/отписками на события интеграции.</param>
        /// <param name="serviceProvider">Обьект предоставляющий доступ к сервисам.</param>
        /// <param name="logger">Логгер.</param>
        /// <exception cref="ArgumentException">Исключение возникающие при невалидном аргументе.</exception>
        public EventBus(
            IConfiguration configuration, 
            IEventBusSubscriptionManager subManager, 
            IServiceProvider serviceProvider,
            ILogger<EventBus> logger)
        {
            _configuration = configuration ?? 
                throw new ArgumentException(nameof(configuration));
            _subManager = subManager ?? 
                throw new ArgumentException(nameof(subManager));
            _serviceProvider = serviceProvider ?? 
                throw new ArgumentException(nameof(serviceProvider));
            _logger = logger ?? 
                throw new ArgumentException(nameof(logger));

            try
            {
                ConnectionFactory factory = _CreateConnectionFactory();
               _connection = factory.CreateConnection();
                _CreateConsumerChannel();
                _subManager.OnEventRemoved += SubManager_OnEventRemoved;
            }
            catch (Exception ex)
            {
                _logger.LogError("Ошибка в инициализации брокера", ex);
            }
            
        }

        #endregion Constructors

        #region Utilities

        /// <summary>
        /// Обработчик события отписки от события интеграции.
        /// </summary>
        /// <param name="sender">Отправитель.</param>
        /// <param name="eventName">Имя события.</param>
        private void SubManager_OnEventRemoved(object? sender, string eventName)
        {
            if(_channel != null)
            {
                _channel.QueueUnbind(
                   queue: _queueName,
                   exchange: EXCHANGE_NAME,
                   routingKey: eventName);
            } 
        }

        /// <summary>
        /// Проверить данные конфигурации.
        /// </summary>
        /// <param name="propertyName">Имя свойства, которое проверяем в конфигурации.</param>
        /// <exception cref="Exception">Исключение, если свойсство равно null или пустой строке.</exception>
        private void  _CheckConfigurationData(string propertyName)
        {
            if (string.IsNullOrEmpty(_configuration[propertyName]))
            {
                _logger.LogError(MESSAGE_ITEM_MISSING + " " + propertyName);
                throw new Exception(MESSAGE_ITEM_MISSING + " " + propertyName);
            }
        }

        /// <summary>
        /// Создать фабрику для создания подключения.
        /// </summary>
        /// <returns>Обьект фабричного класса для создания подключения.</returns>
        private ConnectionFactory _CreateConnectionFactory()
        {
            _CheckConfigurationData("RabbitMQHost");
            _CheckConfigurationData("RabbitMQPort");

            return new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQHost"],
                Port = int.Parse(_configuration["RabbitMQPort"]),
                DispatchConsumersAsync = true
            };
        }

        /// <summary>
        /// Создать канал получения.
        /// Важные аспекты.
        /// 1)durable: true. Очередь выдержит перезапуск сервера RabbitMQ.
        /// 2)prefetchCount: 1. Не отправлять новое сообщение, пока не подтверждено предыдущее.
        /// </summary>
        private void _CreateConsumerChannel()
        {
            _logger.LogInformation("Создание канала получения.");

            _channel = _connection.CreateModel();
           
            _queueName = _channel.QueueDeclare(durable: true).QueueName;

            _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            _channel.ExchangeDeclare(
                exchange: EXCHANGE_NAME, type: ExchangeType.Direct);

            _channel.CallbackException += _channel_CallbackException;
        }

        /// <summary>
        /// Обработчик события CallbackException в канале.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ex"></param>
        private void _channel_CallbackException(object? sender, CallbackExceptionEventArgs ex)
        {
            _logger.LogWarning(ex.Exception, "Пересоздаем канал");
            _channel.Dispose();
            _CreateConsumerChannel();
            _StartBasicConsume();
        }

        /// <summary>
        /// Создание получателя.
        /// Создаем получателя в двух случаях.
        /// 1) Регистрация получателя по подписке. 
        /// Для каждого события интеграции, регистрируем получателя.
        /// 2) При возникновении исключение в канале.
        /// </summary>
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

        /// <summary>
        /// Событие возникает, когда доставка доставлена потребителю.
        /// </summary>
        /// <param name="sender">Отправитель.</param>
        /// <param name="eventArgs">Аргументы содержащие дополнительную информацию.</param>
        /// <returns>Результат выполнения асинхронной операции.</returns>
        private async Task _Consumer_Received(object sender, BasicDeliverEventArgs eventArgs)
        {
            string eventName = eventArgs.RoutingKey;

            List<Type> handlers = _subManager
                .GetEventHandlersTypesByEventName(eventName);

            var eventType = _subManager.GetEventHandlerTypeByName(eventName);

            foreach (var handler in handlers)
            {
                string message = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
                await _InvokeHandle(message, eventType, handler);
            }

            _channel.BasicAck(eventArgs.DeliveryTag, multiple: false);
            _logger.LogInformation("Подтверждение доставки сообщения.");
        }

        /// <summary>
        /// Вызов обработчика события интеграции.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        /// <param name="eventType">Тип сообщения.</param>
        /// <param name="eventHadlerType">Тип обработчика сообщения.</param>
        /// <returns>Результат выполения операции.</returns>
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

                Type concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);

                // Переключаем контекст и гарантируем, что оставшаяся часть метода выполниться в текущем контексте синхронизации
                await Task.Yield();
                await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent });

            }
            catch (Exception ex)
            {
                _logger.LogError(
                    $"Ошибка при выполнении метода Handle {eventType} {eventHadlerType} ", ex);
            }
        }

        /// <summary>
        /// Отправка сообщения.
        /// Важное свойство properties.Persistent = true;
        /// Гарантируем, что сообщения не будут потеряны в случае остановки сервера 
        /// </summary>
        /// <param name="event">Событие интеграции.</param>
        private void _SendMessage(IntegrationEvent @event)
        {
            string eventName = @event.GetType().Name;

            var body = JsonSerializer.SerializeToUtf8Bytes(
                @event, @event.GetType(), new JsonSerializerOptions
                {
                    WriteIndented = true
                });

            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true;

            _channel.BasicPublish(
                exchange: EXCHANGE_NAME,
                routingKey: eventName,
                basicProperties: properties,
                body: body);

            _logger.LogInformation($"Публикация сообщения  {eventName}");
        }

        #endregion Utilities

        #region Methods

        /// <summary>
        /// Публикация события интеграции.
        /// </summary>
        /// <param name="event">Событие интеграции.</param>
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

        /// <summary>
        /// Подписка обработчиков событий интеграции на событие интеграции.
        /// </summary>
        /// <typeparam name="T">Обобщенный тип события интеграции.</typeparam>
        /// <typeparam name="TH">Обощенный тип обработчика события интеграции.</typeparam>
        public void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            _subManager.AddSubscription<T, TH>();

            _logger.LogInformation(
                "Подписка на событие {EventName} обработчика {EventHandler}", 
                typeof(T).Name, 
                typeof(TH).Name);

            _channel.QueueBind(
                queue: _queueName,
                exchange: EXCHANGE_NAME,
                routingKey: typeof(T).Name);

            _StartBasicConsume();
        }

        /// <summary>
        /// Отписка обработчика события интеграции от события интеграции.
        /// </summary>
        /// <typeparam name="T">Обощенный тип события интеграции.</typeparam>
        /// <typeparam name="TH">Обобщенный тип обработчика события интеграции.</typeparam>
        public void UnSubscribe<T, TH>()
           where T : IntegrationEvent
           where TH : IIntegrationEventHandler<T>
        {
            _subManager.RemoveSubscription<T, TH>();
            
            _logger.LogInformation(
               "Удаление подписки на событие {EventName} обработчика {EventHandler}", typeof(T), typeof(TH));
        }

        /// <summary>
        /// Освобождение неуправляемых ресурсов.
        /// </summary>
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
