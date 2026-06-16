using RealtimeChat.DTOs.Auth;

namespace RealtimeChat.Interfaces
{
    public interface IUserService
    {
        Task RegisterAsync(RegisterRequest request);

        Task<AuthResponse?> LoginAsync(LoginRequest request);
        Task<AuthResponse?> RefreshTokenAsync(string refreshToken);
    }
}
