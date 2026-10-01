namespace TikTok.Application.DTOs.Video;

public class CreateVideoRequest
{
    public string VideoUrl { get; set; } = string.Empty;
    public string Caption { get; set; } = string.Empty;
}