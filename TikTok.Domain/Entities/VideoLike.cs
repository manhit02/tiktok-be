namespace TikTok.Domain.Entities;

public class VideoLike
{
    public Guid Id { get; set; }

    public Guid VideoId { get; set; }

    public Guid UserId { get; set; }

    public Video Video { get; set; } = null!;
    public User User { get; set; } = null!;
}