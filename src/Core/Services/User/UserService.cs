using Core.Entities;
using Core.Ports.Repositories.Article;
using Core.Ports.Services.User;

namespace Core.Services.User
{
    public class UserArticlesService : IUserArticlesService 
    {
        private readonly IArticleRepository _articleRepository;

        public UserArticlesService(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public async Task SaveArticleAsync(Article article, Guid userId)
        {
            article.UserId = userId;
            await _articleRepository.AddAsync(article);
        }

        public async Task<IEnumerable<Article>> GetUserArticlesAsync(Guid userId)
        {
            return await _articleRepository.GetByUserIdAsync(userId);
        }

        public async Task DeleteArticleAsync(Guid articleId, Guid userId)
        {
            await _articleRepository.DeleteAsync(articleId, userId);
        }
    }
}