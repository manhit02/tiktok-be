using BCrypt.Net;
using TikTok.Application.DTOs.Auth;
using TikTok.Application.Interfaces;
using TikTok.Domain.Entities;
using TikTok.Application.DTOs;

using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
namespace TikTok.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    public AuthService(IUserRepository userRepository, IConfiguration configuration, IRefreshTokenRepository refreshTokenRepository)
    {
        _userRepository = userRepository;
        _configuration = configuration;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task RegisterAsync(RegisterRequest request)
    {
        var existingEmail = await _userRepository
            .GetByEmailAsync(request.Email);

        if (existingEmail != null)
            throw new Exception("Email đã tồn tại");

        var existingUsername = await _userRepository
            .GetByUsernameAsync(request.Username);

        if (existingUsername != null)
            throw new Exception("Username đã tồn tại");

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = request.Username,
            Email = request.Email,
            Password = hashedPassword
        };

        await _userRepository.AddAsync(user);
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);

        if (user == null)
            throw new Exception("Email hoặc password không đúng");

        var isMatch = BCrypt.Net.BCrypt.Verify(
            request.Password,
            user.Password
        );

        if (!isMatch)
            throw new Exception("Email hoặc password không đúng");
        var refreshToken = Guid.NewGuid().ToString();
        var refreshTokenEntity = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };

        await _refreshTokenRepository.AddAsync(refreshTokenEntity);
        var accessSecret = _configuration["Jwt:AccessSecret"]!;

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(accessSecret)
        );

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256
        );

        var claims = new[]
        {
    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
    new Claim(ClaimTypes.Email, user.Email),
    new Claim(ClaimTypes.Name, user.Username)
};

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(
                int.Parse(_configuration["Jwt:ExpiresMinutes"]!)
            ),
            signingCredentials: credentials
        );

        var accessToken = new JwtSecurityTokenHandler()
            .WriteToken(token);
        return new LoginResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            User = new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Avatar = user.Avatar
            }
        };
    }
    public async Task<string> RefreshAsync(string refreshToken)
    {
        var token = await _refreshTokenRepository
            .GetByTokenAsync(refreshToken);

        if (token == null)
            throw new Exception("Refresh token không hợp lệ");

        if (token.ExpiresAt < DateTime.UtcNow)
            throw new Exception("Refresh token đã hết hạn");

        var user = await _userRepository.GetByIdAsync(token.UserId);

        if (user == null)
            throw new Exception("User không tồn tại");

        var accessSecret = _configuration["Jwt:AccessSecret"]!;

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(accessSecret)
        );

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256
        );

        var claims = new[]
        {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Name, user.Username)
    };

        var jwt = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(
                int.Parse(_configuration["Jwt:ExpiresMinutes"]!)
            ),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
    public async Task LogoutAsync(string refreshToken)
    {
        await _refreshTokenRepository.DeleteAsync(refreshToken);
    }

}