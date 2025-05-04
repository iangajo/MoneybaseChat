using MoneybaseChat.Domain.Entities;

namespace MoneybaseChat.Domain.Interfaces
{
    public interface IAgentQueueManagerService
    {
        Task AssignChatSession(int agentId, Guid sessionId);
        Task<ChatSession?> GetSessionBySessionId(Guid sessionId);
        Task<List<ChatSession>> GetAllSessions();
    }
}
