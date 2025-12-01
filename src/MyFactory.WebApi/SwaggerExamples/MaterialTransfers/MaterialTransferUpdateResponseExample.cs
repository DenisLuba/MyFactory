using MyFactory.WebApi.Contracts.MaterialTransfers;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.MaterialTransfers;

public class MaterialTransferUpdateResponseExample : IExamplesProvider<MaterialTransferUpdateResponse>
{
    public MaterialTransferUpdateResponse GetExamples() =>
        new(
            TransferId: Guid.Parse("bbbb1111-2222-3333-4444-555555555555"),
            Status: MaterialTransferStatus.Draft);
}
