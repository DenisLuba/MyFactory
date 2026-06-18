using MyFactory.WebApi.Contracts.ProductionOrders;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.ProductionOrders;

public sealed class RegisterSewingOperationRequestExample : IExamplesProvider<RegisterSewingOperationRequest>
{
    public RegisterSewingOperationRequest GetExamples() => new(
        AssignmentId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"),
        EmployeeId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0021"),
        QtySewn: 12,
        HoursWorked: 8,
        OperationDate: new DateOnly(2025, 7, 5));
}
