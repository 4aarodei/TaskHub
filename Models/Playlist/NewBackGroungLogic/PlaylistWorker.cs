using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using TaskHub.Services;
using TaskHub.Services;

namespace TaskHub.Workers
{
    public class PlaylistWorker : BackgroundService
    {
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly ILogger<PlaylistWorker> _logger;

        public PlaylistWorker(IBackgroundTaskQueue taskQueue, ILogger<PlaylistWorker> logger)
        {
            _taskQueue = taskQueue;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Playlist Worker запущений.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // Деактивуємо завдання з черги
                    var workItem = await _taskQueue.DequeueAsync(stoppingToken);

                    _logger.LogInformation("Виконую завдання...");
                    await workItem(stoppingToken);
                    _logger.LogInformation("Завдання виконано.");
                }
                catch (OperationCanceledException)
                {
                    // Обробка скасування
                    _logger.LogInformation("Playlist Worker зупинено.");
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Помилка при виконанні фонової задачі.");
                }
            }
        }
    }
}