using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MoneybaseChat.Application.HostedServices;
using MoneybaseChat.Application.Services;
using MoneybaseChat.Application.UseCases;
using MoneybaseChat.Application.UseCases.Interfaces;
using MoneybaseChat.Domain.Interfaces;

namespace MoneybaseChat.Application
{
    public static class DependencyInjection
    {
        public static void AddApplicationSerivice(this IHostApplicationBuilder builder)
        {
            builder.Services.AddSingleton<IAgentQueueManagerService, AgentQueueManagerService>();
            builder.Services.AddSingleton<ISessionQueueService, SessionQueueService>();

            builder.Services.AddScoped<IAssignChatsUseCase, AssignChatsUseCase>();

            builder.Services.Configure<HostOptions>(x =>
            {
                x.StartupTimeout = TimeSpan.FromSeconds(5);
                x.ServicesStartConcurrently = true;
            });

            builder.Services.AddHostedService<AgentChatCoordinatorService>();

        }
    }
}
