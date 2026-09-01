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
        private readonly IUserContext _userContext;

        public ChatRoomsController(IChatRoomService chatRoomService, IUserContext userContext)
        {
            _chatRoomService = chatRoomService;
            _userContext = userContext;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateChatRoomRequest request)
        {
            var userId = _userContext.UserId;

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

        [Authorize]
        [HttpPost("{roomId}/join")]
        public async Task<IActionResult> Join(int roomId)
        {
            var userId = _userContext.UserId;

            await _chatRoomService.JoinAsync(roomId, userId);

            return Ok(new{ Message = "채팅방에 참가했습니다." });
        }

        [Authorize]
        [HttpDelete("{roomId}/leave")]
        public async Task<IActionResult> Leave(int roomId)
        {
            var userId = _userContext.UserId;

            await _chatRoomService.LeaveAsync(roomId, userId);

            return Ok(new{ Message = "채팅방에서 나갔습니다." });
        }

        [Authorize]
        [HttpGet("{roomId}/users")]
        public async Task<IActionResult> GetUsers(int roomId)
        {
            var userId = _userContext.UserId;

            var users = await _chatRoomService.GetUsersAsync(roomId, userId);

            return Ok(users);
        }

    }
}
