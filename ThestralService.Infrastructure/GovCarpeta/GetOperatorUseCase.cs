namespace ThestralService.Infrastructure.GovCarpeta;

using Domain.GovCarpeta.Dtos;
using Domain.GovCarpeta;
using System.Threading.Tasks;
using Infrastructure.Cache;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Infrastructure.Configuration;

internal class GetOperatorUseCase(IOperatorsHttpClient operatorsHttpClient, IOptions<BaseTransferReplyUrl> options, ICacheStore cacheStore, ILogger<GetOperatorUseCase> logger) : IGetOperatorUseCase
{
    private readonly BaseTransferReplyUrl _baseTransferUrl = options.Value;

    public async Task<(OperatorDto? requestedOperator, string replyTransferUrl)> GetOperatorAsync(string operatorId)
    {
        var operators = await GetOperatorsFromGovCarpetaAsync();
        return (operators?.SingleOrDefault(op => op.OperatorId == operatorId), _baseTransferUrl.Url);
    }

    public async Task<OperatorDto[]?> GetOperatorsAsync()
    {
        var operators = await GetOperatorsFromGovCarpetaAsync();
        return operators;
    }

    private async Task<OperatorDto[]?> GetOperatorsFromGovCarpetaAsync()
    {
        var operators = await TryGetOperatorsFromCacheAsync();

        if (operators == null || operators.Length == 0)
        {
            operators = await FetchOperatorsAsync();
            if (operators != null && operators.Length > 0)
            {
                await SaveOperatorsToCacheAsync(operators);
            }
        }

        return operators ?? [];
    }

    private async Task<OperatorDto[]?> TryGetOperatorsFromCacheAsync()
    {
        try
        {
            var serializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return await cacheStore.GetAsync<OperatorDto[]>("transferOperators", CancellationToken.None);
        }
        catch (Exception ex)
        {
            logger.LogInformation($"Error retrieving operators from cache: {ex.Message}");
            return null;
        }
    }

    private async Task SaveOperatorsToCacheAsync(OperatorDto[] operators)
    {
        try
        {
            await cacheStore.SaveAsync("transferOperators", operators, CancellationToken.None, TimeSpan.FromHours(10));
        }
        catch (Exception ex)
        {
            logger.LogInformation("Error saving operators to cache: {message}", ex.Message);
        }
    }

    private async Task<OperatorDto[]?> FetchOperatorsAsync()
    {
        return await operatorsHttpClient.ExecuteAsync(); 
    }
}
