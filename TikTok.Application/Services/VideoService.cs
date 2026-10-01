using TikTok.Application.DTOs.Video;
using TikTok.Application.Interfaces;
using TikTok.Domain.Entities;

namespace TikTok.Application.Services;

public class VideoService : IVideoService
{
    private readonly IVideoRepository _videoRepository;
    private readonly IVideoLikeRepository _videoLikeRepository;
    public VideoService(IVideoRepository videoRepository, IVideoLikeRepository videoLikeRepository)
    {
        _videoRepository = videoRepository;
        _videoLikeRepository = videoLikeRepository;
    }

    public async Task<List<VideoDto>> GetAllAsync(int page, int limit)
    {
        return await _videoRepository.GetAllAsync(page, limit);

    }

    public async Task<VideoDto?> GetByIdAsync(
        Guid id,
        Guid? userId)
    {
        var video = await _videoRepository.GetByIdAsync(id);

        if (video == null)
            return null;

        return new VideoDto
        {
            Id = video.Id,
            VideoUrl = video.VideoUrl,
            Caption = video.Caption,
            Views = video.Views,
            UserId = video.UserId,
            Username = video.User.Username,
            Avatar = video.User.Avatar,
            CreatedAt = video.CreatedAt,
            LikeCount = await _videoLikeRepository.CountAsync(video.Id),
            IsLiked = userId.HasValue &&
          await _videoLikeRepository.GetAsync(
              video.Id,
              userId.Value
          ) != null
        };
    }

    public async Task<VideoDto> CreateAsync(
        CreateVideoRequest request,
        Guid userId)
    {
        var video = new Video
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            VideoUrl = request.VideoUrl,
            Caption = request.Caption
        };

        await _videoRepository.AddAsync(video);

        return (await GetByIdAsync(video.Id, userId))!;
    }

    public async Task UpdateAsync(
        Guid id,
        UpdateVideoRequest request,
        Guid userId)
    {
        var video = await _videoRepository.GetByIdAsync(id);

        if (video == null)
            throw new Exception("Video không tồn tại");

        if (video.UserId != userId)
            throw new Exception("Bạn không có quyền sửa video này");

        video.Caption = request.Caption;

        await _videoRepository.UpdateAsync(video);
    }

    public async Task DeleteAsync(Guid id, Guid userId)
    {
        var video = await _videoRepository.GetByIdAsync(id);

        if (video == null)
            throw new Exception("Video không tồn tại");

        if (video.UserId != userId)
            throw new Exception("Bạn không có quyền xóa video này");

        await _videoRepository.DeleteAsync(video);
    }

    public async Task<int> IncrementViewsAsync(Guid id)
    {
        return await _videoRepository.IncrementViewsAsync(id);
    }
    public async Task<List<VideoDto>> GetFeedAsync(Guid? currentUserId, int page, int limit)
    {
        return await _videoRepository.GetFeedAsync(currentUserId, page, limit);
    }
    public async Task<List<VideoDto>> GetVideosByUserIdAsync(Guid userId, int page, int limit)
    {
        return await _videoRepository.GetVideosByUserIdAsync(userId, page, limit);
    }
}