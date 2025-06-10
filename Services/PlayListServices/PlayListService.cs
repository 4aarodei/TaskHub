using Microsoft.AspNetCore.SignalR;
using System.Globalization;
using System.Reflection.Emit;
using TaskHub.Controllers;
using TaskHub.Hubs;
using TaskHub.Models.Playlist;

namespace TaskHub.Services.PlayListServices;

public class PlaylistService
{
    private static readonly Dictionary<string, int> _progressBySession = new();
    private readonly IHubContext<ProgressHub> _hubContext;
    private readonly ILogger<PlayListController> _logger; // Вже має бути


    public PlaylistService(IHubContext<ProgressHub> hubContext, ILogger<PlayListController> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }

    public async Task BuildDefaultPlaylistsAsync(List<PlayListQuery> queries, string connectionId,
        CancellationToken cancellationToken)
    {

        _logger.LogInformation("real generation start");


        int total = queries.Count;

        await _hubContext.Clients.Client(connectionId)
            .SendAsync("ReceiveProgress", 0, cancellationToken: cancellationToken);

        for (int i = 0; i < queries.Count; i++)
        {
            _logger.LogInformation("generated progress - {Progress}", i + 1);

            await Task.Delay(2000, cancellationToken); // Симуляція генерації

            int progress = (int)(((i + 1) / (double)total) * 100);

            await _hubContext.Clients.Client(connectionId)
                .SendAsync("ReceiveProgress", progress, cancellationToken: cancellationToken);
        }

        _logger.LogInformation("Generations end");

        // Гарантовано надсилаємо 100% в кінці
        await _hubContext.Clients.Client(connectionId)
            .SendAsync("ReceiveProgress", 100, cancellationToken: cancellationToken);

    }


    public int GetProgress(string sessionId)
    {
        return _progressBySession.TryGetValue(sessionId, out var progress) ? progress : 0;
    }

}