namespace ThestralService.Domain.Transfer.Exceptions;

using SharedKernel.Exceptions;

public class ExternalOperatorTransferUrlNotFoundException : BusinessException
{
    public ExternalOperatorTransferUrlNotFoundException() : base("Transfer URL from external operator not found")
    {
    }

    public ExternalOperatorTransferUrlNotFoundException(string message) : base($"External operator URL transfer not found with Id: {message}")
    {
    }

    public ExternalOperatorTransferUrlNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

