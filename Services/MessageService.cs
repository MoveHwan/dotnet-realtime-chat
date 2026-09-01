using RealtimeChat.DTOs.Messages;
using RealtimeChat.Interfaces;
using RealtimeChat.Models;

namespace RealtimeChat.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IChatRoomUserRepository _chatRoomUserRepository;

        public MessageService(IMessageRepository messageRepository, IChatRoomUserRepository chatRoomUserRepository)
        {
            _messageRepository = messageRepository;
            _chatRoomUserRepository = chatRoomUserRepository;
        }

        public async Task<int> CreateAsync(CreateMessageRequest request, int userId)
        {
            var isParticipant = await _chatRoomUserRepository.ExistsAsync(request.ChatRoomId, userId);

            // 채팅방 참여자만 메시지 전송 가능
            if (!isParticipant)
            {
                throw new UnauthorizedAccessException("채팅방 참여자만 메시지를 보낼 수 있습니다.");
            }

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

        public async Task<List<MessageResponse>> GetByRoomIdAsync(int roomId, int userId)
        {
            var isParticipant = await _chatRoomUserRepository.ExistsAsync(roomId, userId);

            // 채팅방 참여자만 메시지 조회 가능
            if (!isParticipant)
            {
                throw new UnauthorizedAccessException("채팅방 참여자만 메시지를 조회할 수 있습니다.");
            }

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