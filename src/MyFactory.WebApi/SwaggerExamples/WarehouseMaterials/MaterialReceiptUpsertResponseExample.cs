using MyFactory.WebApi.Contracts.WarehouseMaterials;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.WarehouseMaterials;

public class MaterialReceiptUpsertResponseExample : IExamplesProvider<MaterialReceiptUpsertResponse>
{
    public MaterialReceiptUpsertResponse GetExamples() =>
        new(
            Id: Guid.Parse("aaaaaaaa-0000-0000-0000-000000000010"),
            Status: MaterialReceiptStatus.Updated
        );
}
