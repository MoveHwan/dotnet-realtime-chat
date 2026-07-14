using Microsoft.EntityFrameworkCore;
using RealtimeChat.Models;
using System.Collections.Generic;

namespace RealtimeChat.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<ChatRoom> ChatRooms => Set<ChatRoom>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
        public DbSet<Message> Messages => Set<Message>();
    }
}
