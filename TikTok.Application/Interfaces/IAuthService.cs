using TikTok.Application.DTOs.Auth;
using TikTok.Application.DTOs;

namespace TikTok.Application.Interfaces;

public interface IAuthService
{
    Task RegisterAsync(RegisterRequest request);

    Task<LoginResponse> LoginAsync(LoginRequest request);
    Task<string> RefreshAsync(string refreshToken);
    Task LogoutAsync(string refreshToken);
}

public class AuthResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public UserDto User { get; set; } = null!;
}