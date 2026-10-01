using TikTok.Domain.Entities;

namespace TikTok.Application.Interfaces;

public interface IVideoLikeRepository
{
    Task<VideoLike?> GetAsync(Guid videoId, Guid userId);
    Task AddAsync(VideoLike videoLike);
    Task DeleteAsync(VideoLike videoLike);
    Task<int> CountAsync(Guid videoId);
}