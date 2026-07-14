using Microsoft.EntityFrameworkCore;
using RealtimeChat.Data;
using RealtimeChat.Interfaces;
using RealtimeChat.Models;

namespace RealtimeChat.Repositories
{
    public class ChatRoomRepository : IChatRoomRepository
    {
        private readonly AppDbContext _context;

        public ChatRoomRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ChatRoom> CreateAsync(ChatRoom chatRoom)
        {
            _context.ChatRooms.Add(chatRoom);

            await _context.SaveChangesAsync();

            return chatRoom;
        }

        public async Task<List<ChatRoom>> GetAllAsync()
        {
            return await _context.ChatRooms
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<ChatRoom?> GetByIdAsync(int id)
        {
            // 채팅방 ID로 조회
            return await _context.ChatRooms
            .FirstOrDefaultAsync(x => x.Id == id);
        }

    }
}
