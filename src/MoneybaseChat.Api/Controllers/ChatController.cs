using Microsoft.AspNetCore.Mvc;
using MoneybaseChat.Application.UseCases.Interfaces;

namespace MoneybaseChat.Api.Controllers
{
    [Route("api/chats")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IAssignChatsUseCase _assignChatUseCase;
        public ChatController(IAssignChatsUseCase assignChatsUseCase)
        {
            _assignChatUseCase = assignChatsUseCase;
        }

        [HttpPost("")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateRequest()
        {
            var result = await _assignChatUseCase.AddChatSessionToQueue();

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok(new { result.Value });
        }

        [HttpGet("")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _assignChatUseCase.GetChatSessions();

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok(new { result.Value });
        }

        [HttpGet("{sessionId:guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetRequest(Guid sessionId)
        {
            var result = await _assignChatUseCase.GetSessionBySessionId(sessionId);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok(new { result.Value });
        }
    }
}
