using Core.Entities;
using Core.Ports.Services.News;
using Core.Ports.Services.NytNews;

namespace Core.Services.News;

public class NewsService : INewsService
{
    private readonly INytNewsPort _nytNewsPort;

    public NewsService(INytNewsPort nytNewsPort)
    {
        _nytNewsPort = nytNewsPort;
    }

    public async Task<IEnumerable<Article>> SearchArticlesAsync(string keyword, string? section = null, int page = 0)
    {
        return await _nytNewsPort.SearchArticlesAsync(keyword, section, page);
    }

    public async Task<IEnumerable<Article>> GetMostPopularAsync(int period)
    {
        return await _nytNewsPort.GetMostPopularAsync(period);
    }
}