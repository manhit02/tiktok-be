using Microsoft.AspNetCore.Mvc;
using TikTok.Application.DTOs.Auth;
using TikTok.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using TikTok.Application.DTOs;
namespace TikTok.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IUserRepository _userRepository;
    public AuthController(IAuthService authService, IUserRepository userRepository)
    {
        _authService = authService;
        _userRepository = userRepository;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        try
        {

            await _authService.RegisterAsync(request);

            return Ok(new
            {
                success = true,
                message = "Đăng ký thành công"
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
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        try
        {
            var user = await _authService.LoginAsync(request);

            return Ok(new
            {
                success = true,
                message = "Đăng nhập thành công",
                data = user
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
    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> Me()
    {
        var userId = Guid.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!
        );

        var user = await _userRepository.GetByIdAsync(userId);

        if (user == null)
            return NotFound();

        return Ok(new
        {
            success = true,
            data = new UserDto
            {
                Id = userId,
                Username = user.Username,
                Email = user.Email,
                Avatar = user.Avatar
            }
        });
    }
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(RefreshRequest request)
    {
        try
        {
            var accessToken = await _authService
                .RefreshAsync(request.RefreshToken);

            return Ok(new
            {
                success = true,
                data = new
                {
                    accessToken
                }
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
    [HttpPost("logout")]
    public async Task<IActionResult> Logout(RefreshRequest request)
    {
        try
        {
            await _authService.LogoutAsync(request.RefreshToken);

            return Ok(new
            {
                success = true,
                message = "Đăng xuất thành công"
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