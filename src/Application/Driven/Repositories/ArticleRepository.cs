using Application.Driven.Database;
using Core.Entities;
using Core.Ports.Repositories.Article;
using Microsoft.EntityFrameworkCore;

namespace Application.Driven.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly AppDbContext _db;

        public ArticleRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(Article article)
        {
            _db.Articles.Add(article);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Article>> GetByUserIdAsync(Guid userId)
        {
            return await _db.Articles
                .Where(a => a.UserId == userId)
                .ToListAsync();
        }

        public async Task DeleteAsync(Guid articleId, Guid userId)
        {
            var article = await _db.Articles
                .FirstOrDefaultAsync(a => a.Id == articleId && a.UserId == userId);

            if (article != null)
            {
                _db.Articles.Remove(article);
                await _db.SaveChangesAsync();
            }
        }
    }
}