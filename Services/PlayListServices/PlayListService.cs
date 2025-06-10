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
        _logger.LogInformation("Real generation start for connectionId: {ConnectionId}", connectionId);
        var random = new Random(); // Для симуляції випадкової помилки

        try
        {
            int total = queries.Count;
            await _hubContext.Clients.Client(connectionId)
                .SendAsync("ReceiveProgress", 0, cancellationToken: cancellationToken);

            for (int i = 0; i < queries.Count; i++)
            {
                // Перевірка, чи не було скасовано операцію
                cancellationToken.ThrowIfCancellationRequested();

                _logger.LogInformation("Generated progress - {Progress}", i );

                // --- СИМУЛЯЦІЯ ПОМИЛКИ ---
                // Створюємо 25% ймовірність помилки (1 з 10 шансів)
                if (random.Next(0, 10) == 0)
                {
                    throw new InvalidOperationException($"Simulated critical error during playlist generation for item {i + 1}.");
                }

                await Task.Delay(2000, cancellationToken); // Симуляція генерації

                int progress = (int)(((i + 1) / (double)total) * 100);
                await _hubContext.Clients.Client(connectionId)
                    .SendAsync("ReceiveProgress", progress, cancellationToken: cancellationToken);
            }

            _logger.LogInformation("Generations end successfully for {ConnectionId}", connectionId);

            // Гарантовано надсилаємо 100% в кінці, лише якщо все пройшло успішно
            await _hubContext.Clients.Client(connectionId)
                .SendAsync("ReceiveProgress", 100, cancellationToken: cancellationToken);
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Playlist generation was canceled for connection {ConnectionId}.", connectionId);
            // Опціонально, можна повідомити клієнта про скасування
            await _hubContext.Clients.Client(connectionId)
                .SendAsync("ReceiveError", "Generation was canceled by the user.");
        }
        catch (Exception ex)
        {
            // --- ОБРОБКА ПОМИЛКИ ---
            // 1. Записуємо помилку в логер з усіма деталями (включаючи stack trace)
            _logger.LogError(ex, "A critical error occurred during playlist generation for connection {ConnectionId}", connectionId);

            // 2. Надсилаємо клієнту повідомлення про помилку
            await _hubContext.Clients.Client(connectionId)
                .SendAsync("ReceiveError", "An error occurred during playlist generation. Please try again.", cancellationToken);
        }
    }


    public int GetProgress(string sessionId)
    {
        return _progressBySession.TryGetValue(sessionId, out var progress) ? progress : 0;
    }

}