using Microsoft.EntityFrameworkCore;
using RealtimeChat.Data;
using RealtimeChat.Interfaces;
using RealtimeChat.Models;

namespace RealtimeChat.Repositories
{
    public class MessageReadRepository : IMessageReadRepository
    {
        private readonly AppDbContext _context;

        public MessageReadRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<MessageRead> CreateAsync(MessageRead messageRead)
        {
            _context.MessageReads.Add(messageRead);

            await _context.SaveChangesAsync();

            return messageRead;
        }

        public async Task<bool> ExistsAsync(int messageId, int userId)
        {
            return await _context.MessageReads
                .AnyAsync(x => x.MessageId == messageId && x.UserId == userId);
        }
    }
}