using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Suppliers;

namespace MyFactory.WebApi.SwaggerExamples.Suppliers;

public class SuppliersCreateUpdateRequestExample : IExamplesProvider<SuppliersCreateUpdateRequest>
{
    public SuppliersCreateUpdateRequest GetExamples()
        => new(
            Name: "ТексМаркет",
            SupplierType: SupplierTypes.Materials,
            Status: SupplierStatus.Active,
            Address: "г. Иваново, ул. Текстильщиков, 12",
            Phone: "+7 (900) 111–22–33",
            Email: "texmarket@mail.co");
}

