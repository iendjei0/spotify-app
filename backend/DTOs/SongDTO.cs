namespace SpotifyApp.DTOs;

public record class SongDTO
{
    public string Title { get; init; } = string.Empty;
    public List<string> Artists { get; init; } = new();
}
