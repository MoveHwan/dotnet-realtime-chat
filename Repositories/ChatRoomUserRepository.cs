using Microsoft.EntityFrameworkCore;
using RealtimeChat.Data;
using RealtimeChat.Interfaces;
using RealtimeChat.Models;

namespace RealtimeChat.Repositories
{
    public class ChatRoomUserRepository : IChatRoomUserRepository
    {
        private readonly AppDbContext _context;

        public ChatRoomUserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ChatRoomUser> CreateAsync(ChatRoomUser chatRoomUser)
        {
            _context.ChatRoomUsers.Add(chatRoomUser);
            await _context.SaveChangesAsync();

            return chatRoomUser;
        }

        public async Task<bool> ExistsAsync(int roomId, int userId)
        {
            return await _context.ChatRoomUsers
                .AnyAsync(x => x.ChatRoomId == roomId && x.UserId == userId);
        }

        public async Task<List<ChatRoomUser>> GetUsersByRoomIdAsync(int roomId)
        {
            return await _context.ChatRoomUsers
                .Include(x => x.User)
                .Where(x => x.ChatRoomId == roomId)
                .ToListAsync();
        }

        public async Task<ChatRoomUser?> GetAsync(int roomId, int userId)
        {
            return await _context.ChatRoomUsers
                .FirstOrDefaultAsync(x => x.ChatRoomId == roomId && x.UserId == userId);
        }

        public async Task DeleteAsync(ChatRoomUser chatRoomUser)
        {
            _context.ChatRoomUsers.Remove(chatRoomUser);
            await _context.SaveChangesAsync();
        }
    }
}