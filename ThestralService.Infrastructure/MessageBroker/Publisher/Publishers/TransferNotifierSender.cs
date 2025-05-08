namespace ThestralService.Infrastructure.MessageBroker.Publisher.Publishers;

using Options;
using Domain.Transfer;
using Domain.User.Dtos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

internal class TransferNotifierSender(
    ILogger<TransferNotifierSender> logger, 
    IMessageSender messageSender, 
    IOptions<PublisherConfiguration> publisherOptions
) : ITransferNotifier
{
    private readonly PublisherConfiguration _publisherQueues = publisherOptions.Value;

    public void NotifyTransferToInternalServices(string userId, UserTransferRequestDto transferRequest, string messageType)
    {
        logger.LogInformation("Sending broadcast message on the queue {queue} to notify user own", _publisherQueues.UserTransferRequestBroadcastQueue);
        var headers = new EventHeaders(messageType, userId);
        messageSender.SendBroadcast(transferRequest, _publisherQueues.UserTransferRequestBroadcastQueue, headers.GetAttributesAsDictionary());
        logger.LogInformation("Message sent");
    }
}
