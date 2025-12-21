using FluentValidation;

namespace MyFactory.Application.OldFeatures.Files.Queries.GetFileById;

public sealed class GetFileByIdQueryValidator : AbstractValidator<GetFileByIdQuery>
{
    public GetFileByIdQueryValidator()
    {
        RuleFor(query => query.FileId).NotEmpty();
    }
}
