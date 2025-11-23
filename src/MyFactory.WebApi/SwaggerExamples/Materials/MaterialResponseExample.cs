using MyFactory.WebApi.Contracts.Materials;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Materials;

public class MaterialResponseExample : IExamplesProvider<MaterialResponse>
{
    public MaterialResponse GetExamples() =>
        new(
            Id: Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Code: "MAT-001",
            Name: "Ткань Ситец",
            MaterialTypeId: Guid.Parse("33333333-3333-3333-3333-333333333333"),
            Unit: "m",
            IsActive: true,
            LastPrice: 180.50m,
            Suppliers:
            [
                new SupplierPrice(
                    Id: Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Name: "ТексМаркет",
                    MaterialPrice: 175.00m
                ),
                new SupplierPrice(
                    Id: Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    Name: "ТканиОпт",
                    MaterialPrice: 178.50m
                )
            ]
        );
}