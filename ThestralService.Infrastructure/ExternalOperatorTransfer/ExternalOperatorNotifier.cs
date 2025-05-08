namespace ThestralService.Infrastructure.ExternalOperatorTransfer;

using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Polly;
using Exceptions;
using Domain.Transfer.Dtos;
using Domain.Transfer;
using Microsoft.Extensions.Logging;

internal class ExternalOperatorNotifier(IHttpClientFactory httpClientFactory, IAsyncPolicy<HttpResponseMessage> retryPolicy, ILogger<ExternalOperatorNotifier> logger) : IExternalOperatorNotifier
{
    private readonly HttpClient _externalOperatorsClient = httpClientFactory.CreateClient("ExternalOperators");

    public async Task NotifyAsync(TransferPackageDto packageDto, string externalOperatorUrl)
    {
        string jsonData = JsonSerializer.Serialize(packageDto);
        Exception? lastException = null;

        try
        {
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await _externalOperatorsClient.PostAsync(externalOperatorUrl, content);
            logger.LogInformation("Response: {response}", response.Content.ReadAsStringAsync());
            response.EnsureSuccessStatusCode();
            return;
        }
        catch (Exception ex)
        {
            lastException = ex;
            await retryPolicy.ExecuteAsync(() =>
            {
                if (ex is not HttpRequestException httpException) throw lastException;
                var statusCode = httpException?.StatusCode;
                if (statusCode != System.Net.HttpStatusCode.RequestTimeout &&
                    statusCode != System.Net.HttpStatusCode.ServiceUnavailable) throw lastException;
                logger.LogError("External operator unreachable: {url}", externalOperatorUrl);
                throw new OperatorUnreachableException($"Operator unreachable: {externalOperatorUrl}", lastException);
            });
        }
    }
}
