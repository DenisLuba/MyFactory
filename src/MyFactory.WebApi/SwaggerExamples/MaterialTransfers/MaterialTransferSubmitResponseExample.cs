using MyFactory.WebApi.Contracts.MaterialTransfers;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.MaterialTransfers;

public class MaterialTransferSubmitResponseExample : IExamplesProvider<MaterialTransferSubmitResponse>
{
    public MaterialTransferSubmitResponse GetExamples() =>
        new(
            TransferId: Guid.Parse("bbbb1111-2222-3333-4444-555555555555"),
            Status: MaterialTransferStatus.Submitted,
            SubmittedAt: new DateTime(2025, 11, 13, 9, 30, 0, DateTimeKind.Utc));
}
