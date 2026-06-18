using MyFactory.WebApi.Contracts.ProductionOrders;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.ProductionOrders;

public sealed class AvailableProductionStageEmployeesResponseExample
    : IExamplesProvider<IReadOnlyList<AvailableProductionStageEmployeeResponse>>
{
    public IReadOnlyList<AvailableProductionStageEmployeeResponse> GetExamples() =>
    [
        new(
            Id: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0021"),
            FullName: "Владимиров Д.С.",
            DepartmentName: "Швейный цех 1",
            PositionName: "Швея",
            IsActive: true),
        new(
            Id: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0022"),
            FullName: "Петрова Н.А.",
            DepartmentName: "Швейный цех 1",
            PositionName: "Швея",
            IsActive: true)
    ];
}
