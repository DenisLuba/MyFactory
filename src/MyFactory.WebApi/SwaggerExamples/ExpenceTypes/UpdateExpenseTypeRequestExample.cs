using MyFactory.WebApi.Contracts.ExpenceTypes;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.ExpenceTypes;

public sealed class UpdateExpenseTypeRequestExample : IExamplesProvider<UpdateExpenseTypeRequest>
{
    public UpdateExpenseTypeRequest GetExamples() => new(
        Name: "Коммуналка",
        Description: "Свет, вода, отопление");
}
