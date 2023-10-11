using Common.EventBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace Common.Api.Exstensions
{
    /// <summary>
    /// Общее расширение для всех проектов.
    /// </summary>
    public static class CommonExstension
    {
        /// <summary>
        /// Добавление общих сервисов, которые используется всеми проектами. 
        /// </summary>
        /// <param name="services">Коллекция сервисов.</param>
        /// <param name="configuration">Конфигурация приложения.</param>
        /// <returns></returns>
        public static IServiceCollection AddServiceDefaults(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<Dictionary<string, string>>(
                configuration.GetSection("messages"));

            services.AddControllers();

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddConfiguration(
                    configuration.GetSection("Logging"));

                loggingBuilder.AddSimpleConsole(options =>
                {
                    options.IncludeScopes = true;
                    options.SingleLine = true;
                    options.TimestampFormat = "HH:mm:ss ";
                });

                loggingBuilder.AddDebug();
            });

            services.AddSingleton<IEventBusSubscriptionManager, EventBusSubscriptionManager>();
            services.AddSingleton<IEventBus, EventBus.EventBus>();
            return services;
        }
    }
}
