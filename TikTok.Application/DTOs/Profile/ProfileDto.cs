using TikTok.Application.DTOs;
using TikTok.Domain.Entities;

public class ProfileDto
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string DisplayName { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public string Avatar { get; set; } = string.Empty;
    public string CoverImage { get; set; } = string.Empty;
    public int TotalLikes { get; set; }
    public List<FollowUserDto> Followers { get; set; } = [];
    public List<FollowUserDto> Following { get; set; } = [];
}