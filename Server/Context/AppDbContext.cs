using Microsoft.EntityFrameworkCore;

namespace Server.Context;

public sealed class AppDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Data Source=DESKTOP-3BJ5GK9;Initial Catalog=ChatDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Chat> Chats { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<ChatParticipant> ChatParticipants { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChatParticipant>()
            .HasKey(cp => new { cp.UserId, cp.ChatId });        
    }
}

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }    
}

public class Chat
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime CreationDate { get; set; }

    // Relationships
    public ICollection<Message> Messages { get; set; }
    public ICollection<ChatParticipant> ChatParticipants { get; set; }
}

public class Message
{
    public int Id { get; set; }
    public string Text { get; set; }
    public DateTime Timestamp { get; set; }

    // Foreign keys
    public int UserId { get; set; }
    public int ChatId { get; set; }

    // Relationships
    public User User { get; set; }
    public Chat Chat { get; set; }
}

public class ChatParticipant
{
    public int UserId { get; set; }
    public int ChatId { get; set; }

    // Relationships
    public User User { get; set; }
    public Chat Chat { get; set; }
}