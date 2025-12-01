using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyFactory.WebApi.Contracts.Materials;
using MyFactory.WebApi.SwaggerExamples.Materials;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/materials")]
[Produces("application/json")]
public class MaterialsController : ControllerBase
{
    private static readonly Guid Mat1 = Guid.Parse("11111111-1111-1111-1111-111111111111");
    private static readonly Guid Mat2 = Guid.Parse("22222222-2222-2222-2222-222222222222");

    private static readonly Guid Supplier1 = Guid.Parse("bbbbbbb1-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
    private static readonly Guid Supplier2 = Guid.Parse("bbbbbbb2-bbbb-bbbb-bbbb-bbbbbbbbbbbb");

    private static readonly IReadOnlyList<MaterialPriceHistoryItem> Mat1PriceHistory = new List<MaterialPriceHistoryItem>
    {
        new(Supplier1, "Фабрика ткани", 175.0m, new DateTime(2025, 11, 1)),
        new(Supplier2, "ООО \"Текстильные решения\"", 182.0m, new DateTime(2025, 09, 15))
    };

    private static readonly IReadOnlyList<MaterialPriceHistoryItem> Mat2PriceHistory = new List<MaterialPriceHistoryItem>
    {
        new(Supplier2, "Фабрика фурнитуры", 115.0m, new DateTime(2025, 10, 10))
    };

    // GET /api/materials
    [HttpGet]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(MaterialListResponseExample))]
    [ProducesResponseType(typeof(IEnumerable<MaterialListResponse>), StatusCodes.Status200OK)]
    public IActionResult List([FromQuery] string? type = null)
        => Ok(new[]
        {
            new MaterialListResponse(Mat1, "МАТ-001", "Ткань Ситец", "Ткань", "м", true, 180.0m),
            new MaterialListResponse(Mat2, "МАТ-002", "Молния 20 см", "Фурнитура", "шт", true, 100.0m)
        });

    // GET /api/materials/{id}
    [HttpGet("{id}")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(MaterialCardResponseExample))]
    [ProducesResponseType(typeof(MaterialCardResponse), StatusCodes.Status200OK)]
    public IActionResult Get(Guid id)
        => Ok(
            new MaterialCardResponse(
                Id: id,
                Code: id == Mat2 ? "МАТ-002" : "МАТ-001",
                Name: id == Mat2 ? "Молния 20 см" : "Ткань Ситец",
                MaterialType: id == Mat2 ? "Фурнитура" : "Ткань",
                Unit: id == Mat2 ? "шт" : "м",
                IsActive: true,
                LastPrice: id == Mat2 ? 100.0m : 180.0m,
                PriceHistory: id == Mat2 ? Mat2PriceHistory : Mat1PriceHistory
            )
        );

    // POST /api/materials
    [HttpPost]
    [Consumes("application/json")]
    [SwaggerRequestExample(typeof(CreateMaterialRequest), typeof(CreateMaterialRequestExample))]
    [SwaggerResponseExample(201, typeof(CreateMaterialResponseExample))]
    [ProducesResponseType(typeof(CreateMaterialResponse), StatusCodes.Status201Created)]
    public IActionResult Create([FromBody] CreateMaterialRequest request)
        => Created("", new CreateMaterialResponse(MaterialStatus.Created));

    // PUT /api/materials/{id}
    [HttpPut("{id}")]
    [SwaggerRequestExample(typeof(UpdateMaterialRequest), typeof(UpdateMaterialRequestExample))]
    [SwaggerResponseExample(200, typeof(UpdateMaterialResponseExample))]
    [ProducesResponseType(typeof(UpdateMaterialResponse), StatusCodes.Status200OK)]
    public IActionResult Update(string id, [FromBody] UpdateMaterialRequest request)
        => Ok(new UpdateMaterialResponse(MaterialStatus.Updated, Guid.Parse(id)));

    // GET /api/materials/{id}/price-history
    [HttpGet("{id}/price-history")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(MaterialPriceHistoryResponseExample))]
    [ProducesResponseType(typeof(IEnumerable<MaterialPriceHistoryItem>), StatusCodes.Status200OK)]
    public IActionResult PriceHistory(Guid id)
        => Ok(id == Mat2 ? Mat2PriceHistory : Mat1PriceHistory);

    // POST /api/materials/{id}/prices
    [HttpPost("{id}/prices")]
    [Consumes("application/json")]
    [SwaggerRequestExample(typeof(AddMaterialPriceRequest), typeof(AddMaterialPriceRequestExample))]
    [SwaggerResponseExample(200, typeof(AddMaterialPriceResponseExample))]
    [ProducesResponseType(typeof(AddMaterialPriceResponse), StatusCodes.Status200OK)]
    public IActionResult AddPrice(string id, [FromBody] AddMaterialPriceRequest request)
        => Ok(new AddMaterialPriceResponse(MaterialPriceStatus.PriceUpdated, Guid.Parse(id)));

    // GET /api/materials/type
    [HttpGet("type")]
    [SwaggerResponseExample(200, typeof(MaterialTypeResponseExample))]
    [ProducesResponseType(typeof(MaterialTypeResponse), StatusCodes.Status200OK)]
    public IActionResult GetMaterialTypeById([FromQuery] Guid id)
        => Ok(new MaterialTypeResponse(id, "Ткань"));
}


