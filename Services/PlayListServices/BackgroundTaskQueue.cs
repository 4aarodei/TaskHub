using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace TaskHub.Services.PlayListServices
{
    public interface IBackgroundTaskQueue
    {
        void Enqueue(Func<CancellationToken, Task> task);
        Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken);
    }

    public class BackgroundTaskQueue : IBackgroundTaskQueue
    {
        private readonly Channel<Func<CancellationToken, Task>> _queue = Channel.CreateUnbounded<Func<CancellationToken, Task>>();

        public void Enqueue(Func<CancellationToken, Task> task)
        {
            _queue.Writer.TryWrite(task);
        }

        public async Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken)
        {
            return await _queue.Reader.ReadAsync(cancellationToken);
        }
    }
}
