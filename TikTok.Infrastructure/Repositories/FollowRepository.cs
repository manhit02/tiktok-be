using Microsoft.EntityFrameworkCore;
using TikTok.Application.Interfaces;
using TikTok.Domain.Entities;
using TikTok.Infrastructure.Data;

namespace TikTok.Infrastructure.Repositories;

public class FollowRepository : IFollowRepository
{
    private readonly AppDbContext _context;

    public FollowRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Follow?> GetAsync(Guid followerId, Guid followingId)
    {
        return await _context.Follows
            .FirstOrDefaultAsync(x =>
                x.FollowerId == followerId &&
                x.FollowingId == followingId);
    }

    public async Task AddAsync(Follow follow)
    {
        await _context.Follows.AddAsync(follow);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Follow follow)
    {
        _context.Follows.Remove(follow);
        await _context.SaveChangesAsync();
    }

    public async Task<int> CountFollowersAsync(Guid userId)
    {
        return await _context.Follows
            .CountAsync(x => x.FollowingId == userId);
    }

    public async Task<int> CountFollowingAsync(Guid userId)
    {
        return await _context.Follows
            .CountAsync(x => x.FollowerId == userId);
    }

    public async Task<List<User>> GetFollowersAsync(
        Guid userId,
        int page,
        int limit)
    {
        return await _context.Follows
            .Where(x => x.FollowingId == userId)
            .Include(x => x.Follower)
            .Select(x => x.Follower)
            .Skip((page - 1) * limit)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<List<User>> GetFollowingAsync(
        Guid userId,
        int page,
        int limit)
    {
        return await _context.Follows
            .Where(x => x.FollowerId == userId)
            .Include(x => x.Following)
            .Select(x => x.Following)
            .Skip((page - 1) * limit)
            .Take(limit)
            .ToListAsync();
    }
}