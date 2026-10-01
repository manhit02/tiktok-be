using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TikTok.Application.DTOs.Comment;
using TikTok.Application.Interfaces;

namespace TikTok.API.Controllers;

[ApiController]
[Route("api/videos/{videoId}/comments")]
public class CommentController : ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetByVideoId(Guid videoId)
    {
        var comments = await _commentService
            .GetByVideoIdAsync(videoId);

        return Ok(new
        {
            success = true,
            data = comments
        });
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(
        Guid videoId,
        CreateCommentRequest request)
    {
        var userId = Guid.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!
        );

        var comment = await _commentService
            .CreateAsync(videoId, userId, request);

        return Ok(new
        {
            success = true,
            message = "Bình luận thành công",
            data = comment
        });
    }

    [Authorize]
    [HttpDelete("{commentId}")]
    public async Task<IActionResult> Delete(
        Guid videoId,
        Guid commentId)
    {
        try
        {
            var userId = Guid.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );

            await _commentService.DeleteAsync(
                commentId,
                userId
            );

            return Ok(new
            {
                success = true,
                message = "Xóa bình luận thành công"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                success = false,
                message = ex.Message
            });
        }
    }
    [Authorize]
    [HttpPut("{commentId}")]
    public async Task<IActionResult> Update(
        Guid commentId, UpdateCommentRequest request)
    {
        var userId = Guid.Parse(
  User.FindFirstValue(ClaimTypes.NameIdentifier)!
  );
        try
        {
            await _commentService.UpdateAsync(
                commentId,
                userId,
                request
            );
            return Ok(new
            {
                success = true,
                message = "Sửa bình luận thành công"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                success = false,
                message = ex.Message
            });
        }
    }
}