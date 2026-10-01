using TikTok.Domain.Entities;
using TikTok.Application.DTOs;

namespace TikTok.Application.Interfaces;

public interface IProfileRepository
{
    Task<(Profile?, int totalLikes)> GetProfileByIdAsync(Guid id);
    Task<Profile> UpdateProfileAsync(Guid id, UpdateProfileRequest request);
}