using TikTok.Application.DTOs.Comment;
using TikTok.Application.Interfaces;
using TikTok.Domain.Entities;

namespace TikTok.Application.Services;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;

    public CommentService(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    public async Task<List<CommentDto>> GetByVideoIdAsync(Guid videoId)
    {
        var comments = await _commentRepository
            .GetByVideoIdAsync(videoId);

        return comments.Select(x => new CommentDto
        {
            Id = x.Id,
            Content = x.Content,
            UserId = x.UserId,
            Username = x.User.Username,
            Avatar = x.User.Avatar,
            CreatedAt = x.CreatedAt,

        }).ToList();
    }

    public async Task<CommentDto> CreateAsync(
        Guid videoId,
        Guid userId,
        CreateCommentRequest request)
    {
        var comment = new Comment
        {
            Id = Guid.NewGuid(),
            VideoId = videoId,
            UserId = userId,
            Content = request.Content
        };

        await _commentRepository.AddAsync(comment);

        var createdComment = await _commentRepository
            .GetByIdAsync(comment.Id);

        return new CommentDto
        {
            Id = createdComment!.Id,
            Content = createdComment.Content,
            UserId = createdComment.UserId,
            Username = createdComment.User.Username,
            Avatar = createdComment.User.Avatar,
            CreatedAt = createdComment.CreatedAt
        };
    }

    public async Task DeleteAsync(
        Guid commentId,
        Guid userId)
    {
        var comment = await _commentRepository
            .GetByIdAsync(commentId);

        if (comment == null)
            throw new Exception("Comment không tồn tại");

        if (comment.UserId != userId)
            throw new Exception(
                "Bạn không có quyền xóa comment này"
            );

        await _commentRepository.DeleteAsync(comment);
    }
    public async Task UpdateAsync(
     Guid commentId,
     Guid userId,
     UpdateCommentRequest request)
    {
        var comment = await _commentRepository
            .GetByIdAsync(commentId);

        if (comment == null)
            throw new Exception("Comment không tồn tại");

        if (comment.UserId != userId)
            throw new Exception("Bạn không có quyền sửa comment này");

        comment.Content = request.Content;

        await _commentRepository.UpdateAsync(comment);
    }
}