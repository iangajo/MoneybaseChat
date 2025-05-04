using MoneybaseChat.Application.Common;
using MoneybaseChat.Domain.Entities;

namespace MoneybaseChat.Application.UseCases.Interfaces
{
    public interface IAssignChatsUseCase
    {
        Task<Result<int>> GetTeamCapacity();
        Task<Result<Guid>> AddChatSessionToQueue();
        Task<Result<ChatSession>> GetSessionBySessionId(Guid sessionId);
        Task<Result<List<ChatSession>>> GetChatSessions();
    }
}
