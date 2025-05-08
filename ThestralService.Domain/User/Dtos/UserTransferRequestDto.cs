using System.Text.Json.Serialization;

namespace ThestralService.Domain.User.Dtos;

public class UserTransferRequestDto
{
    [JsonPropertyName("externalOperatorId")]
    public required string ExternalOperatorId { get; set; }
    [JsonPropertyName("userEmail")]
    public required string UserEmail { get; set; }
}
