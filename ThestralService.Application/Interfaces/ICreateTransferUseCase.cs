namespace ThestralService.Application.Interfaces;

using Domain.Transfer.Entities;
using Domain.User.Dtos;

public interface ICreateTransferUseCase
{
    Task<Transfer> ExecuteAsync(string userId, UserTransferRequestDto transferRequestDto);
}
