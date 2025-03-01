using System.Text.Json.Serialization;

namespace GOILauncher.Models;

public class Update
{

    [JsonPropertyName("version")]
    public string Version { get; init; }
    [JsonPropertyName("url")]
    public string Url { get; init; }
    [JsonPropertyName("changelog")]
    public string Changelog { get; init; }
}