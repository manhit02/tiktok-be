namespace TikTok.Domain.Entities;

public class Video
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string VideoUrl { get; set; } = string.Empty;

    public string Caption { get; set; } = string.Empty;

    public int Views { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User User { get; set; } = null!;
    public ICollection<VideoLike> VideoLikes { get; set; } = [];
    public ICollection<Comment> Comments { get; set; } = [];
}