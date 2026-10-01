using Microsoft.EntityFrameworkCore;
using TikTok.Application.Interfaces;
using TikTok.Domain.Entities;
using TikTok.Infrastructure.Data;

namespace TikTok.Infrastructure.Repositories;

public class VideoLikeRepository : IVideoLikeRepository
{
    private readonly AppDbContext _context;

    public VideoLikeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<VideoLike?> GetAsync(Guid videoId, Guid userId)
    {
        return await _context.VideoLikes
            .FirstOrDefaultAsync(x =>
                x.VideoId == videoId &&
                x.UserId == userId);
    }

    public async Task AddAsync(VideoLike videoLike)
    {
        await _context.VideoLikes.AddAsync(videoLike);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(VideoLike videoLike)
    {
        _context.VideoLikes.Remove(videoLike);
        await _context.SaveChangesAsync();
    }
    public async Task<int> CountAsync(Guid videoId)
    {
        return await _context.VideoLikes
            .CountAsync(x => x.VideoId == videoId);
    }
}