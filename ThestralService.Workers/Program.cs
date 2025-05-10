using ThestralService.Infrastructure.EntityFrameworkCore.DbContext;
using ThestralService.Infrastructure.MessageBroker.Options;
using ThestralService.Workers.Extensions;
using ThestralService.Workers.MessageBroker.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddServices(builder.Configuration);
builder.Configuration.AddEnvironmentVariables();

var host = builder.Build();

// run migrations
using (var scope = host.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<TransferDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    await dbContext.Database.MigrateAsync();
    logger.LogInformation("Database created successfully or already exists.");
}

using (var scope = host.Services.CreateScope())
{
    var connectionFactory = scope.ServiceProvider.GetRequiredService<ConnectionFactory>();
    using var connection = connectionFactory.CreateConnection();
    using var channel = connection.CreateModel();
    var queues = scope.ServiceProvider.GetRequiredService<IOptions<ConsumerConfiguration>>().Value;
    var transferUserReplyQueue = queues.TransferUserReplyQueue;
    var transferUserQueue = queues.TransferUserQueue;
    channel.QueueDeclare(queue: transferUserReplyQueue,
                         durable: true,
                         exclusive: false,
                         autoDelete: false,
                         arguments: null);
    channel.QueueDeclare(queue: transferUserQueue,
                         durable: true,
                         exclusive: false,
                         autoDelete: false,
                         arguments: null);
    var publisherQueues = scope.ServiceProvider.GetRequiredService<IOptions<PublisherConfiguration>>().Value;
    var userTransferNotificationQueue = publisherQueues.UserTransferNotificationQueue;
    var userTransferRequestBroadcastQueue = publisherQueues.UserTransferRequestBroadcastQueue;
    channel.QueueDeclare(queue: userTransferNotificationQueue,
                         durable: true,
                         exclusive: false,
                         autoDelete: false,
                         arguments: null);
    channel.ExchangeDeclare(exchange: userTransferRequestBroadcastQueue,
                             type: ExchangeType.Fanout,
                             durable: true,
                             autoDelete: false,
                             arguments: null);
    var userAuthExchangeQueue = publisherQueues.UserAuthExchangeQueue;
    var userManagementExchange = publisherQueues.UserManagementExchange;
    var fileManagerExchangeQueue = publisherQueues.FileManagerExchangeQueue;
    //channel.QueueDeclare(queue: userAuthExchangeQueue, durable: true, exclusive: false, autoDelete: false, arguments: null);
    channel.QueueBind(queue: userAuthExchangeQueue, exchange: userTransferRequestBroadcastQueue, routingKey: "");
    //channel.QueueDeclare(queue: userManagementExchange, durable: true, exclusive: false, autoDelete: false, arguments: null);
    channel.QueueBind(queue: userManagementExchange, exchange: userTransferRequestBroadcastQueue, routingKey: "");
    //channel.QueueDeclare(queue: fileManagerExchangeQueue, durable: true, exclusive: false, autoDelete: false, arguments: null);
    channel.QueueBind(queue: fileManagerExchangeQueue, exchange: userTransferRequestBroadcastQueue, routingKey: "");
}

host.Run();