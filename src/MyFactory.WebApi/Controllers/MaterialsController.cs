using Microsoft.AspNetCore.Mvc;
using MyFactory.WebApi.Contracts.Materials;
using MyFactory.WebApi.SwaggerExamples.Materials;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/materials")]
public class MaterialsController : ControllerBase
{
    private static readonly Guid Mat1 = Guid.Parse("11111111-1111-1111-1111-111111111111");
    private static readonly Guid Mat2 = Guid.Parse("22222222-2222-2222-2222-222222222222");

    private static readonly Guid Type1 = Guid.Parse("aaaaaaa1-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
    private static readonly Guid Type2 = Guid.Parse("aaaaaaa2-aaaa-aaaa-aaaa-aaaaaaaaaaaa");

    private static readonly Guid Supplier1 = Guid.Parse("bbbbbbb1-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
    private static readonly Guid Supplier2 = Guid.Parse("bbbbbbb2-bbbb-bbbb-bbbb-bbbbbbbbbbbb");

    [HttpGet]
    public IActionResult List([FromQuery] string? type = null)
        => Ok(new[]
        {
            new MaterialResponse(
                Id: Mat1,
                Code: "МАТ-001",
                Name: "Ткань Ситец",
                MaterialTypeId: Type1,
                Unit: "m",
                IsActive: true,
                LastPrice: 180.0m,
                Suppliers:
                [
                    new SupplierPrice(
                        Id: Supplier1,
                        Name: "Фабрика ткани",
                        MaterialPrice: 160.0m
                    )
                ]
            ),
            new MaterialResponse(
                Id: Mat2,
                Code: "МАТ-002",
                Name: "Молния 20 см",
                MaterialTypeId: Type2,
                Unit: "шт",
                IsActive: true,
                LastPrice: 100.0m,
                Suppliers:
                [
                    new SupplierPrice(
                        Id: Supplier2,
                        Name: "Фабрика фурнитуры",
                        MaterialPrice: 120.0m
                    )
                ]
            )
        });

    [HttpGet("{id}")]
    public IActionResult Get(string id)
        => Ok(
            new MaterialResponse(
                Id: Mat1,
                Code: "МАТ-001",
                Name: "Ткань Ситец",
                MaterialTypeId: Type1,
                Unit: "m",
                IsActive: true,
                LastPrice: 180.0m,
                Suppliers:
                [
                    new SupplierPrice(
                        Id: Supplier1,
                        Name: "Фабрика ткани",
                        MaterialPrice: 160.0m
                    )
                ]
            )
        );

    [HttpPost]
    public IActionResult Create([FromBody] CreateMaterialRequest request)
        => Created("", new CreateMaterialResponse(MaterialStatus.Created));

    [HttpPut("{id}")]
    public IActionResult Update(string id, [FromBody] UpdateMaterialRequest request)
        => Ok(new UpdateMaterialResponse(MaterialStatus.Updated, Guid.Parse(id)));

    [HttpGet("{id}/price-history")]
    public IActionResult PriceHistory(string id)
        => Ok(new[]
        {
            new MaterialPriceHistoryResponse(
                MaterialId: Guid.Parse(id),
                SupplierId: Supplier1,
                Price: 175.0m,
                EffectiveFrom: new DateTime(2025, 11, 1)
            )
        });

    [HttpPost("{id}/prices")]
    public IActionResult AddPrice(string id, [FromBody] AddMaterialPriceRequest request)
        => Ok(new AddMaterialPriceResponse(MaterialPriceStatus.PriceUpdated, Guid.Parse(id)));
}

