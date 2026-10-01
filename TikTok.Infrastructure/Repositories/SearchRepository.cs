using Microsoft.EntityFrameworkCore;
using TikTok.Application.Interfaces;
using TikTok.Application.DTOs.Search;
using TikTok.Application.DTOs;
using TikTok.Application.DTOs.Video;
using TikTok.Infrastructure.Data;
using TikTok.Domain.Entities;
namespace TikTok.Infrastructure.Repositories;

public class SearchRepository : ISearchRepository
{
    private readonly AppDbContext _context;

    public SearchRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<SearchDto> SearchAsync(string query)
    {
        var videos = await _context.Videos
            .Where(x => x.Caption.Contains(query))
            .Select(x => new VideoDto
            {
                Id = x.Id,
                VideoUrl = x.VideoUrl,
                Caption = x.Caption,
                Views = x.Views,
                UserId = x.UserId,
                Username = x.User.Username,
                Avatar = x.User.Avatar,
                LikeCount = x.VideoLikes.Count,
            })
            .ToListAsync();
        var users = await _context.Users
            .Where(x => x.Username.Contains(query))
            .Select(x => new UserDto
            {
                Id = x.Id,
                Username = x.Username,
                Avatar = x.Avatar,

            })
            .ToListAsync();
        return new SearchDto
        {
            Videos = videos,
            Users = users
        };
    }
    public async Task AddHistoryAsync(Guid userId, AddSearchHistoryRequest request)
    {
        var search = new Search
        {
            UserId = userId,
            Query = request.Query
        };

        _context.Search.Add(search);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Search>> GetHistoryAsync(Guid userId)
    {
        return await _context.Search
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .Take(10)
            .ToListAsync();
    }
}