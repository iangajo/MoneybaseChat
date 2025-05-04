using MoneybaseChat.Domain.Entities;
using MoneybaseChat.Domain.Interfaces;
using System.Collections.Concurrent;

namespace MoneybaseChat.Application.Services
{
    internal class SessionQueueService : ISessionQueueService
    {
        private readonly ConcurrentQueue<ChatSession> _sessionQueue = new();

        public async Task AddSession(ChatSession session)
        {
            _sessionQueue.Enqueue(session);

            await Task.CompletedTask;
        }

        public async Task<int> GetCurrentSessionCount()
        {
            var count = _sessionQueue.Count();

            return await Task.FromResult(count);
        }

        public async Task<ChatSession?> RemoveSession()
        {
            _sessionQueue.TryDequeue(out var sessionQueue);

            if (sessionQueue is null) return null;

            return await Task.FromResult(sessionQueue);
        }

        public async Task<ChatSession?> TryPeek()
        {
            _sessionQueue.TryPeek(out var sessionQueue);

            return await Task.FromResult(sessionQueue);
        }
    }
}
