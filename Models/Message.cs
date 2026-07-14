namespace RealtimeChat.Models
{
    public class Message
    {
        public int Id { get; set; }

        public int ChatRoomId { get; set; }

        // 메시지를 보낸 사용자
        public int SenderId { get; set; }

        public string Content { get; set; } = string.Empty;

        // 메시지 전송 시각 (UTC 기준)
        public DateTime SentAt { get; set; } = DateTime.UtcNow;

    }
}
