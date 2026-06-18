using MyFactory.WebApi.Contracts.Advances;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Advances;

public sealed class CashAdvanceListExample : IExamplesProvider<IReadOnlyList<CashAdvanceListItemResponse>>
{
    public IReadOnlyList<CashAdvanceListItemResponse> GetExamples() => new List<CashAdvanceListItemResponse>
    {
        new(
            Id: Guid.Parse("aaaaaaaa-aa01-4a0a-b001-aaaaaaaa0001"),
            IssueDate: new DateOnly(2025, 3, 5),
            EmployeeName: "Иванов И.И.",
            IssuedAmount: 20000m,
            SpentAmount: 18500m,
            ReturnedAmount: 1000m,
            Balance: 500m,
            IsClosed: false),
        new(
            Id: Guid.Parse("bbbbbbbb-bb01-4a0a-b001-bbbbbbbb0001"),
            IssueDate: new DateOnly(2025, 3, 10),
            EmployeeName: "Петров П.П.",
            IssuedAmount: 15000m,
            SpentAmount: 15000m,
            ReturnedAmount: 0m,
            Balance: 0m,
            IsClosed: true)
    };
}
