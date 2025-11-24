using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.Contracts.Production;

public class ProductionRecordStageResponseExample : IExamplesProvider<ProductionRecordStageResponse>
{
    public ProductionRecordStageResponse GetExamples() =>
        new ProductionRecordStageResponse(
            OrderId: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
            Status: ProductionStatus.StageRecorded
        );
}

