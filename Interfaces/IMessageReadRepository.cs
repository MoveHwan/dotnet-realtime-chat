using RealtimeChat.Models;

namespace RealtimeChat.Interfaces
{
    public interface IMessageReadRepository
    {
        Task<MessageRead> CreateAsync(MessageRead messageRead);

        Task<bool> ExistsAsync(int messageId, int userId);
    }
}