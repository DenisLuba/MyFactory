using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.Contracts.Production;

public class ProductionAssignWorkerResponseExample : IExamplesProvider<ProductionAssignWorkerResponse>
{
    public ProductionAssignWorkerResponse GetExamples() =>
        new ProductionAssignWorkerResponse(
            OrderId: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
            Status: ProductionStatus.WorkerAssigned
        );
}

