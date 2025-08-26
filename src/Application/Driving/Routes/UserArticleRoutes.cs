using System.Security.Claims;
using Application.Driving.Dtos.Articles;
using Core.Entities;
using Core.Ports.Services.User;
using Microsoft.AspNetCore.Mvc;

namespace Application.Driving.Routes;

public static class UserArticlesRoutes
    {
        public static void ConfigureUserArticlesRoutes(this WebApplication app)
        {
            var group = app.MapGroup("/api/user/articles").WithTags("User Articles").RequireAuthorization();

            group.MapGet("", GetUserSavedArticles)
                .WithName("GetUserArticles")
                .Produces<IEnumerable<Article>>()
                .WithSummary("Busca os artigos salvos pelo usuário")
                .WithDescription("Retorna uma lista de todos os artigos que o usuário autenticado salvou.");

            group.MapPost("", SaveArticleForUser)
                .WithName("SaveUserArticle")
                .Produces<Article>(StatusCodes.Status201Created)
                .Produces(StatusCodes.Status400BadRequest)
                .WithSummary("Salva um novo artigo para o usuário")
                .WithDescription("Associa um artigo ao usuário autenticado.");

            group.MapDelete("/{articleId:guid}", DeleteArticleForUser)
                .WithName("DeleteUserArticle")
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status404NotFound)
                .WithSummary("Exclui um artigo salvo")
                .WithDescription("Remove a associação de um artigo com o usuário autenticado.");
        }
        
        private static Guid GetCurrentUserId(HttpContext httpContext)
        {
            var userIdClaim = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(userIdClaim, out var userId) ? userId : Guid.Empty;
        }

        private static async Task<IResult> GetUserSavedArticles(HttpContext httpContext, IUserArticlesService userArticlesService)
        {
            var userId = GetCurrentUserId(httpContext);
            if (userId == Guid.Empty) return Results.Unauthorized();
            
            var articles = await userArticlesService.GetUserArticlesAsync(userId);
            return Results.Ok(articles);
        }

        private static async Task<IResult> SaveArticleForUser(
            [FromBody] SaveArticleRequest request, 
            HttpContext httpContext, 
            IUserArticlesService userArticlesService)
        {
            var userId = GetCurrentUserId(httpContext);
            if (userId == Guid.Empty) return Results.Unauthorized();

            await userArticlesService.SaveArticleAsync(request.Article, userId);
            return Results.Created($"/api/user/articles/{request.Article.Id}", request.Article);
        }

        private static async Task<IResult> DeleteArticleForUser(Guid articleId, HttpContext httpContext, IUserArticlesService userArticlesService)
        {
            var userId = GetCurrentUserId(httpContext);
            if (userId == Guid.Empty) return Results.Unauthorized();
            
            await userArticlesService.DeleteArticleAsync(articleId, userId);
            return Results.NoContent();
        }
    }