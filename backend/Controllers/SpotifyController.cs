using Microsoft.AspNetCore.Mvc;
using SpotifyApp.Config;
using SpotifyApp.Services;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SpotifyController : ControllerBase
{
    private readonly SpotifyConfig spotifyConfig;
    private readonly SpotifyService spotifyService;
    public SpotifyController(SpotifyConfig spotifyConfig, SpotifyService spotifyService)
    {
        this.spotifyConfig = spotifyConfig;
        this.spotifyService = spotifyService;
    }

    [HttpGet("login")]
    public IActionResult Login()
    {
        var clientId = spotifyConfig.ClientId;
        var redirectUrl = spotifyConfig.RedirectUrl;

        if (string.IsNullOrWhiteSpace(clientId) || string.IsNullOrWhiteSpace(redirectUrl))
        {
            return Problem(detail: "Missing Spotify client configuration (ClientId or FrontendUrl).");
        }

        var scopes = "user-top-read";
        var authorizeUrl = $"https://accounts.spotify.com/authorize" +
                           $"?client_id={Uri.EscapeDataString(clientId)}" +
                           $"&response_type=code" +
                           $"&redirect_uri={Uri.EscapeDataString(redirectUrl)}" +
                           $"&scope={Uri.EscapeDataString(scopes)}";

        return Ok(new { url = authorizeUrl });
    }

    public record CodeRequest(string Code);

    [HttpPost("exchange")]
    public IActionResult ExchangeCode([FromBody] CodeRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Code))
            return Problem(detail: "Missing", statusCode: 400);

        using var client = new HttpClient();

        var authHeader = Convert.ToBase64String(
            Encoding.UTF8.GetBytes($"{spotifyConfig.ClientId}:{spotifyConfig.ClientSecret}")
        );
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeader);
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var postData = new Dictionary<string, string>
        {
            ["grant_type"] = "authorization_code",
            ["code"] = request.Code,
            ["redirect_uri"] = spotifyConfig.RedirectUrl
        };

        var response = client.PostAsync(
            "https://accounts.spotify.com/api/token",
            new FormUrlEncodedContent(postData)
        ).Result;

        var content = response.Content.ReadAsStringAsync().Result;

        if (!response.IsSuccessStatusCode)
            return StatusCode((int)response.StatusCode, content);

        return Ok(content);
    }

    [HttpGet("top-tracks")]
    public async Task<IActionResult> GetTopTracks([FromHeader(Name = "Token")] string? token)
    {
        spotifyService.SetToken(token ?? "");

        try
        {
            var response = await spotifyService.GetTopTracksAsync();
            return Ok(response);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Forbidden)
        {
            return StatusCode(403, "Spotify API access forbidden");
        }
        catch (HttpRequestException ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    
    [HttpGet("top-artists")]
    public async Task<IActionResult> GetTopArtists([FromHeader(Name = "Token")] string? token)
    {
        spotifyService.SetToken(token ?? "");

        try
        {
            var response = await spotifyService.GetTopArtistsAsync();
            return Ok(response);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Forbidden)
        {
            return StatusCode(403, "Spotify API access forbidden");
        }
        catch (HttpRequestException ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
