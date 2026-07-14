using RealtimeChat.Models;

namespace RealtimeChat.Interfaces
{
    public interface IMessageRepository
    {
        Task<Message> CreateAsync(Message message);
        Task<List<Message>> GetByRoomIdAsync(int roomId);
    }
}
