using FluentValidation;

namespace MyFactory.Application.Features.Files.Queries.GetFileById;

public sealed class GetFileByIdQueryValidator : AbstractValidator<GetFileByIdQuery>
{
    public GetFileByIdQueryValidator()
    {
        RuleFor(query => query.FileId).NotEmpty();
    }
}
