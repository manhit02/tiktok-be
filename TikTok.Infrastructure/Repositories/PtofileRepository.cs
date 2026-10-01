using Microsoft.EntityFrameworkCore;
using TikTok.Application.Interfaces;
using TikTok.Domain.Entities;
using TikTok.Infrastructure.Data;

namespace TikTok.Infrastructure.Repositories;

public class ProfileRepository : IProfileRepository
{
    private readonly AppDbContext _context;

    public ProfileRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<(Profile?, int totalLikes)> GetProfileByIdAsync(Guid id)
    {
        var totalLikes = await _context.Videos
            .Where(x => x.UserId == id)
            .SelectMany(x => x.VideoLikes)
            .CountAsync();
        var profile = await _context
                .Profile.Include(x => x.User)
                    .ThenInclude(x => x.Followers)
                        .ThenInclude(x => x.Follower)
                            .ThenInclude(x => x.Profile)
                .Include(x => x.User)
                    .ThenInclude(x => x.Following)
                        .ThenInclude(x => x.Following)
                            .ThenInclude(x => x.Profile)

                .FirstOrDefaultAsync(x => x.UserId == id);
        return (profile, totalLikes);
    }

    public async Task<Profile> UpdateProfileAsync(
    Guid id,
    UpdateProfileRequest request)
    {
        var profile = await _context.Profile
            .FirstOrDefaultAsync(x => x.UserId == id);

        if (profile == null)
            throw new Exception("Profile không tồn tại");

        profile.DisplayName = request.DisplayName;
        profile.Bio = request.Bio;
        profile.Avatar = request.Avatar;
        profile.CoverImage = request.CoverImage;

        await _context.SaveChangesAsync();

        return profile;
    }
}