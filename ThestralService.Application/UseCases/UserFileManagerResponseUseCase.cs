namespace ThestralService.Application.UseCases;

using Interfaces;
using System.Text.Json;
using Domain.Transfer.Exceptions;
using Domain.Transfer.Repositories;
using Domain.Transfer;
using Domain.File.Dtos;
using Domain.SharedKernel.Exceptions;
using Microsoft.Extensions.Logging;

public class UserFileManagerResponseUseCase(ITransferCommandRepository transferCommandRepository, ITransferQueryRepository transferQueryRepository, ITransferToExternalProviderUseCase transferToExternalProvider, ILogger<UserFileManagerResponseUseCase> logger) : IInternalServicesResponseUseCase
{
    public TransferOperations UseCase { get; } = TransferOperations.USER_FILE_MANAGER_RESPONSE;

    public async Task ExecuteAsync(string body, string userId)
    {
        logger.LogInformation("Start processing UserFileManager request");
        var filesDto = JsonSerializer.Deserialize<FileDto>(body) ?? throw new InvalidBodyException();
        var transfer = await transferCommandRepository.GetByUserIdAsync(userId) ?? throw new TransferNotFoundException(userId);
        var files = filesDto.Files;
        transfer.SetFiles(files);
        transfer.SetUserFileManagerDisabled();
        await transferQueryRepository.UpdateAsync(transfer);
        await transferToExternalProvider.TryTransferAsync(transfer);
        logger.LogInformation("FileManager request complete");
    }
}
