using System;
using System.Net.Http.Headers;
using System.Text.Json;
using SpotifyApp.DTOs;

namespace SpotifyApp.Services;

public class SpotifyService
{
    private readonly HttpClient httpClient;

    public SpotifyService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
        httpClient.BaseAddress = new Uri("https://api.spotify.com/v1/");
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public void SetToken(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            throw new ArgumentException("Token cannot be null or whitespace.", nameof(token));
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public async Task<List<SongDTO>> GetTopTracksAsync()
    {
        var response = await httpClient.GetAsync("me/top/tracks?limit=10&time_range=long_term");
        response.EnsureSuccessStatusCode();
        
        var json = await response.Content.ReadAsStringAsync();
        var data = JsonDocument.Parse(json);
        var items = data.RootElement.GetProperty("items");

        var songs = new List<SongDTO>();
        foreach (var track in items.EnumerateArray())
        {
            var artists = new List<string>();
            foreach (var artist in track.GetProperty("artists").EnumerateArray())
            {
                artists.Add(artist.GetProperty("name").GetString() ?? "");
            }

            songs.Add(new SongDTO 
            {
                Title = track.GetProperty("name").GetString() ?? "",
                Artists = artists
            });
        }
        return songs;
    }

    public async Task<List<ArtistDTO>> GetTopArtistsAsync()
    {
        var response = await httpClient.GetAsync("me/top/artists?limit=12&time_range=long_term");
        response.EnsureSuccessStatusCode();
        
        var json = await response.Content.ReadAsStringAsync();
        var data = JsonDocument.Parse(json);
        var items = data.RootElement.GetProperty("items");

        var artists = new List<ArtistDTO>();
        foreach (var artist in items.EnumerateArray())
        {
            var images = artist.GetProperty("images");
            var imageUrl = "";
            if (images.GetArrayLength() > 0)
            {
                imageUrl = images[0].GetProperty("url").GetString() ?? "";
            }

            artists.Add(new ArtistDTO 
            {
                Name = artist.GetProperty("name").GetString() ?? "",
                imageUrl = imageUrl
            });
        }
        return artists;
    }
}
