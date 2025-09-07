using Microsoft.EntityFrameworkCore;
using LlLambBackend.Models;

namespace LlLambBackend.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<UserSession> UserSessions { get; set; }
    public DbSet<Model> Models { get; set; }
    public DbSet<ChatSession> ChatSessions { get; set; }
    public DbSet<Message> Messages { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // User configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId);
            entity.HasIndex(e => e.Username).IsUnique();
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.UserId).HasDefaultValueSql("NEWID()");
        });
        
        // UserSession configuration
        modelBuilder.Entity<UserSession>(entity =>
        {
            entity.HasKey(e => e.SessionId);
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.RefreshToken);
            entity.Property(e => e.SessionId).HasDefaultValueSql("NEWID()");
            
            entity.HasOne(e => e.User)
                  .WithMany(u => u.Sessions)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
        
        // Model configuration
        modelBuilder.Entity<Model>(entity =>
        {
            entity.HasKey(e => e.ModelId);
            entity.Property(e => e.ModelId).HasDefaultValueSql("NEWID()");
        });
        
        // ChatSession configuration
        modelBuilder.Entity<ChatSession>(entity =>
        {
            entity.HasKey(e => e.SessionId);
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.ModelId);
            entity.Property(e => e.SessionId).HasDefaultValueSql("NEWID()");
            
            entity.HasOne(e => e.User)
                  .WithMany(u => u.ChatSessions)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
                  
            entity.HasOne(e => e.Model)
                  .WithMany(m => m.ChatSessions)
                  .HasForeignKey(e => e.ModelId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
        
        // Message configuration
        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.MessageId);
            entity.HasIndex(e => e.SessionId);
            entity.HasIndex(e => e.CreatedAt);
            entity.Property(e => e.MessageId).HasDefaultValueSql("NEWID()");
            entity.Property(e => e.Role).HasConversion<string>();
            
            entity.HasOne(e => e.ChatSession)
                  .WithMany(cs => cs.Messages)
                  .HasForeignKey(e => e.SessionId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}