namespace ThestralService.Domain.Transfer;

using User.Dtos;

public interface ITransferNotifier
{
    void NotifyTransferToInternalServices(string userId, UserTransferRequestDto transferRequest, string messageType);
}
