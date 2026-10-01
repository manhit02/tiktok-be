using TikTok.Domain.Entities;
using TikTok.Application.Interfaces;
namespace TikTok.Application.Services;

public class FollowService : IFollowService
{
    private readonly IFollowRepository _followRepository;

    public FollowService(IFollowRepository followRepository)
    {
        _followRepository = followRepository;
    }

    public async Task<bool> ToggleFollowAsync(
        Guid followerId,
        Guid followingId)
    {
        if (followerId == followingId)
            throw new Exception("Không thể follow chính mình");

        var follow = await _followRepository.GetAsync(
            followerId,
            followingId);

        if (follow != null)
        {
            await _followRepository.DeleteAsync(follow);
            return false;
        }

        await _followRepository.AddAsync(new Follow
        {
            Id = Guid.NewGuid(),
            FollowerId = followerId,
            FollowingId = followingId
        });

        return true;
    }

    public async Task<bool> IsFollowingAsync(
        Guid followerId,
        Guid followingId)
    {
        var follow = await _followRepository.GetAsync(
            followerId,
            followingId);

        return follow != null;
    }
    public async Task<List<User>> GetFollowersAsync(
    Guid userId,
    int page,
    int limit)
    {
        return await _followRepository
            .GetFollowersAsync(userId, page, limit);
    }

    public async Task<List<User>> GetFollowingAsync(
        Guid userId,
        int page,
        int limit)
    {
        return await _followRepository
            .GetFollowingAsync(userId, page, limit);
    }
}