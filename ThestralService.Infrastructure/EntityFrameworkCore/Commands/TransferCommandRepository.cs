namespace ThestralService.Infrastructure.EntityFrameworkCore.Commands;

using DbContext;
using Domain.Transfer.Entities;
using Domain.Transfer.Repositories;
using Microsoft.EntityFrameworkCore;

internal class TransferCommandRepository(TransferDbContext context) : ITransferCommandRepository
{
    public async Task<bool> ExistsByUserIdAsync(string id)
    {
        return await context.Transfers.AnyAsync(u => u.UserId == id);
    }

    public async Task<Transfer?> GetByUserIdAsync(string id)
    {
        return await context.Transfers
            .FirstOrDefaultAsync(u => u.UserId == id);
    }
}
