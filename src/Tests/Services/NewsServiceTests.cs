using Core.Entities;
using Core.Ports.Services.NytNews;
using Core.Services.News;
using FluentAssertions;
using Moq;

namespace Tests.Services;

public class NewsServiceTests
{
    private readonly Mock<INytNewsPort> _nytNewsPortMock;
    private readonly NewsService _newsService;

    public NewsServiceTests()
    {
        _nytNewsPortMock = new Mock<INytNewsPort>();
        _newsService = new NewsService(_nytNewsPortMock.Object);
    }

    [Fact]
    public async Task SearchArticlesAsync_ShouldReturnArticlesFromPort()
    {
        var expectedArticles = new List<Article> { new Article { Title = "Test Article 1" } };
        _nytNewsPortMock.Setup(p => p.SearchArticlesAsync("test", null, 0)).ReturnsAsync(expectedArticles);

        var result = await _newsService.SearchArticlesAsync("test");

        result.Should().BeEquivalentTo(expectedArticles);
        _nytNewsPortMock.Verify(p => p.SearchArticlesAsync("test", null, 0), Times.Once);
    }

    [Fact]
    public async Task GetMostPopularAsync_ShouldReturnArticlesFromPort()
    {
        var expectedArticles = new List<Article> { new Article { Title = "Popular Article 1" } };
        _nytNewsPortMock.Setup(p => p.GetMostPopularAsync(7)).ReturnsAsync(expectedArticles);

        var result = await _newsService.GetMostPopularAsync(7);

        result.Should().BeEquivalentTo(expectedArticles);
        _nytNewsPortMock.Verify(p => p.GetMostPopularAsync(7), Times.Once);
    }
}