namespace ThestralService.Application.UseCases;

using Domain.Transfer.Repositories;
using Domain.Transfer.Entities;
using Domain.User.Dtos;
using Domain.Transfer;
using Interfaces;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

internal class CreateTransferUseCase(
        ITransferCommandRepository transferCommandRepository, 
        ITransferQueryRepository transferQueryRepository,
        ILogger<CreateTransferUseCase> logger,
        ITransferNotifier transferNotifier
    ) : ICreateTransferUseCase
{
    public async Task<Transfer> ExecuteAsync(string userId, UserTransferRequestDto transferRequestDto)
    {
        logger.LogInformation("Transfer for the user {userId} begins", userId);
        var transferExists = await transferCommandRepository.ExistsByUserIdAsync(userId);
        var transfer = Transfer.BuildFromUserTransferRequest(userId, transferRequestDto);
        if (!transferExists)
        {
            await transferQueryRepository.CreateAsync(transfer);
        }
        transferNotifier.NotifyTransferToInternalServices(userId, transferRequestDto, TransferOperations.TRANSFER_USER.ToString());
        logger.LogInformation("Hogwarts services are notified to process the transfer");
        return transfer;
    }
}
