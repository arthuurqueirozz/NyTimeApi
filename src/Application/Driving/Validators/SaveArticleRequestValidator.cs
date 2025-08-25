using Application.Driving.Dtos.Articles;
using FluentValidation;

namespace Application.Driving.Validators;

public class SaveArticleRequestValidator : AbstractValidator<SaveArticleRequest>
{
    public SaveArticleRequestValidator()
    {
        RuleFor(x => x.Article).NotNull().WithMessage("O artigo não pode ser nulo.");
        RuleFor(x => x.Article.NytId).NotEmpty().When(x => x.Article != null).WithMessage("A ID do NYT é obrigatória.");
        RuleFor(x => x.Article.Title).NotEmpty().When(x => x.Article != null).WithMessage("O título do artigo é obrigatório.");
        RuleFor(x => x.Article.Url).NotEmpty().When(x => x.Article != null).WithMessage("A URL do artigo é obrigatória.");
    }
}