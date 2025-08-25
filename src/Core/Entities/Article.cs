namespace Core.Entities;

public class Article
{
    public Guid Id { get; set; } = Guid.NewGuid();


    public string NytId { get; set; } = string.Empty; 
    public string Title { get; set; } = string.Empty;
    public string Abstract { get; set; } = string.Empty;
    public string Section { get; set; } = string.Empty;
    public string Subsection { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public DateTime PublishedAt { get; set; }
    public string Url { get; set; } = string.Empty;
    public string ThumbnailUrl { get; set; } = string.Empty;

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}