using MyFactory.WebApi.Contracts.MaterialTransfers;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.MaterialTransfers;

public class MaterialTransferDeleteResponseExample : IExamplesProvider<MaterialTransferDeleteResponse>
{
    public MaterialTransferDeleteResponse GetExamples() =>
        new(
            TransferId: Guid.Parse("bbbb1111-2222-3333-4444-555555555555"),
            IsDeleted: true);
}
