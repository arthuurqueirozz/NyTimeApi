using Core.Entities;

namespace Core.Ports.Services.News;

public interface INewsService
{
    Task<IEnumerable<Article>> SearchArticlesAsync(string keyword, string? section = null, int page = 0);
    Task<IEnumerable<Article>> GetMostPopularAsync(int period);

}