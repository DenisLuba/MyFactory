using Microsoft.AspNetCore.Mvc;
using MyFactory.WebApi.Contracts.FinishedGoods;
using MyFactory.WebApi.SwaggerExamples.FinishedGoods;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/finished-goods")]
[Produces("application/json")]
public class FinishedGoodsController : ControllerBase
{
    private static readonly Guid Spec1 = Guid.Parse("11111111-1111-2222-3333-444444444444");
    private static readonly Guid Wh1 = Guid.Parse("22222222-2222-3333-4444-555555555555");

    private static readonly Guid ReceiptId = Guid.Parse("33333333-3333-4444-5555-666666666666");

    // POST /api/finished-goods/receipt
    [HttpPost("receipt")]
    [Consumes("application/json")]
    [SwaggerRequestExample(typeof(ReceiptFinishedGoodsRequest), typeof(ReceiptFinishedGoodsRequestExample))]
    [SwaggerResponseExample(201, typeof(ReceiptFinishedGoodsResponseExample))]
    [ProducesResponseType(typeof(ReceiptFinishedGoodsResponse), StatusCodes.Status201Created)]
    public IActionResult Receipt([FromBody] ReceiptFinishedGoodsRequest request)
        => Created(
            "",
            new ReceiptFinishedGoodsResponse(
                ReceiptId,
                FinishedGoodsStatus.Accepted
            )
        );

    // GET /api/finished-goods
    [HttpGet]
    [SwaggerResponseExample(200, typeof(FinishedGoodsInventoryResponseExample))]
    [ProducesResponseType(typeof(IEnumerable<FinishedGoodsInventoryResponse>), StatusCodes.Status200OK)]
    public IActionResult Inventory()
        => Ok(new[]
        {
            new FinishedGoodsInventoryResponse(
                SpecificationId: Spec1,
                WarehouseId: Wh1,
                Quantity: 20,
                UnitCost: 444.0m
            )
        });

    // POST /api/finished-goods/move
    [HttpPost("move")]
    [Consumes("application/json")]
    [SwaggerRequestExample(typeof(MoveFinishedGoodsRequest), typeof(MoveFinishedGoodsRequestExample))]
    [SwaggerResponseExample(200, typeof(MoveFinishedGoodsResponseExample))]
    [ProducesResponseType(typeof(MoveFinishedGoodsResponse), StatusCodes.Status200OK)]
    public IActionResult Move([FromBody] MoveFinishedGoodsRequest request)
        => Ok(new MoveFinishedGoodsResponse(FinishedGoodsStatus.Moved));
}

