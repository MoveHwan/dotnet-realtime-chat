using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using RealtimeChat.Interfaces;
using System.Security.Claims;

namespace RealtimeChat.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IChatRoomUserRepository _chatRoomUserRepository;

        public ChatHub(IChatRoomUserRepository chatRoomUserRepository)
        {
            _chatRoomUserRepository = chatRoomUserRepository;
        }

        public async Task JoinRoom(string roomId)
        {
            var userIdClaim = Context.User?.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim is null)
            {
                throw new HubException("로그인한 사용자만 채팅방에 참가할 수 있습니다.");
            }

            var userId = int.Parse(userIdClaim.Value);

            var isParticipant = await _chatRoomUserRepository.ExistsAsync(int.Parse(roomId), userId);

            Console.WriteLine($"JoinRoom - RoomId: {roomId}, UserId: {userId}, Participant: {isParticipant}");

            // 실제 채팅방 참여자만 SignalR 그룹에 참가할 수 있음
            if (!isParticipant)
            {
                throw new HubException("채팅방 참여자만 입장할 수 있습니다.");
            }

            // 채팅방 그룹 참가
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
        }

        public async Task LeaveRoom(string roomId)
        {
            // 채팅방 그룹 나가기
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
        }
    }
}