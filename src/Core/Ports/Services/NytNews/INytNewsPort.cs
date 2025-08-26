using Core.Entities;

namespace Core.Ports.Services.NytNews;

public interface INytNewsPort
{
    Task<IEnumerable<Article>> SearchArticlesAsync(string keyword, string? section = null, int page = 0);
    Task<IEnumerable<Article>> GetMostPopularAsync(int period);
}