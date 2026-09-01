using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealtimeChat.Models
{
    public class ChatRoomUser
    {
        public int Id { get; set; }

        public int ChatRoomId { get; set; }

        public int UserId { get; set; }

        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(ChatRoomId))]
        public ChatRoom? ChatRoom { get; set; }

        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }
    }
}