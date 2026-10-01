namespace TikTok.Application.Interfaces;

using TikTok.Domain.Entities;
public interface IFollowService
{
    Task<bool> ToggleFollowAsync(Guid followerId, Guid followingId);
    Task<bool> IsFollowingAsync(Guid followerId, Guid followingId);
    Task<List<User>> GetFollowersAsync(
    Guid userId,
    int page,
    int limit);

    Task<List<User>> GetFollowingAsync(
        Guid userId,
        int page,
        int limit);
}