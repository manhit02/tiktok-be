using TikTok.Domain.Entities;

namespace TikTok.Application.Interfaces;

public interface ICommentRepository
{
    Task<List<Comment>> GetByVideoIdAsync(Guid videoId);
    Task<Comment?> GetByIdAsync(Guid id);
    Task AddAsync(Comment comment);
    Task DeleteAsync(Comment comment);
    Task UpdateAsync(Comment comment);
}