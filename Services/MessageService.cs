using RealtimeChat.DTOs.Messages;
using RealtimeChat.Interfaces;
using RealtimeChat.Models;

namespace RealtimeChat.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;

        public MessageService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task<int> CreateAsync(CreateMessageRequest request, int userId)
        {
            // 메시지 엔티티 생성
            var message = new Message
            {
                ChatRoomId = request.ChatRoomId,
                SenderId = userId, // JWT에서 가져온 현재 로그인 사용자
                Content = request.Content
            };

            // DB 저장
            var result = await _messageRepository.CreateAsync(message);

            return result.Id;
        }

        public async Task<List<MessageResponse>> GetByRoomIdAsync(int roomId)
        {
            var messages = await _messageRepository.GetByRoomIdAsync(roomId);

            return messages.Select(m => new MessageResponse
            {
                Id = m.Id,
                SenderId = m.SenderId,
                Content = m.Content,
                SentAt = m.SentAt
            }).ToList();
        }

    }
}