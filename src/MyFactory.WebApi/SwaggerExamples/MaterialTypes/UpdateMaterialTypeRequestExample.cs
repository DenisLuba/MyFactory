using MyFactory.WebApi.Contracts.MaterialTypes;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.MaterialTypes;

public sealed class UpdateMaterialTypeRequestExample : IExamplesProvider<UpdateMaterialTypeRequest>
{
    public UpdateMaterialTypeRequest GetExamples() => new(
        Name: "Фурнитура",
        Description: "Обновленное описание категории фурнитуры");
}
