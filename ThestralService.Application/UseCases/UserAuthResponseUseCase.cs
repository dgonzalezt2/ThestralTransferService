namespace ThestralService.Application.UseCases;

using Application.Interfaces;
using Domain.Transfer.Repositories;
using Domain.Transfer;
using Domain.Transfer.Exceptions;
using Microsoft.Extensions.Logging;

public class UserAuthResponseUseCase(ITransferCommandRepository transferCommandRepository, ITransferQueryRepository transferQueryRepository, ITransferToExternalProviderUseCase transferToExternalProvider, ILogger<UserAuthResponseUseCase> logger) : IInternalServicesResponseUseCase
{
    public TransferOperations UseCase { get; } = TransferOperations.USER_AUTH_RESPONSE;

    public async Task ExecuteAsync(string body, string userId)
    {
        logger.LogInformation("Start processing UserAuth request");
        var transfer = await transferCommandRepository.GetByUserIdAsync(userId) ?? throw new TransferNotFoundException(userId);
        transfer.SetUserAuthDisabled();
        await transferQueryRepository.UpdateAsync(transfer);
        await transferToExternalProvider.TryTransferAsync(transfer);
        logger.LogInformation("UserAuth request complete");
    }
}
