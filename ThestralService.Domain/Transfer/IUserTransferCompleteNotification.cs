namespace ThestralService.Domain.Transfer;

using Domain.Transfer.Dtos;

public interface IUserTransferCompleteNotification
{
    void Notify(UserTransferCompleteDto userTransferCompleteDto, string userId);
}
