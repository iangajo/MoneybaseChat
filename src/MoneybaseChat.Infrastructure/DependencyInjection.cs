using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MoneybaseChat.Domain.Interfaces;
using MoneybaseChat.Infrastructure.Services;

namespace MoneybaseChat.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfrastructureService(this IHostApplicationBuilder builder)
        {
            builder.Services.AddSingleton<IAgentsService, AgentsService>();
        }
    }
}
