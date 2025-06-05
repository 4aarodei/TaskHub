using Microsoft.AspNetCore.SignalR;
using System.Globalization;
using System.Reflection.Emit;
using TaskHub.Hubs;
using TaskHub.Models.Playlist;

namespace TaskHub.Services.PlayListServices;

public class PlaylistService
{
    private static readonly Dictionary<string, int> _progressBySession = new();
    private readonly IHubContext<ProgressHub> _hubContext;
    public PlaylistService(IHubContext<ProgressHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task BuildDefaultPlaylistsAsync(List<PlayListQuery> queries, string connectionId, CancellationToken cancellationToken)
    {
        int total = queries.Count;

        await _hubContext.Clients.Client(connectionId)
            .SendAsync("ReceiveProgress", 0, cancellationToken: cancellationToken);

        for (int i = 0; i < queries.Count; i++)
        {
            await Task.Delay(5000, cancellationToken); // Симуляція генерації

            int progress = (int)(((i + 1) / (double)total) * 100);

            await _hubContext.Clients.Client(connectionId)
                .SendAsync("ReceiveProgress", progress, cancellationToken: cancellationToken);
        }

        // Гарантовано надсилаємо 100% в кінці
        await _hubContext.Clients.Client(connectionId)
            .SendAsync("ReceiveProgress", 100, cancellationToken: cancellationToken);
    }


    public int GetProgress(string sessionId)
    {
        return _progressBySession.TryGetValue(sessionId, out var progress) ? progress : 0;
    }

}