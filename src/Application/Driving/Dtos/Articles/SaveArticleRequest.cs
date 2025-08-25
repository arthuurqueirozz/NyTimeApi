using Core.Entities;

namespace Application.Driving.Dtos.Articles;

public class SaveArticleRequest
{
    public Article Article { get; set; } = null!;
}