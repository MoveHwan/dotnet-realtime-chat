using System.Security.Claims;
using RealtimeChat.Interfaces;

namespace RealtimeChat.Services
{
    public class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int UserId
        {
            get
            {
                var userId = _httpContextAccessor.HttpContext?
                    .User
                    .FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId is null)
                {
                    throw new UnauthorizedAccessException();
                }

                return int.Parse(userId);
            }
        }
    }
}