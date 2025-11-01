using DotNetEnv;
using SpotifyApp.Config;
using SpotifyApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

try { Env.Load(); } catch { }

builder.Services.AddSingleton(new SpotifyConfig
{
    ClientId = Environment.GetEnvironmentVariable("SPOTIFY_CLIENT_ID") ?? "",
    ClientSecret = Environment.GetEnvironmentVariable("SPOTIFY_CLIENT_SECRET") ?? "",
    RedirectUrl = Environment.GetEnvironmentVariable("FRONTEND_URI") + "/callback" ?? ""
});

builder.Services.AddHttpClient<SpotifyService>();

builder.Services.AddControllers();

var app = builder.Build();
app.UseCors();

app.MapControllers();

app.Run();


