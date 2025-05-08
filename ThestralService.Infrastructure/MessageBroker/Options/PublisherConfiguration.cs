namespace ThestralService.Infrastructure.MessageBroker.Options;

public class PublisherConfiguration
{
    public required string UserTransferRequestBroadcastQueue { get; set; }
    public required string UserTransferNotificationQueue { get; set; }
    public required string UserAuthExchangeQueue { get; set; }
    public required string UserManagementExchange { get; set; }
    public required string FileManagerExchangeQueue { get; set; }
}
