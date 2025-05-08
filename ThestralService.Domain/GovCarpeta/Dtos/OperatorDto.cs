namespace ThestralService.Domain.GovCarpeta.Dtos;

using System.Text.Json.Serialization;

public class OperatorDto
{
    [JsonPropertyName("_id")]
    public string OperatorId { get; set; }
    [JsonPropertyName("operatorName")]
    public string OperatorName { get; set; }
    [JsonPropertyName("transferAPIURL")]
    public string? TransferAPIURL { get; set; }
}
