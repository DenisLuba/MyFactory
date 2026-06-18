using MyFactory.WebApi.Contracts.ExpenceTypes;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.ExpenceTypes;

public sealed class CreateExpenseTypeRequestExample : IExamplesProvider<CreateExpenseTypeRequest>
{
    public CreateExpenseTypeRequest GetExamples() => new(
        Name: "Аренда",
        Description: "Офисные помещения");
}
