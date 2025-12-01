using System;
using System.Collections.Generic;
using MyFactory.WebApi.Contracts.Materials;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Materials;

public class MaterialPriceHistoryResponseExample : IExamplesProvider<IEnumerable<MaterialPriceHistoryItem>>
{
    public IEnumerable<MaterialPriceHistoryItem> GetExamples() => new[]
    {
        new MaterialPriceHistoryItem(
            SupplierId: Guid.Parse("22222222-2222-2222-2222-222222222222"),
            SupplierName: "Фабрика ткани",
            Price: 175.50m,
            EffectiveFrom: new DateTime(2025, 11, 1)
        )
    };
}
