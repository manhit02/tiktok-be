namespace TikTok.Application.DTOs.Video;

public class VideoDto
{
    public Guid Id { get; set; }
    public string VideoUrl { get; set; } = string.Empty;
    public string Caption { get; set; } = string.Empty;
    public int Views { get; set; }

    public Guid UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Avatar { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
    public int LikeCount { get; set; }
    public bool IsLiked { get; set; }
    public int CommentCount { get; set; }
}