using System;
using System.Collections.Generic;
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

    // GET /api/finished-goods/receipt
    [HttpGet("receipt")]
    [SwaggerResponseExample(200, typeof(FinishedGoodsReceiptListResponseExample))]
    [ProducesResponseType(typeof(IEnumerable<FinishedGoodsReceiptListResponse>), StatusCodes.Status200OK)]
    public IActionResult GetReceipts()
        => Ok(new[]
        {
            new FinishedGoodsReceiptListResponse(
                ReceiptId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                ProductName: "Пижама женская",
                Quantity: 20,
                Date: new DateTime(2025, 11, 10),
                Warehouse: "Готовая продукция",
                UnitPrice: 444m,
                Sum: 8880m
            ),
            new FinishedGoodsReceiptListResponse(
                ReceiptId: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                ProductName: "Футболка детская",
                Quantity: 30,
                Date: new DateTime(2025, 11, 12),
                Warehouse: "Готовая продукция",
                UnitPrice: 170m,
                Sum: 5100m
            )
        });

    // GET /api/finished-goods/receipt/{id}
    [HttpGet("receipt/{id:guid}")]
    [SwaggerResponseExample(200, typeof(FinishedGoodsReceiptCardResponseExample))]
    [ProducesResponseType(typeof(FinishedGoodsReceiptCardResponse), StatusCodes.Status200OK)]
    public IActionResult GetReceiptById(Guid id)
        => Ok(new FinishedGoodsReceiptCardResponse(
            ReceiptId: id,
            DocumentNumber: "FG-2025-0001",
            ProductName: "Пижама женская",
            Quantity: 20,
            UnitPrice: 444m,
            Sum: 8880m,
            Date: new DateTime(2025, 11, 10),
            Warehouse: "Готовая продукция",
            Status: FinishedGoodsStatus.Accepted
        ));

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

