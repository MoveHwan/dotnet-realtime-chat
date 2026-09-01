using RealtimeChat.DTOs.ChatRooms;
using RealtimeChat.Interfaces;
using RealtimeChat.Models;

namespace RealtimeChat.Services
{
    public class ChatRoomService : IChatRoomService
    {
        private readonly IChatRoomRepository _chatRoomRepository;
        private readonly IChatRoomUserRepository _chatRoomUserRepository;

        public ChatRoomService(IChatRoomRepository chatRoomRepository, IChatRoomUserRepository chatRoomUserRepository)
        {
            _chatRoomRepository = chatRoomRepository;
            _chatRoomUserRepository = chatRoomUserRepository;
        }

        public async Task<int> CreateAsync(CreateChatRoomRequest request, int userId)
        {
            var chatRoom = new ChatRoom
            {
                Name = request.Name,
                CreatedBy = userId      // 채팅방 생성자 저장
            };

            var result = await _chatRoomRepository.CreateAsync(chatRoom);

            var chatRoomUser = new ChatRoomUser
            {
                ChatRoomId = result.Id,
                UserId = userId
            };

            // 채팅방 생성자는 자동으로 참여자에 등록
            await _chatRoomUserRepository.CreateAsync(chatRoomUser);

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

        public async Task JoinAsync(int roomId, int userId)
        {
            var exists = await _chatRoomUserRepository.ExistsAsync(roomId, userId);

            // 이미 참여 중인 사용자는 다시 참가할 수 없음
            if (exists)
            {
                throw new InvalidOperationException("이미 참여한 채팅방입니다.");
            }

            var chatRoomUser = new ChatRoomUser
            {
                ChatRoomId = roomId,
                UserId = userId
            };

            await _chatRoomUserRepository.CreateAsync(chatRoomUser);
        }

        public async Task LeaveAsync(int roomId, int userId)
        {
            var participant = await _chatRoomUserRepository.GetAsync(roomId, userId);

            // 참여하지 않은 채팅방은 나갈 수 없음
            if (participant is null)
            {
                throw new InvalidOperationException("참여 중인 채팅방이 아닙니다.");
            }

            await _chatRoomUserRepository.DeleteAsync(participant);
        }

        public async Task<List<ChatRoomUserResponse>> GetUsersAsync(int roomId, int userId)
        {
            var isParticipant = await _chatRoomUserRepository.ExistsAsync(roomId, userId);

            // 채팅방 참여자만 참여자 목록을 조회할 수 있음
            if (!isParticipant)
            {
                throw new UnauthorizedAccessException("채팅방 참여자만 참여자 목록을 조회할 수 있습니다.");
            }

            var users = await _chatRoomUserRepository.GetUsersByRoomIdAsync(roomId);

            return users.Select(x => new ChatRoomUserResponse
            {
                UserId = x.UserId,
                UserName = x.User?.Name ?? string.Empty,
                JoinedAt = x.JoinedAt
            }).ToList();
        }
    }
}
