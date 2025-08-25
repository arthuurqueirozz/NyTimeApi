using Core.Entities;

namespace Core.Ports.Services.User;

public interface IUserArticlesService
{
    Task SaveArticleAsync(Article article, Guid userId);
    Task<IEnumerable<Article>> GetUserArticlesAsync(Guid userId);
    Task DeleteArticleAsync(Guid articleId, Guid userId);

}