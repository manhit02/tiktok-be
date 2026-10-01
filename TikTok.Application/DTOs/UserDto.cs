namespace TikTok.Application.DTOs;

public class UserDto
{
    public Guid Id { get; set; }

    public string Username { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Avatar { get; set; } = string.Empty;
}