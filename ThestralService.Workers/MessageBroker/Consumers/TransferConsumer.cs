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
using Domain.Transfer;
using System.Text.Json;
using Domain.User.Dtos;
using Application.Interfaces;
using Domain.SharedKernel.Exceptions;

public class TransferConsumer(
        ILogger<TransferConsumer> logger,
        ConnectionFactory rabbitConnection,
        IHealthCheckNotifier healthCheckNotifier,
        SystemStatusMonitor statusMonitor,
        IOptions<ConsumerConfiguration> queues,
        IServiceProvider serviceProvider
    ) : BaseRabbitMQWorker(logger, rabbitConnection.CreateConnection(), healthCheckNotifier, statusMonitor, queues.Value.TransferUserQueue)
{
    protected override async Task ProcessMessageAsync(BasicDeliverEventArgs eventArgs, IModel channel)
    {
        var body = eventArgs.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        var headers = eventArgs.BasicProperties.Headers;
        var operation = headers.GetUserEventType();
        using var scope = serviceProvider.CreateScope();

        if (operation is TransferOperations.TRANSFER_USER)
        {
            var userDto = JsonSerializer.Deserialize<UserTransferRequestDto>(message) ?? throw new InvalidBodyException();
            var userId = headers.GetHeaderValue("UserId");
            logger.LogInformation("Processing request for user {userId}", userId);
            var transferUseCase = scope.ServiceProvider.GetRequiredService<ICreateTransferUseCase>();
            await transferUseCase.ExecuteAsync(userId, userDto);
        }
        channel.BasicAck(eventArgs.DeliveryTag, false);
    }
}

