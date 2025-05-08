namespace ThestralService.Domain.Transfer.Repositories;

using Entities;

public interface ITransferQueryRepository
{
    Task CreateAsync(Transfer transfer);
    Task UpdateAsync(Transfer transfer);
}
