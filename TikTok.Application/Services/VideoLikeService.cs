using TikTok.Application.Interfaces;
using TikTok.Domain.Entities;

namespace TikTok.Application.Services;

public class VideoLikeService : IVideoLikeService
{
    private readonly IVideoLikeRepository _videoLikeRepository;

    public VideoLikeService(IVideoLikeRepository videoLikeRepository)
    {
        _videoLikeRepository = videoLikeRepository;
    }

    public async Task<(bool Liked, int LikeCount)> ToggleLikeAsync(
    Guid videoId,
    Guid userId)
    {
        var existingLike = await _videoLikeRepository
            .GetAsync(videoId, userId);

        if (existingLike != null)
        {
            await _videoLikeRepository.DeleteAsync(existingLike);

            var count = await _videoLikeRepository.CountAsync(videoId);

            return (false, count);
        }

        var videoLike = new VideoLike
        {
            Id = Guid.NewGuid(),
            VideoId = videoId,
            UserId = userId
        };

        await _videoLikeRepository.AddAsync(videoLike);

        var countAfterLike = await _videoLikeRepository.CountAsync(videoId);

        return (true, countAfterLike);
    }
}