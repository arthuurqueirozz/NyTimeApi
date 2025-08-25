namespace Core.Ports.News;

public interface INewsService
{
    Task<IEnumerable<Entities.Article>> SearchArticlesAsync(string keyword, string? section = null, int page = 0);
    Task<IEnumerable<Entities.Article>> GetMostPopularAsync(int period = 7);
}