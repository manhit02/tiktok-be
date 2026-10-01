using TikTok.Application.DTOs.Comment;

namespace TikTok.Application.Interfaces;

public interface ICommentService
{
    Task<List<CommentDto>> GetByVideoIdAsync(Guid videoId);

    Task<CommentDto> CreateAsync(
        Guid videoId,
        Guid userId,
        CreateCommentRequest request
    );

    Task DeleteAsync(
        Guid commentId,
        Guid userId
    );
    Task UpdateAsync(
     Guid commentId,
     Guid userId,
     UpdateCommentRequest request);
}