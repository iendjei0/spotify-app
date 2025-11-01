namespace SpotifyApp.DTOs;

public record class ArtistDTO
{
    public string Name { get; init; } = string.Empty;
    public string imageUrl { get; init; } = string.Empty;
}
