using Microsoft.AspNetCore.SignalR;

namespace RealtimeChat.Hubs
{
    public class ChatHub : Hub
    {
        public async Task JoinRoom(string roomId)
        {
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