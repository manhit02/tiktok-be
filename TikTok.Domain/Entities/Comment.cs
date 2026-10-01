namespace TikTok.Domain.Entities;

public class Comment
{
    public Guid Id { get; set; }

    public Guid VideoId { get; set; }

    public Guid UserId { get; set; }

    public string Content { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Video Video { get; set; } = null!;

    public User User { get; set; } = null!;

}