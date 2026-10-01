namespace TikTok.Application.Interfaces;

public interface IVideoLikeService
{
    Task<(bool Liked, int LikeCount)> ToggleLikeAsync(Guid videoId, Guid userId);
}