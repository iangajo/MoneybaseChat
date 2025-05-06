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
            var invokeTimes = 6;

            for (int i = 0; i < invokeTimes; i++)
            {
                await assignChatsUseCase.AddChatSessionToQueue();
            }

            // Act
            for (int i = 0; i < invokeTimes; i++)
            {
                await agentChatCoordinator.AssignChats();
            }

            var result = await assignChatsUseCase.GetChatSessions();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(3, result.Value?.Count(s => s.AssignedAgentId == 2));
            Assert.Equal(3, result.Value?.Count(s => s.AssignedAgentId == 3));
        }

        [Fact]
        public async Task AssignChats_ShouldAssignToOverFlowAgent()
        {
            // Arrange
            var fixture = new TestFixture();
            var assignChatsUseCase = fixture.ServiceProvider.GetRequiredService<IAssignChatsUseCase>();
            var agentChatCoordinator = fixture.ServiceProvider.GetRequiredService<AgentChatCoordinatorService>();
            var invokeTimes = 50;

            for (int i = 0; i < invokeTimes; i++)
            {
                await assignChatsUseCase.AddChatSessionToQueue();
            }

            var systemDate = new DateTime(2025, 5, 5, 8, 30, 0);

            // Act
            for (int i = 0; i < invokeTimes; i++)
            {
                await agentChatCoordinator.AssignChats(systemDate);
            }

            //additional session for overflow
            for (int i = 0; i < invokeTimes; i++)
            {
                await assignChatsUseCase.AddChatSessionToQueue();
            }
            //assign to overflow agents
            for (int i = 0; i < invokeTimes; i++)
            {
                await agentChatCoordinator.AssignChats(systemDate);
            }

            var result = await assignChatsUseCase.GetChatSessions();

            // Assert
            Assert.True(result.IsSuccess);
            //mid level
            Assert.Equal(6, result.Value?.Count(s => s.AssignedAgentId == 1));

            //juniors
            Assert.Equal(4, result.Value?.Count(s => s.AssignedAgentId == 2));
            Assert.Equal(4, result.Value?.Count(s => s.AssignedAgentId == 3));

            //overflow agent
            Assert.Equal(1, result.Value?.Count(s => s.AssignedAgentId == 100));
        }

    }
}
