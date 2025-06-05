using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using TaskHub.Services.PlayListServices;

namespace TaskHub.PlaylistWorker
{
    public class PlaylistWorker : BackgroundService
    {
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly IServiceScopeFactory _scopeFactory;

        public PlaylistWorker(IBackgroundTaskQueue taskQueue, IServiceScopeFactory scopeFactory)
        {
            _taskQueue = taskQueue;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var task = await _taskQueue.DequeueAsync(stoppingToken);

                using var scope = _scopeFactory.CreateScope();
                await task(stoppingToken); // виконуємо задачу
            }
        }
    }
}