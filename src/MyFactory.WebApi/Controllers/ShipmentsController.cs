using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyFactory.Application.DTOs.Shipments;
using MyFactory.Application.Features.Shipments.CreateShipment;
using MyFactory.Application.Features.Shipments.DeleteShipment;
using MyFactory.Application.Features.Shipments.GetShipmentDetails;
using MyFactory.Application.Features.Shipments.GetShipments;
using MyFactory.Application.Features.Shipments.UpdateShipment;
using MyFactory.WebApi.Contracts.Shipments;
using MyFactory.WebApi.SwaggerExamples.Shipments;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/shipments")]
[Produces("application/json")]
public class ShipmentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ShipmentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // LIST
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ShipmentListItemResponse>), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(ShipmentListResponseExample))]
    public async Task<IActionResult> GetList(
        [FromQuery] Guid? salesOrderId,
        [FromQuery] Guid? customerId,
        [FromQuery] ShipmentStatusResponse? status,
        [FromQuery] DateTime? fromDate,
        [FromQuery] DateTime? toDate)
    {
        var query = new GetShipmentsQuery(
            SalesOrderId: salesOrderId,
            CustomerId: customerId,
            Status: ShipmentToApplicationStatus(status),
            FromDate: fromDate,
            ToDate: toDate);

        var dtos = await _mediator.Send(query);

        var response = dtos.Select(s => new ShipmentListItemResponse(
            Id: s.Id,
            SalesOrderId: s.SalesOrderId,
            CustomerId: s.CustomerId,
            ShipmentDate: s.ShipmentDate,
            Status: ApplicationToShipmentStatus(s.Status),
            ItemsCount: s.ItemsCount,
            TotalQty: s.TotalQty)).ToList();

        return Ok(response);
    }

    // DETAILS
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ShipmentDetailsResponse), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(ShipmentDetailsResponseExample))]
    public async Task<IActionResult> GetDetails(Guid id)
    {
        var dto = await _mediator.Send(new GetShipmentDetailsQuery(id));

        if (dto is null)
            return NotFound();

        var response = new ShipmentDetailsResponse(
            Id: dto.Id,
            SalesOrderId: dto.SalesOrderId,
            CustomerId: dto.CustomerId,
            ShipmentDate: dto.ShipmentDate,
            Status: ApplicationToShipmentStatus(dto.Status),
            Items: dto.Items.Select(i => new ShipmentDetailsItemResponse(
                ShipmentItemId: i.ShipmentItemId,
                SalesOrderItemId: i.SalesOrderItemId,
                ProductId: i.ProductId,
                WarehouseId: i.WarehouseId,
                Qty: i.Qty,
                UnitPrice: i.UnitPrice)).ToList());

        return Ok(response);
    }

    // CREATE
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(CreateShipmentResponse), StatusCodes.Status201Created)]
    [SwaggerRequestExample(typeof(CreateShipmentRequest), typeof(CreateShipmentRequestExample))]
    [SwaggerResponseExample(201, typeof(CreateShipmentResponseExample))]
    public async Task<IActionResult> Create([FromBody] CreateShipmentRequest req)
    {
        var command = new CreateShipmentCommand(
            SalesOrderId: req.SalesOrderId,
            CustomerId: req.CustomerId,
            ShipmentDate: req.ShipmentDate,
            CreatedBy: req.CreatedBy,
            Status: ShipmentToApplicationStatus(req.Status),
            Items: req.Items.Select(i => new CreateShipmentItemDto(
                i.SalesOrderItemId,
                i.ProductId,
                i.WarehouseId,
                i.Qty,
                i.UnitPrice)).ToList());

        var id = await _mediator.Send(command);

        var response = new CreateShipmentResponse(id);
        return CreatedAtAction(nameof(GetDetails), new { id }, response);
    }

    // UPDATE
    [HttpPut("{id:guid}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [SwaggerRequestExample(typeof(UpdateShipmentRequest), typeof(UpdateShipmentRequestExample))]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateShipmentRequest req)
    {
        var command = new UpdateShipmentCommand(
            ShipmentId: id,
            ShipmentDate: req.ShipmentDate,
            Status: ShipmentToApplicationStatus(req.Status),
            Items: req.Items?.Select(i => new MyFactory.Application.DTOs.Shipments.CreateShipmentItemDto(
                i.SalesOrderItemId,
                i.ProductId,
                i.WarehouseId,
                i.Qty,
                i.UnitPrice)).ToList());

        await _mediator.Send(command);
        return NoContent();
    }

    // DELETE
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteShipmentCommand(id));
        return NoContent();
    }

    private static ShipmentStatus? ShipmentToApplicationStatus(ShipmentStatusResponse? status) =>
        status switch
        {
            ShipmentStatusResponse.Draft => ShipmentStatus.Draft,
            ShipmentStatusResponse.Shipped => ShipmentStatus.Shipped,
            ShipmentStatusResponse.Cancelled => ShipmentStatus.Cancelled,
            ShipmentStatusResponse.Confirmed => ShipmentStatus.Confirmed,
            null => null,
            _ => throw new ArgumentOutOfRangeException(nameof(status), $"Unexpected shipment status value: {status}")
        };

    private static ShipmentStatusResponse? ApplicationToShipmentStatus(ShipmentStatus? status) =>
        status switch
        {
            ShipmentStatus.Draft => ShipmentStatusResponse.Draft,
            ShipmentStatus.Shipped => ShipmentStatusResponse.Shipped,
            ShipmentStatus.Cancelled => ShipmentStatusResponse.Cancelled,
            ShipmentStatus.Confirmed => ShipmentStatusResponse.Confirmed,
            null => null,
            _ => throw new ArgumentOutOfRangeException(nameof(status), $"Unexpected shipment status value: {status}")
        };
}
