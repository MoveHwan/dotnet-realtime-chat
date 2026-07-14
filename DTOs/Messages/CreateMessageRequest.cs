namespace RealtimeChat.DTOs.Messages
{
    public class CreateMessageRequest
    {
        public int ChatRoomId { get; set; }

        public string Content { get; set; } = string.Empty;
    }
}
