using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TikTok.Application.DTOs.Video;
using TikTok.Application.Interfaces;
using TikTok.Domain.Entities;

namespace TikTok.API.Controllers;

[ApiController]
[Route("api/videos")]
public class VideoController : ControllerBase
{
    private readonly IVideoService _videoService;
    private readonly IVideoLikeService _videoLikeService;

    public VideoController(IVideoService videoService, IVideoLikeService videoLikeService)
    {
        _videoService = videoService;
        _videoLikeService = videoLikeService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1,
        [FromQuery] int limit = 10)
    {

        var videos = await _videoService.GetAllAsync(page, limit);

        return Ok(new
        {
            success = true,
            data = videos
        });
    }
    [HttpGet("feed")]
    public async Task<IActionResult> GetFeed(
        [FromQuery] int page = 1,
        [FromQuery] int limit = 10)
    {

        var claim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        Guid? currentUserId = null;
        if (Guid.TryParse(claim, out var userId))
            currentUserId = userId;

        var videos = await _videoService.GetFeedAsync(
            currentUserId,
            page,
            limit
        );
        return Ok(new
        {
            success = true,
            data = videos
        });
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        Guid? userId = null;
        if (User.Identity?.IsAuthenticated == true)
        {
            userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        }

        var video = await _videoService.GetByIdAsync(id, userId);

        if (video == null)
            return NotFound(new
            {
                success = false,
                message = "Video không tồn tại"
            });

        return Ok(new
        {
            success = true,
            data = video
        });
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(CreateVideoRequest request)
    {
        var userId = Guid.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!
        );

        var video = await _videoService.CreateAsync(
            request,
            userId
        );

        return Ok(new
        {
            success = true,
            message = "Đăng video thành công",
            data = video
        });
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        Guid id,
        UpdateVideoRequest request)
    {
        try
        {
            var userId = Guid.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );

            await _videoService.UpdateAsync(
                id,
                request,
                userId
            );

            return Ok(new
            {
                success = true,
                message = "Cập nhật video thành công"
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
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var userId = Guid.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );

            await _videoService.DeleteAsync(id, userId);

            return Ok(new
            {
                success = true,
                message = "Xóa video thành công"
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
    [HttpPost("{id}/view")]
    public async Task<IActionResult> View(Guid id)
    {
        var views = await _videoService.IncrementViewsAsync(id);

        return Ok(new
        {
            success = true,
            views

        });
    }
    [Authorize]
    [HttpPost("{id}/like")]
    public async Task<IActionResult> Like(Guid id)
    {
        var userId = Guid.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!
        );


        var result = await _videoLikeService
            .ToggleLikeAsync(id, userId);

        return Ok(new
        {
            success = true,
            liked = result.Liked,
            likeCount = result.LikeCount
        });
    }
    // [HttpPost("search")]


}