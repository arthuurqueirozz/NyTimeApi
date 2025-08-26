using Application.Driving.Dtos.Articles;
using FluentValidation;

namespace Application.Driving.Validators;

public abstract class SaveArticleRequestValidator : AbstractValidator<SaveArticleRequest>
{
    public SaveArticleRequestValidator()
    {
        RuleFor(x => x.NytId).NotEmpty().WithMessage("A ID do NYT é obrigatória.");
        RuleFor(x => x.Title).NotEmpty().WithMessage("O título do artigo é obrigatório.");
        RuleFor(x => x.Url).NotEmpty().WithMessage("A URL do artigo é obrigatória.");
    }
}