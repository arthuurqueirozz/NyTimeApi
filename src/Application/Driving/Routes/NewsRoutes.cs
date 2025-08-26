using Core.Entities;
using Core.Ports.Services.News;

namespace Application.Driving.Routes;

public static class NewsRoutes
{
    public static void ConfigureNewsRoutes(this WebApplication app)
    {
        var group = app.MapGroup("/api/news").WithTags("News").RequireAuthorization();

        group.MapGet("/search", SearchNyTimesArticles)
            .WithName("SearchArticles")
            .Produces<IEnumerable<Article>>()
            .WithSummary("Busca por artigos")
            .WithDescription("Obtém uma lista de artigos do New York Times com base em uma palavra-chave.");

        group.MapGet("/popular", GetMostPopularArticles)
            .WithName("GetPopularArticles")
            .Produces<IEnumerable<Article>>()
            .WithSummary("Busca os artigos mais populares")
            .WithDescription("Obtém a lista de artigos mais populares do New York Times em um determinado período.");
    }

    private static async Task<IResult> SearchNyTimesArticles(string keyword, string? section, int? page, INewsService newsService)
    {
        if (string.IsNullOrWhiteSpace(keyword))
        {
            return Results.BadRequest("O parâmetro 'keyword' é obrigatório.");
        }
        var articles = await newsService.SearchArticlesAsync(keyword, section, page ?? 0);
        return Results.Ok(articles);
    }

    private static async Task<IResult> GetMostPopularArticles(int? period, INewsService newsService)
    {
        var articles = await newsService.GetMostPopularAsync(period ?? 7);
        return Results.Ok(articles);
    }
}