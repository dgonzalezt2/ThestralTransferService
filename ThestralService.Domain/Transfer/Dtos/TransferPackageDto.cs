namespace ThestralService.Domain.Transfer.Dtos;

using Domain.File.Dtos;
using Transfer.Entities;
using System.Text.Json.Serialization;
using ThestralService.Domain.Transfer.Exceptions;

public class TransferPackageDto
{
    [JsonPropertyName("id")]
    public long Id { get; set; }
    [JsonPropertyName("citizenName")]
    public string Name { get; set; }
    [JsonPropertyName("citizenEmail")]
    public string Email { get; set; }
    [JsonPropertyName("confirmAPI")]
    public string CallbackUrl { get; set; }

    [JsonPropertyName("urlDocuments")]
    public Dictionary<string, string[]> Files { get; set; }

    public static TransferPackageDto BuildFromTransfer(Transfer transfer, string callbackUrl)
    {
        var files = new Dictionary<string, string[]>();
        if(transfer.Files is not null)
        {
            files = transfer.Files;
        }
        var userId = transfer.UserId;
        if (string.IsNullOrWhiteSpace(userId)) 
        {
            throw new IdentificationNumberNotFound();
        }
        if (!long.TryParse(userId, out var userIdentificationNumber))
        {
            throw new IdentificationNumberNotFound();
        }
        var userName = transfer.UserName;
        if (string.IsNullOrWhiteSpace(userName))
        {
            throw new ArgumentNullException(nameof(userName));
        }
        
        return new()
        {
            Id = userIdentificationNumber,
            Email = transfer.Email,
            Files = files,
            Name = userName,
            CallbackUrl = callbackUrl
        };
    }
}
