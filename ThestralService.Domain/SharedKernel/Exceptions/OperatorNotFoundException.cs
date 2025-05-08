namespace ThestralService.Domain.SharedKernel.Exceptions;

public class OperatorNotFoundException : BusinessException
{
    public OperatorNotFoundException() : base()
    {
    }

    public OperatorNotFoundException(string message) : base(message)
    {
    }

    public OperatorNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}