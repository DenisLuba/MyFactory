using FluentValidation;

namespace MyFactory.Application.Features.Files.Queries.ListFiles;

public sealed class ListFilesQueryValidator : AbstractValidator<ListFilesQuery>
{
    public ListFilesQueryValidator()
    {
        RuleFor(query => query)
            .Must(query =>
                !query.UploadedFrom.HasValue ||
                !query.UploadedTo.HasValue ||
                query.UploadedFrom <= query.UploadedTo)
            .WithMessage("Uploaded from date must be earlier than uploaded to date.");
    }
}
