using MyFactory.WebApi.Contracts.WarehouseMaterials;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.WarehouseMaterials;

public class MaterialReceiptLineDeleteResponseExample : IExamplesProvider<MaterialReceiptLineDeleteResponse>
{
    public MaterialReceiptLineDeleteResponse GetExamples()
        => new(
            ReceiptId: Guid.Parse("aaaaaaaa-0000-0000-0000-000000000001"),
            LineId: Guid.Parse("bbbbbbbb-0000-0000-0000-000000000001"),
            Status: MaterialReceiptStatus.LineDeleted
        );
}
