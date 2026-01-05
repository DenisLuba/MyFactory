using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyFactory.Application.Features.SalesOrders.AddSalesOrderItem;
using MyFactory.Application.Features.SalesOrders.CancelSalesOrder;
using MyFactory.Application.Features.SalesOrders.CompleteSalesOrder;
using MyFactory.Application.Features.SalesOrders.DeleteSalesOrder;
using MyFactory.Application.Features.SalesOrders.GetSalesOrderDetails;
using MyFactory.Application.Features.SalesOrders.GetSalesOrders;
using MyFactory.Application.Features.SalesOrders.GetSalesOrderShipments;
using MyFactory.Application.Features.SalesOrders.RemoveSalesOrderItem;
using MyFactory.Application.Features.SalesOrders.StartSalesOrder;
using MyFactory.Application.Features.SalesOrders.UpdateSalesOrder;
using MyFactory.Application.Features.SalesOrders.UpdateSalesOrderItem;
using MyFactory.WebApi.Contracts.SalesOrders;
using MyFactory.WebApi.SwaggerExamples.SalesOrders;
using Swashbuckle.AspNetCore.Filters;
using CreatSalesOrder = MyFactory.Application.Features.SalesOrders.CreatSalesOrder;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/sales-orders")]
[Produces("application/json")]
public class SalesOrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public SalesOrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // -------------------------
    //  LIST
    // -------------------------
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<SalesOrderListItemResponse>), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(SalesOrderListResponseExample))]
    public async Task<IActionResult> GetList()
    {
        var dtos = await _mediator.Send(new GetSalesOrdersQuery());
        var response = dtos
            .Select(x => new SalesOrderListItemResponse(
                x.Id,
                x.OrderNumber,
                x.CustomerName,
                x.OrderDate,
                x.Status))
            .ToList();
        return Ok(response);
    }

    // -------------------------
    //  DETAILS
    // -------------------------
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(SalesOrderDetailsResponse), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(SalesOrderDetailsResponseExample))]
    public async Task<IActionResult> GetDetails(Guid id)
    {
        var dto = await _mediator.Send(new GetSalesOrderDetailsQuery(id));
        var response = new SalesOrderDetailsResponse(
            dto.Id,
            dto.OrderNumber,
            dto.OrderDate,
            dto.Status,
            new CustomerDetailsResponse(
                dto.Customer.Id,
                dto.Customer.Name,
                dto.Customer.Phone,
                dto.Customer.Email,
                dto.Customer.Address),
            dto.Items.Select(i => new SalesOrderItemResponse(
                i.Id,
                i.ProductId,
                i.ProductName,
                i.QtyOrdered)).ToList());
        return Ok(response);
    }

    // -------------------------
    //  CREATE
    // -------------------------
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(CreateSalesOrderResponse), StatusCodes.Status201Created)]
    [SwaggerRequestExample(typeof(CreateSalesOrderRequest), typeof(CreateSalesOrderRequestExample))]
    [SwaggerResponseExample(201, typeof(CreateSalesOrderResponseExample))]
    public async Task<IActionResult> Create([FromBody] CreateSalesOrderRequest req)
    {
        var id = await _mediator.Send(new CreatSalesOrder.CreateSalesOrderCommand(req.CustomerId, req.OrderDate));
        return Created(string.Empty, new CreateSalesOrderResponse(id));
    }

    // -------------------------
    //  UPDATE
    // -------------------------
    [HttpPut("{id:guid}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerRequestExample(typeof(UpdateSalesOrderRequest), typeof(UpdateSalesOrderRequestExample))]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSalesOrderRequest req)
    {
        await _mediator.Send(new UpdateSalesOrderCommand(id, req.CustomerId, req.OrderDate));
        return Ok();
    }

    // -------------------------
    //  START
    // -------------------------
    [HttpPost("{id:guid}/start")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Start(Guid id)
    {
        await _mediator.Send(new StartSalesOrderCommand(id));
        return Ok();
    }

    // -------------------------
    //  COMPLETE
    // -------------------------
    [HttpPost("{id:guid}/complete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Complete(Guid id)
    {
        await _mediator.Send(new CompleteSalesOrderCommand(id));
        return Ok();
    }

    // -------------------------
    //  CANCEL
    // -------------------------
    [HttpPost("{id:guid}/cancel")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Cancel(Guid id)
    {
        await _mediator.Send(new CancelSalesOrderCommand(id));
        return Ok();
    }

    // -------------------------
    //  DELETE
    // -------------------------
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteSalesOrderCommand(id));
        return Ok();
    }

    // -------------------------
    //  ITEMS
    // -------------------------
    [HttpPost("{id:guid}/items")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(AddSalesOrderItemResponse), StatusCodes.Status201Created)]
    [SwaggerRequestExample(typeof(AddSalesOrderItemRequest), typeof(AddSalesOrderItemRequestExample))]
    [SwaggerResponseExample(201, typeof(AddSalesOrderItemResponseExample))]
    public async Task<IActionResult> AddItem(Guid id, [FromBody] AddSalesOrderItemRequest req)
    {
        var itemId = await _mediator.Send(new AddSalesOrderItemCommand(id, req.ProductId, req.Qty));
        return Created(string.Empty, new AddSalesOrderItemResponse(itemId));
    }

    [HttpPut("items/{itemId:guid}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerRequestExample(typeof(UpdateSalesOrderItemRequest), typeof(UpdateSalesOrderItemRequestExample))]
    public async Task<IActionResult> UpdateItem(Guid itemId, [FromBody] UpdateSalesOrderItemRequest req)
    {
        await _mediator.Send(new UpdateSalesOrderItemCommand(itemId, req.Qty));
        return Ok();
    }

    [HttpDelete("items/{itemId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> RemoveItem(Guid itemId)
    {
        await _mediator.Send(new RemoveSalesOrderItemCommand(itemId));
        return Ok();
    }

    // -------------------------
    //  SHIPMENTS
    // -------------------------
    [HttpGet("{id:guid}/shipments")]
    [ProducesResponseType(typeof(IReadOnlyList<SalesOrderShipmentResponse>), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(SalesOrderShipmentsResponseExample))]
    public async Task<IActionResult> GetShipments(Guid id)
    {
        var dtos = await _mediator.Send(new GetSalesOrderShipmentsQuery(id));
        var response = dtos
            .Select(x => new SalesOrderShipmentResponse(
                x.Id,
                x.ProductName,
                x.ProductionOrderNumber,
                x.WarehouseName,
                x.Qty,
                x.ShippedAt))
            .ToList();
        return Ok(response);
    }
}
