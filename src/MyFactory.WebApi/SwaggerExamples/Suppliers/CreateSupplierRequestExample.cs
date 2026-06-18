using MyFactory.WebApi.Contracts.Suppliers;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Suppliers;

public sealed class CreateSupplierRequestExample : IExamplesProvider<CreateSupplierRequest>
{
    public CreateSupplierRequest GetExamples() => new(
        Name: "ТексМаркет",
        Description: "Оптовый поставщик тканей и фурнитуры");
}
