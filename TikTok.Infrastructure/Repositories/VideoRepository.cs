using Microsoft.EntityFrameworkCore;
using TikTok.Application.Interfaces;
using TikTok.Domain.Entities;
using TikTok.Infrastructure.Data;
using TikTok.Application.DTOs.Video;
namespace TikTok.Infrastructure.Repositories;

public class VideoRepository : IVideoRepository
{
    private readonly AppDbContext _context;

    public VideoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<VideoDto>> GetAllAsync(int page, int limit)
    {
        return await _context.Videos
            .Include(x => x.User)
            .OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * limit)
            .Take(limit)
            .Select(x => new VideoDto
            {
                Id = x.Id,
                VideoUrl = x.VideoUrl,
                Caption = x.Caption,
                Views = x.Views,

                UserId = x.UserId,
                Username = x.User.Username,
                Avatar = x.User.Avatar,

                CreatedAt = x.CreatedAt,

                LikeCount = x.VideoLikes.Count(),
                CommentCount = x.Comments.Count(),

            })
            .ToListAsync();
    }

    public async Task<Video?> GetByIdAsync(Guid id)
    {
        return await _context.Videos
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddAsync(Video video)
    {
        await _context.Videos.AddAsync(video);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Video video)
    {
        _context.Videos.Update(video);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Video video)
    {
        _context.Videos.Remove(video);
        await _context.SaveChangesAsync();
    }
    public async Task<int> IncrementViewsAsync(Guid id)
    {
        var video = await _context.Videos
            .FirstOrDefaultAsync(x => x.Id == id);

        if (video == null)
            throw new Exception("Video không tồn tại");

        video.Views++;

        await _context.SaveChangesAsync();
        return video.Views;
    }
    public async Task<List<VideoDto>> GetFeedAsync(Guid? currentUserId, int page, int limit)
    {
        return await _context.Videos

            .OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * limit)
            .Take(limit)
            .Select(x => new VideoDto
            {
                Id = x.Id,
                VideoUrl = x.VideoUrl,
                Caption = x.Caption,
                Views = x.Views,

                UserId = x.UserId,
                Username = x.User.Username,
                Avatar = x.User.Avatar,

                CreatedAt = x.CreatedAt,

                LikeCount = x.VideoLikes.Count(),
                CommentCount = x.Comments.Count(),
                IsLiked = currentUserId.HasValue &&
                  x.VideoLikes.Any(l => l.UserId == currentUserId.Value)
            })
            .ToListAsync();
    }
    public async Task<List<VideoDto>> GetVideosByUserIdAsync(Guid userId, int page, int limit)
    {
        return await _context.Videos
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * limit)
            .Take(limit)
            .Select(x => new VideoDto
            {
                Id = x.Id,
                VideoUrl = x.VideoUrl,
                Caption = x.Caption,
                Views = x.Views,

                UserId = x.UserId,
                Username = x.User.Username,
                Avatar = x.User.Avatar,

                CreatedAt = x.CreatedAt,

                LikeCount = x.VideoLikes.Count(),
                CommentCount = x.Comments.Count(),

            })
            .ToListAsync();
    }
}