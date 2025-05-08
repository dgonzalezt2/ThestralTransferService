namespace ThestralService.Infrastructure.Configuration;

using Domain.Transfer;
using MessageBroker.Publisher.Publishers;
using EntityFrameworkCore;
using MessageBroker.Publisher;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Domain.GovCarpeta;
using GovCarpeta;
using Cache;
using Polly;
using ExternalOperatorTransfer;
using UserNotifier;

public static class ConfigureServices
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEntityFramework(configuration);
        services.AddRepositories();
        services.AddSingleton<IMessageSender, RabbitMQMessageSender>();
        services.AddSingleton<ITransferNotifier, TransferNotifierSender>();
        services.AddSingleton<IGetOperatorUseCase, GetOperatorUseCase>();
        services.AddSingleton<IOperatorsHttpClient, OperatorsHttpClient>();
        services.AddSingleton<IAsyncPolicy<HttpResponseMessage>>(provider => GetRetryPolicy());
        services.AddHttpClient("GovCarpeta", client =>
        {
            client.BaseAddress = new Uri(configuration["GovCarpeta:BaseUrl"] ?? "https://govcarpeta-apis-4905ff3c005b.herokuapp.com/");
        });
        services.AddHttpClient("ExternalOperators", client =>
        {
        }).AddPolicyHandler((provider, request) =>
        {
            return provider.GetRequiredService<IAsyncPolicy<HttpResponseMessage>>();
        });
        
        services.AddStackExchangeRedisCache(options =>
        {
            configuration.Bind("Cache:Redis", options);
        });
        services.AddSingleton<ICacheStore, CacheStore>();
        services.AddScoped<IExternalOperatorNotifier, ExternalOperatorNotifier>();
        services.AddSingleton<IUserTransferCompleteNotification, UserTransferCompleteNotification>();
        services.Configure<BaseTransferReplyUrl>(options =>
            configuration.GetSection("BaseTransferReplyUrl").Bind(options)
        );
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return Policy.Handle<HttpRequestException>()
                     .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                     .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }
}
