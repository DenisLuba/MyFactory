using Microsoft.AspNetCore.Mvc;
using MyFactory.WebApi.Contracts.Inventory;
using MyFactory.WebApi.Contracts.Materials;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/inventory")]
public class InventoryController : ControllerBase
{
    [HttpGet]
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
    public IActionResult CreateReceipt([FromBody] CreateInventoryReceiptRequest request)
        => Created("", new CreateInventoryReceiptResponse(Guid.Parse("11111111-1111-1111-1111-111111111111"), StatusInventory.Posted));

    [HttpPost("adjust")]
    public IActionResult Adjust([FromBody] AdjustInventoryRequest request)
        => Ok(new AdjustInventoryResponse(StatusInventory.Adjusted));

    [HttpPost("transfer")]
    public IActionResult Transfer([FromBody] TransferInventoryRequest request)
        => Ok(new TransferInventoryResponse(StatusInventory.Transferred));
}