namespace RealtimeChat.Models
{
    public class ChatRoom
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
