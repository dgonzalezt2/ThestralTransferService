namespace ThestralService.Workers.Extensions;

using MessageBroker.Options;
using Infrastructure.MessageBroker.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using MessageBroker.Consumers;
using Application.Services;
using Infrastructure.Configuration;
using Frieren_Guard.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddFrierenGuardServices(configuration);
        services.AddInfrastructure(configuration);
        services.AddWorkers(configuration);
        services.AddApplication();
    }
    
    public static void AddWorkers(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ConnectionFactory>(sp =>
        {
            var factory = new ConnectionFactory();
            configuration.GetSection("RabbitMQ:Connection").Bind(factory);
            return factory;
        });

        services.Configure<ConsumerConfiguration>(options =>
            configuration.GetSection("RabbitMQ:Queues:Consumer").Bind(options)
        );
        services.Configure<PublisherConfiguration>(options =>
            configuration.GetSection("RabbitMQ:Queues:Publisher").Bind(options)
        );
        services.AddHostedService<TransferConsumer>();
        services.AddHostedService<TransferReplyConsumer>();        
    }
}
