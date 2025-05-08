namespace ThestralService.Domain.Transfer.Exceptions;

using SharedKernel.Exceptions;

public class TransferNotFoundException : BusinessException
{
    public TransferNotFoundException() : base()
    {
    }

    public TransferNotFoundException(string message) : base($"Transfer with userId {message} not found")
    {
    }

    public TransferNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

