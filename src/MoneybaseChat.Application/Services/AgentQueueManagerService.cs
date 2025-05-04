using MoneybaseChat.Domain.Entities;
using MoneybaseChat.Domain.Interfaces;
using System.Collections.Concurrent;

namespace MoneybaseChat.Application.Services
{
    internal class AgentQueueManagerService : IAgentQueueManagerService
    {
        private readonly ConcurrentDictionary<int, ConcurrentQueue<ChatSession>> _agentQueue = new();
        public Task AssignChatSession(int agentId, Guid sessionId)
        {
            var chatSession = new ChatSession()
            {
                SessionId = sessionId,
                IsActivve = true,
                AssignedAgentId = agentId,
            };

            var queue = _agentQueue.GetOrAdd(agentId, _ => new ConcurrentQueue<ChatSession>());
            queue.Enqueue(chatSession);

            return Task.CompletedTask;
        }

        public async Task<List<ChatSession>> GetAllSessions()
        {
            var chatSessions = _agentQueue.SelectMany(q => q.Value).ToList();

            if (!chatSessions.Any()) 
            {
                return Enumerable.Empty<ChatSession>().ToList();
            }

            return await Task.FromResult(chatSessions);
        }

        public async Task<ChatSession?> GetSessionBySessionId(Guid sessionId)
        {
            var chatSession = _agentQueue.SelectMany(q => q.Value).FirstOrDefault(s => s.SessionId == sessionId);

            return await Task.FromResult(chatSession);
        }
    }
}
