namespace ThestralService.Application.Interfaces;

using Domain.Transfer.Entities;

public interface ITransferToExternalProviderUseCase
{
    Task TryTransferAsync(Transfer transfer);
}
