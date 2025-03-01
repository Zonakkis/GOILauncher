using System.Text.Json.Serialization;

namespace GOILauncher.Models;

public class Credit
{

    [JsonPropertyName("player")]
    public string Player { get; init; }
}