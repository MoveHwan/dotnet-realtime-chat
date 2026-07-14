using RealtimeChat.DTOs.ChatRooms;
using RealtimeChat.Interfaces;
using RealtimeChat.Models;

namespace RealtimeChat.Services
{
    public class ChatRoomService : IChatRoomService
    {
        private readonly IChatRoomRepository _chatRoomRepository;

        public ChatRoomService(IChatRoomRepository chatRoomRepository)
        {
            _chatRoomRepository = chatRoomRepository;
        }

        public async Task<int> CreateAsync(CreateChatRoomRequest request, int userId)
        {
            var chatRoom = new ChatRoom
            {
                Name = request.Name,
                CreatedBy = userId      // 채팅방 생성자 저장
            };

            var result = await _chatRoomRepository.CreateAsync(chatRoom);

            return result.Id;
        }

        public async Task<List<ChatRoomResponse>> GetAllAsync()
        {
            var chatRooms = await _chatRoomRepository.GetAllAsync();

            return chatRooms.Select(x => new ChatRoomResponse
            {
                Id = x.Id,
                Name = x.Name,
                CreatedBy = x.CreatedBy,
                CreatedAt = x.CreatedAt

            }).ToList();
        }

        public async Task<ChatRoomResponse?> GetByIdAsync(int id)
        {
            var chatRoom = await _chatRoomRepository.GetByIdAsync(id);

            // 존재하지 않는 채팅방인 경우 null 반환
            if (chatRoom is null) return null;

            return new ChatRoomResponse
            {
                Id = chatRoom.Id,
                Name = chatRoom.Name,
                CreatedBy = chatRoom.CreatedBy,
                CreatedAt = chatRoom.CreatedAt
            };
        }
    }
}
