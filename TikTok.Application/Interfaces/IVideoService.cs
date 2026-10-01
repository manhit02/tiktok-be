using TikTok.Application.DTOs.Video;

namespace TikTok.Application.Interfaces;

public interface IVideoService
{
    Task<List<VideoDto>> GetAllAsync(int page, int limit);
    Task<VideoDto?> GetByIdAsync(Guid id, Guid? userId);
    Task<VideoDto> CreateAsync(CreateVideoRequest request, Guid userId);
    Task UpdateAsync(Guid id, UpdateVideoRequest request, Guid userId);
    Task DeleteAsync(Guid id, Guid userId);
    Task<int> IncrementViewsAsync(Guid id);
    Task<List<VideoDto>> GetFeedAsync(Guid? currentUserId, int page, int limit);
    Task<List<VideoDto>> GetVideosByUserIdAsync(Guid userId, int page, int limit);

}