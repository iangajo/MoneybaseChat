using MoneybaseChat.Domain.Entities;

namespace MoneybaseChat.Domain.Interfaces
{
    public interface ISessionQueueService
    {
        Task AddSession(ChatSession session);
        Task<ChatSession?> RemoveSession();
        Task<int> GetCurrentSessionCount();
        Task<ChatSession?> TryPeek();
    }
}
