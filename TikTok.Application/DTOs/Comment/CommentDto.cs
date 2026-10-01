namespace TikTok.Application.DTOs.Comment;

public class CommentDto
{
    public Guid Id { get; set; }
    public string Content { get; set; } = string.Empty;

    public Guid UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Avatar { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}