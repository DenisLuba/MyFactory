using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.Contracts.Production;

public class ProductionAssignWorkerRequestExample : IExamplesProvider<ProductionAssignWorkerRequest>
{
    public ProductionAssignWorkerRequest GetExamples() =>
        new ProductionAssignWorkerRequest(
            EmployeeId: Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"),
            Quantity: 2
        );
}

