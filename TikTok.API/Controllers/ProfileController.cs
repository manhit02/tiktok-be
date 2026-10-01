using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TikTok.Application.DTOs;
using TikTok.Application.Interfaces;
using TikTok.Domain.Entities;

namespace TikTok.API.Controllers;

[ApiController]
[Route("api/profile")]
public class ProfileController : ControllerBase
{
    private readonly IProfileService _profileService;
    private readonly IVideoService _videoService;

    public ProfileController(IProfileService profileService, IVideoService videoService)
    {
        _profileService = profileService;
        _videoService = videoService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var profile = await _profileService.GetProfileByIdAsync(id);
        if (profile == null)
        {
            return NotFound(new
            {
                success = false,
                message = "Profile không tồn tại"
            });
        }
        return Ok(new
        {
            success = true,
            data = profile
        });
    }
    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateProfileRequest request)
    {
        try
        {
            var currentUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var profile = await _profileService.UpdateProfileAsync(currentUserId, request);
            return Ok(new
            {
                success = true,
                message = "Cập nhật profile thành công",
                data = profile
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
    [HttpGet("videos/{userId}")]
    public async Task<IActionResult> GetVideosByUserId(
       Guid userId,
       [FromQuery] int page = 1,
       [FromQuery] int limit = 10)
    {
        var videos = await _videoService.GetVideosByUserIdAsync(userId, page, limit);
        return Ok(new
        {
            success = true,
            data = videos
        });
    }
}
