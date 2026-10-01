using TikTok.Application.DTOs;
using TikTok.Application.Interfaces;
using TikTok.Domain.Entities;

namespace TikTok.Application.Services;

public class ProfileService : IProfileService
{
    private readonly IProfileRepository _profileRepository;

    public ProfileService(IProfileRepository profileRepository)
    {
        _profileRepository = profileRepository;
    }

    public async Task<ProfileDto?> GetProfileByIdAsync(Guid id)
    {
        var (profile, totalLikes) = await _profileRepository.GetProfileByIdAsync(id);
        if (profile == null)
            return null;
        return new ProfileDto
        {
            Id = profile.Id,
            UserId = profile.UserId,
            DisplayName = profile.DisplayName,
            Bio = profile.Bio,
            Avatar = profile.Avatar,
            CoverImage = profile.CoverImage,
            TotalLikes = totalLikes,
            Followers = profile.User.Followers.Select(x => new FollowUserDto
            {
                Id = x.FollowerId,
                Username = x.Follower.Username,
                Avatar = x.Follower.Profile?.Avatar ?? ""
            }).ToList(),
            Following = profile.User.Following.Select(x => new FollowUserDto
            {
                Id = x.FollowingId,
                Username = x.Following.Username,
                Avatar = x.Following.Profile?.Avatar ?? ""
            }).ToList()
        };
    }

    public async Task<Profile> UpdateProfileAsync(Guid id, UpdateProfileRequest request)
    {
        var (profile, _) = await _profileRepository.GetProfileByIdAsync(id);
        if (profile == null)
            throw new Exception("Profile không tồn tại");
        profile.DisplayName = request.DisplayName;
        profile.Bio = request.Bio;
        profile.Avatar = request.Avatar;
        profile.CoverImage = request.CoverImage;
        await _profileRepository.UpdateProfileAsync(id, request);
        return profile;
    }
}