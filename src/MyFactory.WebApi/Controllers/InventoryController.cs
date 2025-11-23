using Microsoft.AspNetCore.Mvc;
using MyFactory.WebApi.Contracts.Finance;
using MyFactory.WebApi.Contracts.Inventory;
using MyFactory.WebApi.Contracts.Materials;
using MyFactory.WebApi.SwaggerExamples.Inventory;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/inventory")]
public class InventoryController : ControllerBase
{
    [HttpGet]
    [Produces("application/json")]
    [SwaggerResponseExample(200, typeof(InventoryItemResponseExample))]
    [ProducesResponseType(typeof(IEnumerable<InventoryItemResponse>), StatusCodes.Status200OK)]
    public IActionResult GetAll([FromQuery] string? materialId = null)
        => Ok(new[]
        {
            new InventoryItemResponse(
                Guid.Parse("mat-001"),
                "Ткань Ситец",
                Guid.Parse("st-001"),
                120.0,
                Units.Meter,
                180.0m,
                0.0
            )
        });

    [HttpGet("by-warehouse/{warehouseId}")]
    [Produces("application/json")]
    [SwaggerResponseExample(200, typeof(InventoryItemResponseExample))]
    [ProducesResponseType(typeof(IEnumerable<InventoryItemResponse>), StatusCodes.Status200OK)]
    public IActionResult ByWarehouse(string warehouseId)
        => Ok(new[]
        {
            new InventoryItemResponse(
                Guid.Parse("mat-001"),
                "Ткань Ситец",
                Guid.Parse("st-001"),
                120.0,
                Units.Meter,
                180.0m,
                0.0
            )
        });

    [HttpPost("receipt")]
    [Consumes("application/json")]
    [Produces("application/json")]
    [SwaggerRequestExample(typeof(CreateInventoryReceiptRequest), typeof(CreateInventoryReceiptRequestExample))]
    [SwaggerResponseExample(201, typeof(CreateInventoryReceiptResponseExample))]
    [ProducesResponseType(typeof(CreateInventoryReceiptResponse), StatusCodes.Status201Created)]
    public IActionResult CreateReceipt([FromBody] CreateInventoryReceiptRequest request)
        => Created("", new CreateInventoryReceiptResponse(Guid.Parse("11111111-1111-1111-1111-111111111111"), StatusInventory.Posted));

    [HttpPost("adjust")]
    [SwaggerRequestExample(typeof(AdjustInventoryRequest), typeof(AdjustInventoryRequestExample))]
    [SwaggerResponseExample(200, typeof(AdjustInventoryResponseExample))]
    public IActionResult Adjust([FromBody] AdjustInventoryRequest request)
        => Ok(new AdjustInventoryResponse(StatusInventory.Adjusted));

    [HttpPost("transfer")]
    [SwaggerRequestExample(typeof(TransferInventoryRequest), typeof(TransferInventoryRequestExample))]
    [SwaggerResponseExample(200, typeof(TransferInventoryResponseExample))]
    public IActionResult Transfer([FromBody] TransferInventoryRequest request)
        => Ok(new TransferInventoryResponse(StatusInventory.Transferred));
}