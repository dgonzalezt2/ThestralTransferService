using Microsoft.Extensions.Logging;

namespace ThestralService.Application.UseCases;

using Domain.Transfer;
using Domain.Transfer.Exceptions;
using Domain.GovCarpeta;
using Domain.SharedKernel.Exceptions;
using Domain.Transfer.Dtos;
using Domain.Transfer.Entities;
using Interfaces;
using System.Threading.Tasks;

internal class TransferToExternalProviderUseCase(IGetOperatorUseCase getOperatorUseCase, 
    IExternalOperatorNotifier externalOperatorNotifier,
    IUserTransferCompleteNotification userTransferCompleteNotification,
    ILogger<TransferToExternalProviderUseCase> logger) : ITransferToExternalProviderUseCase
{
    public async Task TryTransferAsync(Transfer transfer)
    {
        if (!transfer.IsReadyToBeTransferred())
        {
            logger.LogInformation("Transfer with userId {Id} is not ready to be transferred", transfer.UserId);
            return;
        }

        var (requestedOperator, transferReplyUrl) = await getOperatorUseCase.GetOperatorAsync(transfer.ExternalOperatorId);
        if (requestedOperator is null)
        {
            throw new OperatorNotFoundException();
        }
        if(string.IsNullOrEmpty(transferReplyUrl))
        {
            throw new TransferReplyUrlNotFoundException();
        }
        var externalOperatorUrl = requestedOperator.TransferAPIURL;
        if (string.IsNullOrEmpty(externalOperatorUrl))
        {
            throw new ExternalOperatorTransferUrlNotFoundException();
        }
        var transferPackage = TransferPackageDto.BuildFromTransfer(transfer, transferReplyUrl);
        userTransferCompleteNotification.Notify(new UserTransferCompleteDto { Email = transfer.Email, Name = transfer.UserName?? "User", TicketId = transfer.Id }, transfer.UserId);
        await externalOperatorNotifier.NotifyAsync(transferPackage, externalOperatorUrl);
    }
}
