namespace ThestralService.Domain.Transfer.Exceptions;

using SharedKernel.Exceptions;


public class IdentificationNumberNotFound : BusinessException
{
    public IdentificationNumberNotFound() : base()
    {
    }

    public IdentificationNumberNotFound(string message) : base(message)
    {
    }

    public IdentificationNumberNotFound(string message, Exception innerException) : base(message, innerException)
    {
    }
}
