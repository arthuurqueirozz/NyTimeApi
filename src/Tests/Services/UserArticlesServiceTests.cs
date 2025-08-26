using Core.Entities;
using Core.Ports.Repositories.Article;
using Core.Services.User;
using FluentAssertions;
using Moq;

namespace Tests.Services;

public class UserArticlesServiceTests
{
    private readonly Mock<IArticleRepository> _articleRepositoryMock;
    private readonly UserArticlesService _userArticlesService;

    public UserArticlesServiceTests()
    {
        _articleRepositoryMock = new Mock<IArticleRepository>();
        _userArticlesService = new UserArticlesService(_articleRepositoryMock.Object);
    }

    [Fact]
    public async Task SaveArticleAsync_ShouldSetUserIdAndCallRepository()
    {
        var article = new Article { Id = Guid.NewGuid(), Title = "Article to Save" };
        var userId = Guid.NewGuid();

        await _userArticlesService.SaveArticleAsync(article, userId);

        article.UserId.Should().Be(userId);
        _articleRepositoryMock.Verify(r => r.AddAsync(article), Times.Once);
    }

    [Fact]
    public async Task GetUserArticlesAsync_ShouldReturnArticlesFromRepository()
    {
        var userId = Guid.NewGuid();
        var expectedArticles = new List<Article> { new Article { Title = "User Article" } };
        _articleRepositoryMock.Setup(r => r.GetByUserIdAsync(userId)).ReturnsAsync(expectedArticles);

        var result = await _userArticlesService.GetUserArticlesAsync(userId);

        result.Should().BeEquivalentTo(expectedArticles);
    }

    [Fact]
    public async Task DeleteArticleAsync_ShouldCallRepositoryDelete()
    {
        var articleId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        await _userArticlesService.DeleteArticleAsync(articleId, userId);

        _articleRepositoryMock.Verify(r => r.DeleteAsync(articleId, userId), Times.Once);
    }
}