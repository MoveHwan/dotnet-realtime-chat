using Microsoft.EntityFrameworkCore;
using RealtimeChat.Data;
using RealtimeChat.Interfaces;
using RealtimeChat.Models;

namespace RealtimeChat.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByNameAsync(string name)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Name == name);
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }


        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<RefreshToken?> GetRefreshTokenAsync(string token)
        {
            return await _context.RefreshTokens
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Token == token);
        }
    }
}
