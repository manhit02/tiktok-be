using TikTok.Domain.Entities;

namespace TikTok.Application.Interfaces;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetByTokenAsync(string token);
    Task AddAsync(RefreshToken refreshToken);
    Task DeleteAsync(string token);
}