
using TikTok.Application.DTOs.Video;

namespace TikTok.Application.DTOs.Search;

public class SearchDto
{
    public List<VideoDto> Videos { get; set; } = [];
    public List<UserDto> Users { get; set; } = [];

}