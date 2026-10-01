using TikTok.Domain.Entities;

namespace TikTok.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);

    Task<User?> GetByUsernameAsync(string username);

    Task AddAsync(User user);
    Task<User?> GetByIdAsync(Guid id);
}