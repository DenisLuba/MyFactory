using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyFactory.Application.Features.MaterialPurchaseOrders.AddItem;
using MyFactory.Application.Features.MaterialPurchaseOrders.Confirm;
using MyFactory.Application.Features.MaterialPurchaseOrders.Create;
using MyFactory.Application.Features.MaterialPurchaseOrders.Receive;
using MyFactory.Application.Features.MaterialPurchaseOrders.Cancel;
using MyFactory.Application.Features.MaterialPurchaseOrders.GetDetails;
using MyFactory.Application.Features.MaterialPurchaseOrders.GetSupplierPurchaseOrders;
using MyFactory.Application.Features.MaterialPurchaseOrders.RemoveItem;
using MyFactory.Application.Features.MaterialPurchaseOrders.UpdateItem;
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
            ReceivedByUserId = req.ReceivedByUserId,
            Items = (req.Items ?? Array.Empty<ReceiveMaterialPurchaseOrderItemRequest>())
                .Select(i => new ReceiveMaterialPurchaseOrderItem
                {
                    ItemId = i.ItemId,
                    Allocations = i.Allocations
                        .Select(a => new ReceiveMaterialPurchaseOrderAllocation
                        {
                            WarehouseId = a.WarehouseId,
                            Qty = a.Qty
                        }).ToList()
                }).ToList()
        });

        return NoContent();
    }

    // -------------------------
    //  SUPPLIER ORDERS BY SUPPLIER
    // -------------------------
    [HttpGet("supplier/{supplierId:guid}")]
    [ProducesResponseType(typeof(IReadOnlyList<SupplierPurchaseOrderListItemResponse>), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(SupplierPurchaseOrderListItemResponseExample))]
    public async Task<IActionResult> GetBySupplier(Guid supplierId)
    {
        var result = await _mediator.Send(new GetSupplierPurchaseOrdersQuery(supplierId));
        var response = result
            .Select(o => new SupplierPurchaseOrderListItemResponse(o.Id, o.OrderDate, o.Status, o.ItemsCount, o.TotalAmount))
            .ToList();
        return Ok(response);
    }

    // -------------------------
    //  DETAILS
    // -------------------------
    [HttpGet("{purchaseOrderId:guid}")]
    [ProducesResponseType(typeof(MaterialPurchaseOrderDetailsResponse), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(MaterialPurchaseOrderDetailsResponseExample))]
    public async Task<IActionResult> GetDetails(Guid purchaseOrderId)
    {
        var dto = await _mediator.Send(new GetMaterialPurchaseOrderDetailsQuery(purchaseOrderId));
        var response = new MaterialPurchaseOrderDetailsResponse(
            dto.Id,
            dto.SupplierId,
            dto.SupplierName,
            dto.OrderDate,
            dto.Status,
            dto.Items.Select(i => new MaterialPurchaseOrderItemResponse(
                i.Id,
                i.MaterialId,
                i.MaterialName,
                i.UnitCode,
                i.Qty,
                i.UnitPrice
            )).ToList()
        );
        return Ok(response);
    }

    // -------------------------
    //  UPDATE ITEM
    // -------------------------
    [HttpPut("items/{itemId:guid}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [SwaggerRequestExample(typeof(UpdateMaterialPurchaseOrderItemRequest), typeof(UpdateMaterialPurchaseOrderItemRequestExample))]
    public async Task<IActionResult> UpdateItem(Guid itemId, [FromBody] UpdateMaterialPurchaseOrderItemRequest req)
    {
        await _mediator.Send(new UpdateMaterialPurchaseOrderItemCommand
        {
            ItemId = itemId,
            Qty = req.Qty,
            UnitPrice = req.UnitPrice
        });

        return NoContent();
    }

    // -------------------------
    //  REMOVE ITEM
    // -------------------------
    [HttpDelete("items/{itemId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RemoveItem(Guid itemId)
    {
        await _mediator.Send(new RemoveMaterialPurchaseOrderItemCommand { ItemId = itemId });
        return NoContent();
    }

    // -------------------------
    //  CANCEL
    // -------------------------
    [HttpPost("{purchaseOrderId:guid}/cancel")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Cancel(Guid purchaseOrderId)
    {
        await _mediator.Send(new CancelMaterialPurchaseOrderCommand { PurchaseOrderId = purchaseOrderId });
        return NoContent();
    }
}
