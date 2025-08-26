using Core.Entities;

namespace Application.Driving.Dtos.Articles;

public class SaveArticleRequest
{
    public string NytId { get; set; } = string.Empty; 
    public string Title { get; set; } = string.Empty;
    public string Abstract { get; set; } = string.Empty;
    public string Section { get; set; } = string.Empty;
    public string Subsection { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public DateTime PublishedAt { get; set; }
    public string Url { get; set; } = string.Empty;
    public string ThumbnailUrl { get; set; } = string.Empty;
}