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
        public DbSet<ChatRoomUser> ChatRoomUsers => Set<ChatRoomUser>();
        public DbSet<MessageRead> MessageReads => Set<MessageRead>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 같은 사용자가 같은 채팅방에 중복 참가하는 것을 방지
            modelBuilder.Entity<ChatRoomUser>()
                .HasIndex(x => new { x.ChatRoomId, x.UserId })
                .IsUnique();

            modelBuilder.Entity<MessageRead>()
                .ToTable("MessageRead");

            modelBuilder.Entity<MessageRead>()
               .HasKey(x => new { x.MessageId, x.UserId });
        }
    }
}
