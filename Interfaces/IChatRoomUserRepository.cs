using RealtimeChat.Models;

namespace RealtimeChat.Interfaces
{
    public interface IChatRoomUserRepository
    {
        Task<ChatRoomUser> CreateAsync(ChatRoomUser chatRoomUser);

        Task<bool> ExistsAsync(int roomId, int userId);

        Task<List<ChatRoomUser>> GetUsersByRoomIdAsync(int roomId);

        Task DeleteAsync(ChatRoomUser chatRoomUser);

        Task<ChatRoomUser?> GetAsync(int roomId, int userId);
    }
}