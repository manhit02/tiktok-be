using Microsoft.EntityFrameworkCore;
using TikTok.Domain.Entities;
namespace TikTok.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<Video> Videos => Set<Video>();
    public DbSet<VideoLike> VideoLikes => Set<VideoLike>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<Follow> Follows => Set<Follow>();
    public DbSet<Profile> Profile => Set<Profile>();
    public DbSet<Search> Search => Set<Search>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RefreshToken>()
            .HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId);

        modelBuilder.Entity<Video>()
            .HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId);

        modelBuilder.Entity<VideoLike>()
            .HasIndex(x => new { x.VideoId, x.UserId })
            .IsUnique();

        modelBuilder.Entity<VideoLike>()
            .HasOne(x => x.Video)
            .WithMany(x => x.VideoLikes)
            .HasForeignKey(x => x.VideoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Comment>()
            .HasOne(x => x.Video)
            .WithMany(x => x.Comments)
            .HasForeignKey(x => x.VideoId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Comment>()
            .HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Follow>()
            .HasOne(x => x.Follower)
            .WithMany(x => x.Following)
            .HasForeignKey(x => x.FollowerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Follow>()
            .HasOne(x => x.Following)
            .WithMany(x => x.Followers)
            .HasForeignKey(x => x.FollowingId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Follow>()
            .HasIndex(x => new { x.FollowerId, x.FollowingId })
            .IsUnique();
        modelBuilder.Entity<Profile>()
            .HasOne(x => x.User)
           .WithOne(x => x.Profile)
            .HasForeignKey<Profile>(x => x.UserId);

        modelBuilder.Entity<Search>()
            .HasOne(x => x.User)
            .WithMany(x => x.Searchs)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Search>()
            .HasIndex(x => new { x.UserId, x.CreatedAt });

    }
}