namespace Core.Ports.Repositories.Article;

public interface IArticleRepository
{
    Task AddAsync(Entities.Article article);
    Task<IEnumerable<Entities.Article>> GetByUserIdAsync(Guid userId);
    Task DeleteAsync(Guid articleId, Guid userId);
}