using MoneybaseChat.Domain.Entities;

namespace MoneybaseChat.Domain.Interfaces
{
    public interface IAgentsService
    {
        Task<Agent?> GetAgentToAssingChatSession(bool isOverflow = false, DateTime? systemDate = null);

        Task<int> GetAgentsCapacityOfOnShiftAsync();

        Task UpdateCurrentChat(int agentId);
    }
}
