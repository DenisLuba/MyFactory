using System;
using System.Collections.Generic;
using MyFactory.WebApi.Contracts.Materials;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Materials;

public class MaterialCardResponseExample : IExamplesProvider<MaterialCardResponse>
{
    public MaterialCardResponse GetExamples() => new(
        Id: Guid.Parse("11111111-1111-1111-1111-111111111111"),
        Code: "МАТ-001",
        Name: "Ткань Ситец",
        MaterialType: "Ткань",
        Unit: "м",
        IsActive: true,
        LastPrice: 180.50m,
        PriceHistory: new List<MaterialPriceHistoryItem>
        {
            new(
                SupplierId: Guid.Parse("bbbbbbb1-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                SupplierName: "Фабрика ткани",
                Price: 175.00m,
                EffectiveFrom: new DateTime(2025, 11, 1)
            ),
            new(
                SupplierId: Guid.Parse("bbbbbbb2-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                SupplierName: "ООО \"Текстильные решения\"",
                Price: 182.00m,
                EffectiveFrom: new DateTime(2025, 09, 15)
            )
        }
    );
}
