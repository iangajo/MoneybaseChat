using Microsoft.Extensions.Hosting;
using MoneybaseChat.Domain.Helpers;
using MoneybaseChat.Domain.Interfaces;
using System.Threading;

namespace MoneybaseChat.Application.HostedServices
{
    internal class AgentChatCoordinatorService : BackgroundService
    {
        private readonly IAgentQueueManagerService _agentManagerService;
        private readonly IAgentsService _agentsService;
        private readonly ISessionQueueService _sessionQueueService;

        public AgentChatCoordinatorService(IAgentQueueManagerService agentManagerService, IAgentsService agentsService, ISessionQueueService sessionQueueService)
        {
            _agentsService = agentsService;
            _sessionQueueService = sessionQueueService;
            _agentManagerService = agentManagerService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(5000);
            while (!stoppingToken.IsCancellationRequested)
            {
                await AssignChats();
            }
        }

        public async Task AssignChats()
        {
            while (await _sessionQueueService.GetCurrentSessionCount() > 0)
            {
                var session = await _sessionQueueService.TryPeek();

                if (session == null) continue;
                var sessionCount = await _sessionQueueService.GetCurrentSessionCount();
                var capacity = await _agentsService.GetAgentsCapacityOfOnShiftAsync();
                var isOverFlow = false;
                if (sessionCount >= capacity)
                {
                    isOverFlow = true;
                }

                var agent = await _agentsService.GetAgentToAssingChatSession(isOverFlow);

                if (agent is null) continue;

                await _agentManagerService.AssignChatSession(agent.Id, session.SessionId);
                await _sessionQueueService.RemoveSession();

                await _agentsService.UpdateCurrentChat(agent.Id);

            }
        }
    }
}
