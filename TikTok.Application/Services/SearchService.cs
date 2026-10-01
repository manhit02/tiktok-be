using TikTok.Application.DTOs.Search;
using TikTok.Application.Interfaces;
using TikTok.Application.DTOs;
using TikTok.Domain.Entities;


namespace TikTok.Application.Services;

public class SearchService : ISearchService
{
    private readonly ISearchRepository _SearchRepository;
    public SearchService(ISearchRepository SearchRepository)
    {
        _SearchRepository = SearchRepository;
    }

    public async Task<SearchDto> SearchAsync(string query)
    {

        return await _SearchRepository.SearchAsync(query);
    }
    public async Task AddHistoryAsync(Guid userId, AddSearchHistoryRequest request)
    {
        await _SearchRepository.AddHistoryAsync(userId, request);
    }

    public async Task<List<AddSearchHistoryRequest>> GetHistoryAsync(Guid userId)
    {
        var history = await _SearchRepository.GetHistoryAsync(userId);
        return history.Select(x => new AddSearchHistoryRequest { Query = x.Query }).ToList();
    }
}