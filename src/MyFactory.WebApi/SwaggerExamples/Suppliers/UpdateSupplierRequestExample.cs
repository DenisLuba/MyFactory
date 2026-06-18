using MyFactory.WebApi.Contracts.Suppliers;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Suppliers;

public sealed class UpdateSupplierRequestExample : IExamplesProvider<UpdateSupplierRequest>
{
    public UpdateSupplierRequest GetExamples() => new(
        Name: "ТексМаркет (обновл.)",
        Description: "Обновлённое описание поставщика");
}
