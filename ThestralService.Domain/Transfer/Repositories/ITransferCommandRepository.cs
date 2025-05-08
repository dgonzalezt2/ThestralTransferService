
namespace ThestralService.Domain.Transfer.Repositories;
using Entities;

public interface ITransferCommandRepository
{
    Task<bool> ExistsByUserIdAsync(string id);
    Task<Transfer?> GetByUserIdAsync(string id);
}
