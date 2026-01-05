using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyFactory.Application.Features.MaterialPurchaseOrders.AddItem;
using MyFactory.Application.Features.MaterialPurchaseOrders.Confirm;
using MyFactory.Application.Features.MaterialPurchaseOrders.Create;
using MyFactory.Application.Features.MaterialPurchaseOrders.Receive;
using MyFactory.WebApi.Contracts.MaterialPurchaseOrders;
using MyFactory.WebApi.SwaggerExamples.MaterialPurchaseOrders;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/material-purchase-orders")]
[Produces("application/json")]
public class MaterialPurchaseOrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public MaterialPurchaseOrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // -------------------------
    //  CREATE ORDER
    // -------------------------
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(CreateMaterialPurchaseOrderResponse), StatusCodes.Status201Created)]
    [SwaggerRequestExample(typeof(CreateMaterialPurchaseOrderRequest), typeof(CreateMaterialPurchaseOrderRequestExample))]
    [SwaggerResponseExample(201, typeof(CreateMaterialPurchaseOrderResponseExample))]
    public async Task<IActionResult> Create([FromBody] CreateMaterialPurchaseOrderRequest req)
    {
        var id = await _mediator.Send(new CreateMaterialPurchaseOrderCommand
        {
            SupplierId = req.SupplierId,
            OrderDate = req.OrderDate
        });

        return Created(string.Empty, new CreateMaterialPurchaseOrderResponse(id));
    }

    // -------------------------
    //  ADD ITEM
    // -------------------------
    [HttpPost("{purchaseOrderId:guid}/items")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [SwaggerRequestExample(typeof(AddMaterialPurchaseOrderItemRequest), typeof(AddMaterialPurchaseOrderItemRequestExample))]
    public async Task<IActionResult> AddItem(Guid purchaseOrderId, [FromBody] AddMaterialPurchaseOrderItemRequest req)
    {
        await _mediator.Send(new AddMaterialPurchaseOrderItemCommand
        {
            PurchaseOrderId = purchaseOrderId,
            MaterialId = req.MaterialId,
            Qty = req.Qty,
            UnitPrice = req.UnitPrice
        });

        return NoContent();
    }

    // -------------------------
    //  CONFIRM
    // -------------------------
    [HttpPost("{purchaseOrderId:guid}/confirm")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Confirm(Guid purchaseOrderId)
    {
        await _mediator.Send(new ConfirmMaterialPurchaseOrderCommand(purchaseOrderId));
        return NoContent();
    }

    // -------------------------
    //  RECEIVE
    // -------------------------
    [HttpPost("{purchaseOrderId:guid}/receive")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [SwaggerRequestExample(typeof(ReceiveMaterialPurchaseOrderRequest), typeof(ReceiveMaterialPurchaseOrderRequestExample))]
    public async Task<IActionResult> Receive(Guid purchaseOrderId, [FromBody] ReceiveMaterialPurchaseOrderRequest req)
    {
        await _mediator.Send(new ReceiveMaterialPurchaseOrderCommand
        {
            PurchaseOrderId = purchaseOrderId,
            WarehouseId = req.WarehouseId,
            ReceivedByUserId = req.ReceivedByUserId
        });

        return NoContent();
    }
}
