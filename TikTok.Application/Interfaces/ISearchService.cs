using TikTok.Application.DTOs;
using TikTok.Application.DTOs.Search;
using TikTok.Domain.Entities;
namespace TikTok.Application.Interfaces;

public interface ISearchService
{
    public Task<SearchDto> SearchAsync(string query);
    public Task AddHistoryAsync(Guid userId, AddSearchHistoryRequest request);
    public Task<List<AddSearchHistoryRequest>> GetHistoryAsync(Guid userId);
}