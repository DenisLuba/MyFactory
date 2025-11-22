using Microsoft.AspNetCore.Mvc;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/materials")]
public class MaterialsController : ControllerBase
{
    [HttpGet]
    public IActionResult List([FromQuery] string? type = null)
        => Ok(new[] 
        {
            new { id = "mat-001", code="МАТ-001", name="Ткань Ситец", type="Ткань", unit="m", lastPrice=180.0 },
            new { id = "mat-002", code="МАТ-002", name="Молния 20 см", type="Фурнитура", unit="шт", lastPrice=12.0 }
        });

    [HttpGet("{id}")]
    public IActionResult Get(string id)
        => Ok(new { id, code = "МАТ-001", name = "Ткань Ситец", unit = "m", lastPrice = 180.0 });

    [HttpPost]
    public IActionResult Create([FromBody] object dto) => Created("", new { status = "created" });

    [HttpPut("{id}")]
    public IActionResult Update(string id, [FromBody] object dto) => Ok(new { status = "updated", id });

    [HttpGet("{id}/price-history")]
    public IActionResult PriceHistory(string id)
        => Ok(new[] { new { materialId = id, supplier = "ТексМаркет", price = 175.0, effectiveFrom = "2025-11-01" } });

    [HttpPost("{id}/prices")]
    public IActionResult AddPrice(string id, [FromBody] object dto) => Ok(new { status = "price-added", id });
}
