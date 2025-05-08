using ThestralService.Domain.Transfer;

namespace ThestralService.Application.Interfaces;

public interface IInternalServicesResponseUseCase
{
    TransferOperations UseCase { get; }
    public Task ExecuteAsync(string body, string userId);
}
