namespace RealtimeChat.DTOs.ChatRooms
{
    public class ChatRoomUserResponse
    {
        public int UserId { get; set; }

        public string UserName { get; set; } = string.Empty;

        public DateTime JoinedAt { get; set; }
    }
}