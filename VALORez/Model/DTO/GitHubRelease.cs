using System.Text.Json.Serialization;

namespace Model.DTO;

public class GitHubRelease
{
    [JsonPropertyName("tag_name")]
    public string? TagName { get; set; } = string.Empty;

    [JsonPropertyName("assets")]
    public List<GitHubAsset>? Assets { get; set; } = new();
}

