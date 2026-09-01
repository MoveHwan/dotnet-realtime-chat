using RealtimeChat.Interfaces;
using RealtimeChat.Models;

namespace RealtimeChat.Services
{
    public class MessageReadService : IMessageReadService
    {
        private readonly IMessageReadRepository _messageReadRepository;

        public MessageReadService(IMessageReadRepository messageReadRepository)
        {
            _messageReadRepository = messageReadRepository;
        }

        public async Task<string> MarkAsReadAsync(int messageId, int userId)
        {
            var alreadyRead = await _messageReadRepository.ExistsAsync(messageId, userId);

            if (alreadyRead)
            {
                return "이미 읽은 메세지입니다.";
            }

            var messageRead = new MessageRead
            {
                MessageId = messageId,
                UserId = userId
            };

            await _messageReadRepository.CreateAsync(messageRead);

            return "성공적으로 메세지를 읽었습니다.";
        }
    }
}