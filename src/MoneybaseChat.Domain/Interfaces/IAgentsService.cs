using MoneybaseChat.Domain.Entities;

namespace MoneybaseChat.Domain.Interfaces
{
    public interface IAgentsService
    {
        Task<Agent?> GetAgentToAssingChatSession(bool isOverflow = false);

        Task<int> GetAgentsCapacityOfOnShiftAsync();

        Task UpdateCurrentChat(int agentId);
    }
}
