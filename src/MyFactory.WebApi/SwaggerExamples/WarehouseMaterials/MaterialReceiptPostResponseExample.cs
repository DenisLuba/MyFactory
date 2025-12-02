using MyFactory.WebApi.Contracts.WarehouseMaterials;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.WarehouseMaterials;

public class MaterialReceiptPostResponseExample : IExamplesProvider<MaterialReceiptPostResponse>
{
    public MaterialReceiptPostResponse GetExamples() =>
        new(
            Id: Guid.Parse("aaaaaaaa-0000-0000-0000-000000000001"),
            Status: MaterialReceiptStatus.Posted,
            PostedAt: DateTime.UtcNow
        );
}
