using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealtimeChat.Interfaces;
using System.Security.Claims;

namespace RealtimeChat.Controllers
{
    [ApiController]
    [Route("api/messages")]
    public class MessageReadController : ControllerBase
    {
        private readonly IMessageReadService _messageReadService;

        public MessageReadController(IMessageReadService messageReadService)
        {
            _messageReadService = messageReadService;
        }

        [Authorize]
        [HttpPost("{messageId}/read")]
        public async Task<IActionResult> MarkAsRead(int messageId)
        {
            // JWT에서 현재 로그인한 사용자 ID 조회
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim is null)
                return Unauthorized();

            var userId = int.Parse(userIdClaim.Value);

            var message = await _messageReadService.MarkAsReadAsync(messageId, userId);

            return Ok(message);
        }
    }
}