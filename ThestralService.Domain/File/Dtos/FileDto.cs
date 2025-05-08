using System.Text.Json.Serialization;

namespace ThestralService.Domain.File.Dtos;

public class FileDto
{
    [JsonPropertyName("files")]
    public required Dictionary<string, string[]> Files { get; set; }
}
