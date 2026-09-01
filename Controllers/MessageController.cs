using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using RealtimeChat.DTOs.Messages;
using RealtimeChat.Hubs;
using RealtimeChat.Interfaces;

namespace RealtimeChat.Controllers
{
    [ApiController]
    [Route("api/messages")]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly IUserContext _userContext;

        public MessageController(IMessageService messageService, IHubContext<ChatHub> hubContext, IUserContext userContext)
        {
            _messageService = messageService;
            _hubContext = hubContext;
            _userContext = userContext;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateMessageRequest request)
        {
            var userId = _userContext.UserId;

            var messageId = await _messageService.CreateAsync(request, userId);

            // 같은 채팅방에 참여한 사용자들에게 실시간 전송
            await _hubContext.Clients
                .Group(request.ChatRoomId.ToString())
                .SendAsync("ReceiveMessage", new
                {
                    Id = messageId,
                    SenderId = userId,
                    Content = request.Content,
                    SentAt = DateTime.UtcNow
                });

            return Ok(new { Id = messageId });
        }

        [Authorize]
        [HttpGet("room/{roomId}")]
        public async Task<IActionResult> GetMessages(int roomId)
        {
            var userId = _userContext.UserId;

            var messages = await _messageService.GetByRoomIdAsync(roomId, userId);

            return Ok(messages);
        }
    }
}