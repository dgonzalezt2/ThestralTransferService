namespace ThestralService.Application.UseCases;

using Interfaces;
using System.Text.Json;
using Domain.Transfer.Repositories;
using Domain.Transfer;
using Domain.Transfer.Exceptions;
using Domain.User.Dtos;
using Domain.SharedKernel.Exceptions;
using Microsoft.Extensions.Logging;

public class UserManagementResponseUseCase(ITransferCommandRepository transferCommandRepository, ITransferQueryRepository transferQueryRepository, ITransferToExternalProviderUseCase transferToExternalProvider, ILogger<UserManagementResponseUseCase> logger) : IInternalServicesResponseUseCase
{
    public TransferOperations UseCase { get; } = TransferOperations.USER_MANAGEMENT_RESPONSE;

    public async Task ExecuteAsync(string body, string userId)
    {
        logger.LogInformation("Start processing UserManagement request");
        var userDto = JsonSerializer.Deserialize<UserManagementTransferResponseDto>(body) ?? throw new InvalidBodyException();
        var transfer = await transferCommandRepository.GetByUserIdAsync(userId) ?? throw new TransferNotFoundException(userId);
        transfer.Update(userDto);
        transfer.SetUserManagementDisabled();
        await transferQueryRepository.UpdateAsync(transfer);
        await transferToExternalProvider.TryTransferAsync(transfer);
        logger.LogInformation("UserManagement request complete");
    }
}
