namespace RealtimeChat.DTOs.Messages
{
    public class MessageResponse
    {
        public int Id { get; set; }

        public int SenderId { get; set; }

        public string Content { get; set; } = string.Empty;

        public DateTime SentAt { get; set; }
    }
}