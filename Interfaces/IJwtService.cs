using RealtimeChat.Models;

namespace RealtimeChat.Interfaces
{
    public interface IJwtService
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshToken();
    }
}
