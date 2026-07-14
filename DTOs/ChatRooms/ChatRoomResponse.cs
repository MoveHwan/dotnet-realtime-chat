namespace RealtimeChat.DTOs.ChatRooms
{
    public class ChatRoomResponse
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
