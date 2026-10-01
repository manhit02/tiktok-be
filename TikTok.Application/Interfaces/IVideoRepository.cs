using TikTok.Domain.Entities;
using TikTok.Application.DTOs.Video;

namespace TikTok.Application.Interfaces;

public interface IVideoRepository
{
    Task<List<VideoDto>> GetAllAsync(int page, int limit);
    Task<Video?> GetByIdAsync(Guid id);
    Task AddAsync(Video video);
    Task UpdateAsync(Video video);
    Task DeleteAsync(Video video);
    Task<int> IncrementViewsAsync(Guid id);
    Task<List<VideoDto>> GetFeedAsync(Guid? currentUserId, int page, int limit);
    Task<List<VideoDto>> GetVideosByUserIdAsync(Guid userId, int page, int limit);

}