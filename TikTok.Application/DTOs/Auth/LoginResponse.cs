using TikTok.Application.DTOs;
namespace TikTok.Application.DTOs.Auth;

public class LoginResponse
{
    public string RefreshToken { get; set; } = string.Empty;
    public string AccessToken { get; set; } = string.Empty;
    public UserDto User { get; set; } = null!;
}