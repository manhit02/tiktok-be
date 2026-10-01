using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TikTok.Application.Interfaces;

namespace TikTok.API.Controllers;

[ApiController]
[Route("api/users")]
public class FollowController : ControllerBase
{
    private readonly IFollowService _followService;

    public FollowController(IFollowService followService)
    {
        _followService = followService;
    }

    [Authorize]
    [HttpPost("{userId}/follow")]
    public async Task<IActionResult> ToggleFollow(Guid userId)
    {
        var currentUserId = Guid.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!
        );

        var following = await _followService
            .ToggleFollowAsync(currentUserId, userId);

        return Ok(new
        {
            success = true,
            following
        });
    }

    [Authorize]
    [HttpGet("{userId}/check-following")]
    public async Task<IActionResult> CheckFollowing(Guid userId)
    {
        var currentUserId = Guid.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!
        );

        var following = await _followService
            .IsFollowingAsync(currentUserId, userId);

        return Ok(new
        {
            success = true,
            following
        });
    }
    [HttpGet("{userId}/followers")]
    public async Task<IActionResult> GetFollowers(
    Guid userId,
    [FromQuery] int page = 1,
    [FromQuery] int limit = 10)
    {
        var users = await _followService
            .GetFollowersAsync(userId, page, limit);

        return Ok(new
        {
            success = true,
            data = users
        });
    }

    [HttpGet("{userId}/following")]
    public async Task<IActionResult> GetFollowing(
        Guid userId,
        [FromQuery] int page = 1,
        [FromQuery] int limit = 10)
    {
        var users = await _followService
            .GetFollowingAsync(userId, page, limit);

        return Ok(new
        {
            success = true,
            data = users
        });
    }
}