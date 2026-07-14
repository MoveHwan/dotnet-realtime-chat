using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using RealtimeChat.DTOs.Messages;
using RealtimeChat.Hubs;
using RealtimeChat.Interfaces;
using System.Security.Claims;

namespace RealtimeChat.Controllers
{
    [ApiController]
    [Route("api/messages")]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly IHubContext<ChatHub> _hubContext;

        public MessageController(IMessageService messageService, IHubContext<ChatHub> hubContext)
        {
            _messageService = messageService;
            _hubContext = hubContext;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateMessageRequest request)
        {
            // JWT에서 현재 로그인한 사용자 ID 조회
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim is null)
                return Unauthorized();
            
            var userId = int.Parse(userIdClaim.Value);

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

            return Ok(new{Id = messageId});
        }

        [HttpGet("room/{roomId}")]
        public async Task<IActionResult> GetMessages(int roomId)
        {
            var messages = await _messageService.GetByRoomIdAsync(roomId);

            return Ok(messages);
        }
    }
}