using RealtimeChat.Models;

namespace RealtimeChat.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByNameAsync(string name);
        Task<User?> GetByIdAsync(int id);

        Task AddAsync(User user);
        Task<RefreshToken?> GetRefreshTokenAsync(string token);
    }
}
