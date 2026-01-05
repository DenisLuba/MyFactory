using MyFactory.WebApi.Contracts.ProductionOrders;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.ProductionOrders;

public sealed class ProductionStageEmployeesResponseExample : IExamplesProvider<IReadOnlyList<ProductionStageEmployeeResponse>>
{
    public IReadOnlyList<ProductionStageEmployeeResponse> GetExamples() => new List<ProductionStageEmployeeResponse>
    {
        new(
            EmployeeId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"),
            EmployeeName: "Иванова О.Г.",
            PlanPerHour: 2.5m,
            AssignedQty: 60,
            CompletedQty: 40),
        new(
            EmployeeId: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0002"),
            EmployeeName: "Петров П.П.",
            PlanPerHour: 2.5m,
            AssignedQty: 60,
            CompletedQty: 20)
    };
}
