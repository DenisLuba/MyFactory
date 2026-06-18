using MyFactory.WebApi.Contracts.ExpenceTypes;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.ExpenceTypes;

public sealed class ExpenseTypeListResponseExample : IExamplesProvider<IReadOnlyList<ExpenseTypeResponse>>
{
    public IReadOnlyList<ExpenseTypeResponse> GetExamples() => new List<ExpenseTypeResponse>
    {
        new(
            Id: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"),
            Name: "Аренда",
            Description: "Помещения"),
        new(
            Id: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0002"),
            Name: "Коммуналка",
            Description: "Свет, вода")
    };
}
