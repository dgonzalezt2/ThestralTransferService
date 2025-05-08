namespace ThestralService.Domain.Transfer.Exceptions;

using SharedKernel.Exceptions;

public class TransferReplyUrlNotFoundException : BusinessException
{
    public TransferReplyUrlNotFoundException() : base("Transfer reply URL not found")
    {
    }

    public TransferReplyUrlNotFoundException(string message) : base($"Reply Url not found {message}")
    {
    }

    public TransferReplyUrlNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

