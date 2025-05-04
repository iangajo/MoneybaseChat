using Microsoft.Extensions.DependencyInjection;
using MoneybaseChat.Application.Common;
using MoneybaseChat.Application.HostedServices;
using MoneybaseChat.Application.UseCases.Interfaces;
using MoneybaseChat.Domain.Entities;
using Moq;

namespace MoneybaseChatTest
{
    public class TestAssignChatsUseCase : IClassFixture<TestFixture>
    {
        
        [Fact]
        public async Task AddChatSessionToQueue_ShouldReturnSuccessResult()
        {
            // Arrange
            var fixture = new TestFixture();
            var assignChatsUseCase = fixture.ServiceProvider.GetRequiredService<IAssignChatsUseCase>();

            // Act
            var result = await assignChatsUseCase.AddChatSessionToQueue();

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task AssignChats_ShouldRefuseChatsWhenQueueIsFull()
        {
            // Arrange
            var fixture = new TestFixture();
            var assignChatsUseCase = fixture.ServiceProvider.GetRequiredService<IAssignChatsUseCase>();

            var resultCapacity = await assignChatsUseCase.GetTeamCapacity();
            var capacity = resultCapacity.Value;

            for (int i = 0; i < capacity; i++)
            {
                await assignChatsUseCase.AddChatSessionToQueue();
            }

            // Act
            var result = await assignChatsUseCase.AddChatSessionToQueue();

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("Team capacity meet the threashold.", result.Error);
        }

        [Fact]
        public async Task AssignChats_ShouldDistributeChatsBasedOnSeniority()
        {
            // Arrange
            var fixture = new TestFixture();
            var assignChatsUseCase = fixture.ServiceProvider.GetRequiredService<IAssignChatsUseCase>();
            var agentChatCoordinator = fixture.ServiceProvider.GetRequiredService<AgentChatCoordinatorService>();

            for (int i = 0; i < 6; i++)
            {
                await assignChatsUseCase.AddChatSessionToQueue();
            }

            // Act
            await agentChatCoordinator.AssignChats();

            var result = await assignChatsUseCase.GetChatSessions();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(3, result.Value?.Count(s => s.AssignedAgentId == 2));
            Assert.Equal(3, result.Value?.Count(s => s.AssignedAgentId == 3));
        }

    }
}
