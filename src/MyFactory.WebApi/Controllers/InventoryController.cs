using Microsoft.AspNetCore.Mvc;
using MyFactory.WebApi.Contracts.Inventory;
using MyFactory.WebApi.Contracts.Materials;
using MyFactory.WebApi.SwaggerExamples.Inventory;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/inventory")]
[Produces("application/json")]
public class InventoryController : ControllerBase
{
    private static readonly Guid Mat1 = Guid.Parse("11111111-1111-1111-1111-111111111111");
    private static readonly Guid Wh1 = Guid.Parse("22222222-2222-2222-2222-222222222222");

    // GET /api/inventory
    [HttpGet]
    [SwaggerResponseExample(200, typeof(InventoryItemResponseExample))]
    [ProducesResponseType(typeof(IEnumerable<InventoryItemResponse>), StatusCodes.Status200OK)]
    public IActionResult GetAll([FromQuery] string? materialId = null)
        => Ok(new[]
        {
            new InventoryItemResponse(
                MaterialId: Mat1,
                MaterialName: "Ткань Ситец",
                WarehouseId: Wh1,
                Quantity: 120.0,
                Unit: Units.Meter,
                AvgPrice: 180.0m,
                ReservedQty: 0.0
            )
        });

    // GET /api/inventory/by-warehouse/{warehouseId}
    [HttpGet("by-warehouse/{warehouseId}")]
    [SwaggerResponseExample(200, typeof(InventoryItemResponseExample))]
    [ProducesResponseType(typeof(IEnumerable<InventoryItemResponse>), StatusCodes.Status200OK)]
    public IActionResult ByWarehouse(string warehouseId)
        => Ok(new[]
        {
            new InventoryItemResponse(
                MaterialId: Mat1,
                MaterialName: "Ткань Ситец",
                WarehouseId: Guid.Parse(warehouseId),
                Quantity: 120.0,
                Unit: Units.Meter,
                AvgPrice: 180.0m,
                ReservedQty: 0.0
            )
        });

    // POST /api/inventory/receipt
    [HttpPost("receipt")]
    [Consumes("application/json")]
    [SwaggerRequestExample(typeof(CreateInventoryReceiptRequest), typeof(CreateInventoryReceiptRequestExample))]
    [SwaggerResponseExample(201, typeof(CreateInventoryReceiptResponseExample))]
    [ProducesResponseType(typeof(CreateInventoryReceiptResponse), StatusCodes.Status201Created)]
    public IActionResult CreateReceipt([FromBody] CreateInventoryReceiptRequest request)
        => Created(
            "",
            new CreateInventoryReceiptResponse(
                ReceiptId: Guid.Parse("33333333-3333-3333-3333-333333333333"),
                Status: StatusInventory.Posted
            )
        );

    // POST /api/inventory/adjust
    [HttpPost("adjust")]
    [Consumes("application/json")]
    [SwaggerRequestExample(typeof(AdjustInventoryRequest), typeof(AdjustInventoryRequestExample))]
    [SwaggerResponseExample(200, typeof(AdjustInventoryResponseExample))]
    [ProducesResponseType(typeof(AdjustInventoryResponse), StatusCodes.Status200OK)]
    public IActionResult Adjust([FromBody] AdjustInventoryRequest request)
        => Ok(new AdjustInventoryResponse(StatusInventory.Adjusted));

    // POST /api/inventory/transfer
    [HttpPost("transfer")]
    [Consumes("application/json")]
    [SwaggerRequestExample(typeof(TransferInventoryRequest), typeof(TransferInventoryRequestExample))]
    [SwaggerResponseExample(200, typeof(TransferInventoryResponseExample))]
    [ProducesResponseType(typeof(TransferInventoryResponse), StatusCodes.Status200OK)]
    public IActionResult Transfer([FromBody] TransferInventoryRequest request)
        => Ok(new TransferInventoryResponse(StatusInventory.Transferred));
}
