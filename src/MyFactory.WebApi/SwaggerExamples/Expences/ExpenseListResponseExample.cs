using MyFactory.WebApi.Contracts.Expences;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Expences;

public sealed class ExpenseListResponseExample : IExamplesProvider<IReadOnlyList<ExpenseListItemResponse>>
{
    public IReadOnlyList<ExpenseListItemResponse> GetExamples() => new List<ExpenseListItemResponse>
    {
        new(
            Id: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"),
            ExpenseDate: new DateOnly(2025, 3, 5),
            ExpenseTypeName: "Аренда",
            Amount: 25000m,
            Description: "Офис за март",
            CreatedBy: "user-1"),
        new(
            Id: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0002"),
            ExpenseDate: new DateOnly(2025, 3, 7),
            ExpenseTypeName: "Коммуналка",
            Amount: 4800m,
            Description: "Свет + вода",
            CreatedBy: "user-2")
    };
}
