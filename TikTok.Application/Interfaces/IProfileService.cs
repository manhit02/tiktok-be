namespace TikTok.Application.Interfaces;

using TikTok.Application.DTOs;
using TikTok.Domain.Entities;
public interface IProfileService
{

    Task<ProfileDto?> GetProfileByIdAsync(Guid id);

    Task<Profile> UpdateProfileAsync(Guid id, UpdateProfileRequest request);
}