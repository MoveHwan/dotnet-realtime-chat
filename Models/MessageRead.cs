namespace RealtimeChat.Models
{
    public class MessageRead
    {
        public int MessageId { get; set; }

        public int UserId { get; set; }

        public DateTime ReadAt { get; set; } = DateTime.UtcNow;

        public Message? Message { get; set; }

        public User? User { get; set; }
    }
}