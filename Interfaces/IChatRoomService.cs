using RealtimeChat.DTOs.ChatRooms;

namespace RealtimeChat.Interfaces
{
    public interface IChatRoomService
    {
        Task<int> CreateAsync(CreateChatRoomRequest request, int userId);

        Task<List<ChatRoomResponse>> GetAllAsync();

        Task<ChatRoomResponse?> GetByIdAsync(int id);
    }
}
