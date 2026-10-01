using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TikTok.Application.DTOs;
using TikTok.Application.Interfaces;
using TikTok.Domain.Entities;

namespace TikTok.API.Controllers;

[ApiController]
[Route("api/search")]
public class SearchController : ControllerBase
{
    private readonly ISearchService _searchService;
    public SearchController(ISearchService searchService)
    {
        _searchService = searchService;

    }

    [HttpGet("{query}")]
    public async Task<IActionResult> Search(string query)
    {
        var search = await _searchService.SearchAsync(query);
        return Ok(new
        {
            success = true,
            data = search
        });
    }

    [HttpPost("history")]
    public async Task<IActionResult> AddHistory(AddSearchHistoryRequest request)
    {
        var userId = Guid.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!
        );

        if (string.IsNullOrWhiteSpace(request.Query))
            return BadRequest();

        await _searchService.AddHistoryAsync(userId, request);

        return Ok(new
        {
            success = true
        });
    }

    [HttpGet("history")]
    public async Task<IActionResult> GetHistory()
    {
        var userId = Guid.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!
        );

        var history = await _searchService.GetHistoryAsync(userId);

        return Ok(new
        {
            success = true,
            data = history
        });
    }
}
