using System.Text.Json.Serialization;

namespace ThestralService.Domain.User.Dtos;

public class UserManagementTransferResponseDto
{
    [JsonPropertyName("userName")]
    public required string Name { get; set; }
}
