namespace ThestralService.Workers.MessageBroker.Options;

public class ConsumerConfiguration
{
    public string TransferUserQueue { get; set; }
    public string TransferUserReplyQueue { get; set; }
}
