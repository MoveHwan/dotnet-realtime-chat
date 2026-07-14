using RealtimeChat.Data;
using RealtimeChat.DTOs.Auth;
using RealtimeChat.Interfaces;
using RealtimeChat.Models;

namespace RealtimeChat.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly AppDbContext _context;

        public UserService(IUserRepository userRepository, IJwtService jwtService, AppDbContext context)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _context = context;
        }


        public async Task RegisterAsync(RegisterRequest request)
        {
            var existingUser = await _userRepository.GetByNameAsync(request.Name);

            if (existingUser != null)
            {
                throw new Exception("User already exists.");
            }

            // Password는 원문 저장 금지
            // BCrypt를 사용해 해시 후 저장
            var user = new User
            {
                Name = request.Name,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };

            await _userRepository.AddAsync(user);
        }


        public async Task<AuthResponse?> LoginAsync(LoginRequest request)
        {
            var user = await _userRepository.GetByNameAsync(request.Name);

            if (user == null)
            {
                return null;
            }

            var isValidPassword = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);

            if (!isValidPassword)
            {
                return null;
            }

            var accessToken = _jwtService.GenerateAccessToken(user);

            // Access Token 만료 시 재발급을 위한 Refresh Token 생성
            var refreshToken = _jwtService.GenerateRefreshToken();

            _context.RefreshTokens.Add(new RefreshToken
            {
                UserId = user.Id,
                Token = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            });

            await _context.SaveChangesAsync();

            return new AuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }


        public async Task<AuthResponse?> RefreshTokenAsync(string refreshToken)
        {
            var token = await _userRepository.GetRefreshTokenAsync(refreshToken);

            if (token == null || token.ExpiresAt < DateTime.UtcNow)
            {
                return null;
            }

            var accessToken = _jwtService.GenerateAccessToken(token.User);

            var newRefreshToken = _jwtService.GenerateRefreshToken();

            token.Token = newRefreshToken;
            token.ExpiresAt = DateTime.UtcNow.AddDays(7);

            await _context.SaveChangesAsync();

            return new AuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = newRefreshToken
            };
        }

    }
}
