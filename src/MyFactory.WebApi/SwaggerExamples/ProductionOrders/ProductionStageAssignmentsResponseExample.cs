using MyFactory.Domain.Entities.Production;
using MyFactory.WebApi.Contracts.ProductionOrders;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.ProductionOrders;

public sealed class ProductionStageAssignmentsResponseExample : IExamplesProvider<IReadOnlyList<ProductionStageAssignmentResponse>>
{
    public IReadOnlyList<ProductionStageAssignmentResponse> GetExamples() =>
	[
        new(
            AssignmentId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"),
            Stage: ProductionStage.Sewing,
            EmployeeId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0021"),
            EmployeeName: "Владимиров Д.С.",
            PlanPerHour: 2.5m,
            AssignedQty: 60,
            CompletedQty: 40,
            WorkDate: new DateOnly(2025, 7, 5)),
        new(
            AssignmentId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0002"),
            Stage: ProductionStage.Cutting,
            EmployeeId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0011"),
            EmployeeName: "Иванов И.И.",
            PlanPerHour: null,
            AssignedQty: 60,
            CompletedQty: 40,
            WorkDate: new DateOnly(2024, 6, 1))
    ];
}
