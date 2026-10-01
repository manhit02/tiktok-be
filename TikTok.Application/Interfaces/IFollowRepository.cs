using TikTok.Domain.Entities;

namespace TikTok.Application.Interfaces;

public interface IFollowRepository
{
    Task<Follow?> GetAsync(Guid followerId, Guid followingId);
    Task AddAsync(Follow follow);
    Task DeleteAsync(Follow follow);

    Task<int> CountFollowersAsync(Guid userId);
    Task<int> CountFollowingAsync(Guid userId);

    Task<List<User>> GetFollowersAsync(Guid userId, int page, int limit);
    Task<List<User>> GetFollowingAsync(Guid userId, int page, int limit);
}