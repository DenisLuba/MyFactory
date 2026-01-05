using MyFactory.WebApi.Contracts.ExpenceTypes;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.ExpenceTypes;

public sealed class ExpenseTypeDetailsResponseExample : IExamplesProvider<ExpenseTypeResponse>
{
    public ExpenseTypeResponse GetExamples() => new(
        Id: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"),
        Name: "Аренда",
        Description: "Офисные помещения");
}
