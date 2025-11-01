namespace SpotifyApp.Config;

public record SpotifyConfig
{
    public required string ClientId { get; init; }
    public required string ClientSecret { get; init; }
    public required string RedirectUrl { get; init; }
}
