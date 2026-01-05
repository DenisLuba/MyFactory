using MyFactory.Domain.Entities.Production;
using MyFactory.WebApi.Contracts.Employees;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Employees;

public sealed class EmployeeProductionAssignmentsExample : IExamplesProvider<IReadOnlyList<EmployeeProductionAssignmentResponse>>
{
    public IReadOnlyList<EmployeeProductionAssignmentResponse> GetExamples() => new List<EmployeeProductionAssignmentResponse>
    {
        new(
            ProductionOrderId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"),
            ProductionOrderNumber: "PO-001",
            Stage: ProductionOrderStatus.Cutting,
            QtyAssigned: 10,
            QtyCompleted: 6),
        new(
            ProductionOrderId: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0002"),
            ProductionOrderNumber: "PO-002",
            Stage: ProductionOrderStatus.Sewing,
            QtyAssigned: 8,
            QtyCompleted: 5)
    };
}
