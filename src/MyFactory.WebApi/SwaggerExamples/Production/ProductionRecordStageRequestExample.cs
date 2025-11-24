using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.Contracts.Production;

public class ProductionRecordStageRequestExample : IExamplesProvider<ProductionRecordStageRequest>
{
    public ProductionRecordStageRequest GetExamples() =>
        new ProductionRecordStageRequest(
            Stage: "cutting",
            Quantity: 4
        );
}

