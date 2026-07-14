using RealtimeChat.DTOs.Messages;
using RealtimeChat.Models;

namespace RealtimeChat.Interfaces
{
    public interface IMessageService
    {
        Task<int> CreateAsync(CreateMessageRequest request, int userId);
        Task<List<MessageResponse>> GetByRoomIdAsync(int roomId);
    }
}