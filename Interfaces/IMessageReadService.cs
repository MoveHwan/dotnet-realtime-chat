using RealtimeChat.Models;

namespace RealtimeChat.Interfaces
{
    public interface IMessageReadService
    {
        Task<string> MarkAsReadAsync(int messageId, int userId);
    }
}