namespace ThestralService.Workers.MessageBroker.Consumers;

using Workers.MessageBroker.Options;
using Frieren_Guard;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Threading.Tasks;
using Workers.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using Application.Interfaces;

public class TransferReplyConsumer(
        ILogger<TransferReplyConsumer> logger,
        ConnectionFactory rabbitConnection,
        IHealthCheckNotifier healthCheckNotifier,
        SystemStatusMonitor statusMonitor,
        IOptions<ConsumerConfiguration> queues,
        IServiceProvider serviceProvider
    ) : BaseRabbitMQWorker(logger, rabbitConnection.CreateConnection(), healthCheckNotifier, statusMonitor, queues.Value.TransferUserReplyQueue)
{
    protected override async Task ProcessMessageAsync(BasicDeliverEventArgs eventArgs, IModel channel)
    {
        var body = eventArgs.Body.ToArray();
        var headers = eventArgs.BasicProperties.Headers;
        var operation = headers.GetUserEventType();
        var userId = headers.GetHeaderValue("UserId");
        using var scope = serviceProvider.CreateScope();
        var useCase = scope.ServiceProvider.GetServices<IInternalServicesResponseUseCase>().First(s => s.UseCase == operation);
        var jsonString = Encoding.UTF8.GetString(body);
        await useCase.ExecuteAsync(jsonString, userId);
        channel.BasicAck(eventArgs.DeliveryTag, false);
    }
}
