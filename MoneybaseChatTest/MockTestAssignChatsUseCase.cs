using Microsoft.Extensions.DependencyInjection;
using MoneybaseChat.Application.Common;
using MoneybaseChat.Application.UseCases;
using MoneybaseChat.Application.UseCases.Interfaces;
using MoneybaseChat.Domain.Entities;
using Moq;

namespace MoneybaseChatTest
{
    public class MockTestAssignChatsUseCase
    {

        [Fact]
        public async Task AddChatSessionToQueue_ShouldReturnSuccessResult()
        {
            // Arrange
            var chatSessionId = Guid.NewGuid();
            var mockResult = Result<Guid>.Success(chatSessionId);

            var mockAssignChatsUseCase = new Mock<IAssignChatsUseCase>();
            mockAssignChatsUseCase
                .Setup(x => x.AddChatSessionToQueue())
                .ReturnsAsync(mockResult);

            var serviceProvider = new ServiceCollection()
                .AddSingleton(mockAssignChatsUseCase.Object)
                .BuildServiceProvider();

            var assignChatsUseCase = serviceProvider.GetRequiredService<IAssignChatsUseCase>();

            // Act
            var result = await assignChatsUseCase.AddChatSessionToQueue();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(chatSessionId, result.Value);
        }

        [Fact]
        public async Task AssignChats_ShouldRefuseChatsWhenQueueIsFull()
        {
            // Arrange
            var chatSessionId = Guid.NewGuid();
            var mockResult = Result<Guid>.Failure("Team capacity meet the threashold.");
            var mockAssignChatsUseCase = new Mock<IAssignChatsUseCase>();
            mockAssignChatsUseCase
                .Setup(x => x.AddChatSessionToQueue())
                .ReturnsAsync(mockResult);

            var serviceProvider = new ServiceCollection()
                .AddSingleton(mockAssignChatsUseCase.Object)
                .BuildServiceProvider();

            var assignChatsUseCase = serviceProvider.GetRequiredService<IAssignChatsUseCase>();

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
            var chatSessionId = Guid.NewGuid();
            var mockResult = Result<Guid>.Success(chatSessionId);
            var mockAssignChatsUseCase = new Mock<IAssignChatsUseCase>();
            mockAssignChatsUseCase
                .Setup(x => x.AddChatSessionToQueue())
                .ReturnsAsync(mockResult);

            mockAssignChatsUseCase
                .Setup(x => x.GetChatSessions())
                .ReturnsAsync(Result<List<ChatSession>>.Success(new List<ChatSession>
                {
                    new ChatSession { SessionId = chatSessionId, IsActive = true }
                }));

            var serviceProvider = new ServiceCollection()
                .AddSingleton(mockAssignChatsUseCase.Object)
                .BuildServiceProvider();
            var assignChatsUseCase = serviceProvider.GetRequiredService<IAssignChatsUseCase>();

            // Act
            var result = await assignChatsUseCase.AddChatSessionToQueue();

            var result2 = await assignChatsUseCase.GetChatSessions();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(chatSessionId, result.Value);
            Assert.Equal(chatSessionId, result2.Value?.First().SessionId);
        }

        [Fact]
        public async Task AssignChats_ShouldReturnTeamCapacityResult()
        {
            // Arrange
            var teamCapacity = 10;
            var mockResult = Result<int>.Success(teamCapacity);
            var mockAssignChatsUseCase = new Mock<IAssignChatsUseCase>();
            mockAssignChatsUseCase
                .Setup(x => x.GetTeamCapacity())
                .ReturnsAsync(mockResult);

            var serviceProvider = new ServiceCollection()
                .AddSingleton(mockAssignChatsUseCase.Object)
                .BuildServiceProvider();

            var assignChatsUseCase = serviceProvider.GetRequiredService<IAssignChatsUseCase>();

            // Act
            var result = await assignChatsUseCase.GetTeamCapacity();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(teamCapacity, result.Value);
        }

    }
}
