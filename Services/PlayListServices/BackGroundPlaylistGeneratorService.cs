using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TaskHub;
using TaskHub.Models.Playlist.NewBackGroungLogic; // Переконайся, що ім'я простору імен відповідає твоєму проекту

namespace TaskHub.Services
{
    public interface IPlaylistGeneratorService
    {
        Task GenerateAsync(string connectionId, DateTime startDate, DateTime endDate, CancellationToken token);
    }
    
    public class PlaylistGeneratorService : IPlaylistGeneratorService
    {
        private readonly IHubContext<ProgressHub> _hub;

        public PlaylistGeneratorService(IHubContext<ProgressHub> hub)
        {
            _hub = hub;
        }

        public async Task GenerateAsync(string connectionId, DateTime startDate, DateTime endDate, CancellationToken token)
        {
            int totalSteps = 10; // Припустимо, що генерація складається з 10 кроків

            for (int i = 1; i <= totalSteps; i++)
            {
                token.ThrowIfCancellationRequested(); // Перевірка на скасування

                // Імітація важкої роботи
                await Task.Delay(500, token);

                int progress = (int)((i / (double)totalSteps) * 100);

                // Відправка прогресу клієнту через SignalR
                await _hub.Clients.Client(connectionId).SendAsync("ReceiveProgress", progress, token);
            }

            // Повідомлення про завершення генерації
            await _hub.Clients.Client(connectionId).SendAsync("GenerationCompleted", token);
        }
    }
}