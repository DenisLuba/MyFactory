using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Specifications;

namespace MyFactory.WebApi.SwaggerExamples.Specifications;

public class SpecificationsCreateRequestExample : IExamplesProvider<SpecificationsCreateRequest>
{
    public SpecificationsCreateRequest GetExamples() =>
    new("SP-001", "Пижама женская", 2.5, "Базовый комплект для сна");
}

