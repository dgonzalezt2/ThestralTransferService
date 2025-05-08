namespace ThestralService.Infrastructure.ExternalOperatorTransfer.Exceptions;

public class OperatorUnreachableException : Exception
{
    public OperatorUnreachableException() : base("Operator is unreachable.") { }

    public OperatorUnreachableException(string message) : base($"Operator URL {message} is unreachable.") { }

    public OperatorUnreachableException(string message, Exception innerException) : base(message, innerException) { }
}
