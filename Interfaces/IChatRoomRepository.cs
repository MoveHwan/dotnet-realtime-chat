using RealtimeChat.Models;

namespace RealtimeChat.Interfaces
{
    public interface IChatRoomRepository
    {
        Task<ChatRoom> CreateAsync(ChatRoom chatRoom);

        Task<List<ChatRoom>> GetAllAsync();

        Task<ChatRoom?> GetByIdAsync(int id);
    }
}
