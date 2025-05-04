using Microsoft.Extensions.DependencyInjection;
using MoneybaseChat.Application.HostedServices;
using MoneybaseChat.Application.Services;
using MoneybaseChat.Application.UseCases;
using MoneybaseChat.Application.UseCases.Interfaces;
using MoneybaseChat.Domain.Interfaces;
using MoneybaseChat.Infrastructure.Services;

namespace MoneybaseChatTest
{
    public class TestFixture
    {
        public IServiceProvider ServiceProvider { get; }

        public TestFixture()
        {
            var services = new ServiceCollection();
            services.AddScoped<IAssignChatsUseCase, AssignChatsUseCase>();
            services.AddSingleton<IAgentQueueManagerService, AgentQueueManagerService>();
            services.AddSingleton<ISessionQueueService, SessionQueueService>();
            services.AddSingleton<IAgentsService, AgentsService>();

            services.AddSingleton<AgentChatCoordinatorService>();

            ServiceProvider = services.BuildServiceProvider();
        }
    }
}
