using Microsoft.AspNetCore.Mvc;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/specifications")]
public class SpecificationsController : ControllerBase
{
    [HttpGet]
    public IActionResult List()
        => Ok(new[] { new { id = "sp-001", sku = "SP-001", name = "Пижама женская", planPerHour = 2.5 } });

    [HttpGet("{id}")]
    public IActionResult Get(string id)
        => Ok(new
        {
            id,
            sku = "SP-001",
            name = "Пижама женская",
            planPerHour = 2.5,
            bom = new[] { new { material = "Ткань Ситец", qty = 1.8, unit = "m", price = 180.0 } },
            operations = new[] { new { code = "CUT", name = "Раскрой", minutes = 6, cost = 15.0 } }
        });

    [HttpPost]
    public IActionResult Create([FromBody] object dto) => Created("", new { status = "created" });

    [HttpPut("{id}")]
    public IActionResult Update(string id, [FromBody] object dto) => Ok(new { status = "updated", id });

    [HttpPost("{id}/bom")]
    public IActionResult AddBom(string id, [FromBody] object dto) => Ok(new { status = "bom-added", id });

    [HttpPost("{id}/operations")]
    public IActionResult AddOperation(string id, [FromBody] object dto) => Ok(new { status = "operation-added", id });

    [HttpPost("{id}/images")]
    public IActionResult UploadImage(string id) => Ok(new { status = "image-uploaded", id });

    [HttpGet("{id}/cost")]
    public IActionResult Cost(string id, [FromQuery] DateTime? asOf = null)
        => Ok(new
        {
            specificationId = id,
            asOfDate = asOf?.ToString("yyyy-MM-dd") ?? DateTime.UtcNow.ToString("yyyy-MM-dd"),
            materialsCost = 336.0,
            operationsCost = 68.0,
            workshopExpenses = 40.0,
            totalCost = 444.0
        });

    [HttpDelete("{id}/bom/{bomId}")]
    public IActionResult DeleteBomItem(string id, string bomId)
        => Ok(new
        {
            status = "bom-deleted",
            specificationId = id,
            bomId
        });
}
