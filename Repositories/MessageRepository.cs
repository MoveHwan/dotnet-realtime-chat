using Microsoft.EntityFrameworkCore;
using RealtimeChat.Data;
using RealtimeChat.Interfaces;
using RealtimeChat.Models;

namespace RealtimeChat.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly AppDbContext _context;

        public MessageRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Message> CreateAsync(Message message)
        {
            // 메시지 저장
            _context.Messages.Add(message);

            await _context.SaveChangesAsync();

            return message;
        }

        public async Task<List<Message>> GetByRoomIdAsync(int roomId)
        {
            return await _context.Messages
                .Where(m => m.ChatRoomId == roomId)
                .OrderBy(m => m.SentAt)
                .ToListAsync();
        }
    }
}
