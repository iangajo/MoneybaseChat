using MoneybaseChat.Application.Common;
using MoneybaseChat.Application.UseCases.Interfaces;
using MoneybaseChat.Domain.Entities;
using MoneybaseChat.Domain.Interfaces;

namespace MoneybaseChat.Application.UseCases
{
    internal class AssignChatsUseCase : IAssignChatsUseCase
    {
        private readonly IAgentQueueManagerService _agentQueueManagerService;
        private readonly IAgentsService _agentsService;
        private readonly ISessionQueueService _sessionQueueService;

        public AssignChatsUseCase(IAgentsService agentsService, ISessionQueueService sessionQueueService, IAgentQueueManagerService agentQueueManagerService)
        {
            _agentsService = agentsService;
            _sessionQueueService = sessionQueueService;
            _agentQueueManagerService = agentQueueManagerService;
        }

        public async Task<Result<Guid>> AddChatSessionToQueue()
        {
            var sessionId = Guid.NewGuid();
            var chatSession = new ChatSession()
            {
                SessionId = sessionId,
                IsActivve = true,
            };
            var teamCapacity = await _agentsService.GetAgentsCapacityOfOnShiftAsync();

            var sessionCount = await _sessionQueueService.GetCurrentSessionCount();

            if (sessionCount >= teamCapacity) return Result<Guid>.Failure("Team capacity meet the threashold.");

             await _sessionQueueService.AddSession(chatSession);

            return Result<Guid>.Success(sessionId);
        }

        public async Task<Result<List<ChatSession>>> GetChatSessions()
        {
            var sessions = await _agentQueueManagerService.GetAllSessions();

            return Result<List<ChatSession>>.Success(sessions);
        }

        public async Task<Result<ChatSession>> GetSessionBySessionId(Guid sessionId)
        {
            var chatSession = await _agentQueueManagerService.GetSessionBySessionId(sessionId);

            if (chatSession is null) return Result<ChatSession>.Failure("Chat session not yet assign or found.");

            return Result<ChatSession>.Success(chatSession);
        }
    }
}
