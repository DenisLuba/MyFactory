using MyFactory.WebApi.Contracts.Files;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Files;

public class DeleteFileResponseExample : IExamplesProvider<DeleteFileResponse>
{
    public DeleteFileResponse GetExamples() =>
        new(
            Status: FileStatus.Deleted,
            FileId: Guid.Parse("11111111-1111-1111-1111-111111111111")
        );
}

