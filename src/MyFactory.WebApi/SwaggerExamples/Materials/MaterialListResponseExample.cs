using System;
using System.Collections.Generic;
using MyFactory.WebApi.Contracts.Materials;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Materials;

public class MaterialListResponseExample : IExamplesProvider<IEnumerable<MaterialListResponse>>
{
    public IEnumerable<MaterialListResponse> GetExamples() => new[]
    {
        new MaterialListResponse(
            Id: Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Code: "МАТ-001",
            Name: "Ткань Ситец",
            MaterialType: "Ткань",
            Unit: "м",
            IsActive: true,
            LastPrice: 180.50m
        ),
        new MaterialListResponse(
            Id: Guid.Parse("22222222-2222-2222-2222-222222222222"),
            Code: "МАТ-002",
            Name: "Молния 20 см",
            MaterialType: "Фурнитура",
            Unit: "шт",
            IsActive: true,
            LastPrice: 99.90m
        )
    };
}
