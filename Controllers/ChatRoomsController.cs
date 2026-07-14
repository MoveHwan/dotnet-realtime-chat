using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealtimeChat.DTOs.ChatRooms;
using RealtimeChat.Interfaces;
using System.Security.Claims;

namespace RealtimeChat.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatRoomsController : ControllerBase
    {
        private readonly IChatRoomService _chatRoomService;

        public ChatRoomsController(IChatRoomService chatRoomService)
        {
            _chatRoomService = chatRoomService;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateChatRoomRequest request)
        {
            // JWT에서 현재 로그인한 사용자 ID 조회
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim is null)
            {
                return Unauthorized();
            }
                
            
            var userId = int.Parse(userIdClaim.Value);

            var chatRoomId = await _chatRoomService.CreateAsync(request, userId);

            return Ok(new{Id = chatRoomId});
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var chatRooms = await _chatRoomService.GetAllAsync();

            return Ok(chatRooms);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var chatRoom = await _chatRoomService.GetByIdAsync(id);

            // 요청한 채팅방이 존재하지 않으면 404 반환
            if (chatRoom is null) return NotFound();
            
            return Ok(chatRoom);

        }
    }
}
