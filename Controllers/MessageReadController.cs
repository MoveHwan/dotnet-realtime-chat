using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealtimeChat.Interfaces;

namespace RealtimeChat.Controllers
{
    [ApiController]
    [Route("api/messages")]
    public class MessageReadController : ControllerBase
    {
        private readonly IMessageReadService _messageReadService;
        private readonly IUserContext _userContext;

        public MessageReadController(IMessageReadService messageReadService, IUserContext userContext)
        {
            _messageReadService = messageReadService;
            _userContext = userContext;
        }

        [Authorize]
        [HttpPost("{messageId}/read")]
        public async Task<IActionResult> MarkAsRead(int messageId)
        {
            var userId = _userContext.UserId;

            var message = await _messageReadService.MarkAsReadAsync(messageId, userId);

            return Ok(message);
        }
    }
}