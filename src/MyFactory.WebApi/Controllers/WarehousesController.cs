using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyFactory.Application.Features.Warehouses.AddMaterialToWarehouse;
using MyFactory.Application.Features.Warehouses.GetWarehouseInfo;
using MyFactory.Application.Features.Warehouses.GetWarehouses;
using MyFactory.Application.Features.Warehouses.GetWarehouseStock;
using MyFactory.Application.Features.Warehouses.RemoveMaterialFromWarehouse;
using MyFactory.Application.Features.Warehouses.TransferMaterials;
using MyFactory.Application.Features.Warehouses.TransferProducts;
using MyFactory.Application.Features.Warehouses.UpdateWarehouseMaterialQty;
using MyFactory.WebApi.Contracts.Warehouses;
using MyFactory.WebApi.SwaggerExamples.Warehouses;
using Swashbuckle.AspNetCore.Filters;
using CreateUpdateDeactivateWarehouse = MyFactory.Application.Features.Warehouses.GetWarehouses;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/warehouses")]
[Produces("application/json")]
public class WarehousesController : ControllerBase
{
    private readonly IMediator _mediator;

    public WarehousesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // -------------------------
    //  LIST
    // -------------------------
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<WarehouseListItemResponse>), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(WarehouseListResponseExample))]
    public async Task<IActionResult> GetList([FromQuery] bool includeInactive = false)
    {
        var dtos = await _mediator.Send(new GetWarehousesQuery(includeInactive));
        var response = dtos
            .Select(w => new WarehouseListItemResponse(w.Id, w.Name, w.Type, w.IsActive))
            .ToList();
        return Ok(response);
    }

    // -------------------------
    //  DETAILS
    // -------------------------
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(WarehouseInfoResponse), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(WarehouseInfoResponseExample))]
    public async Task<IActionResult> GetInfo(Guid id)
    {
        var dto = await _mediator.Send(new GetWarehouseInfoQuery(id));
        var response = new WarehouseInfoResponse(dto.Id, dto.Name, dto.Type);
        return Ok(response);
    }

    // -------------------------
    //  STOCK
    // -------------------------
    [HttpGet("{id:guid}/stock")]
    [ProducesResponseType(typeof(IReadOnlyList<WarehouseStockItemResponse>), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(WarehouseStockResponseExample))]
    public async Task<IActionResult> GetStock(Guid id)
    {
        var dtos = await _mediator.Send(new GetWarehouseStockQuery(id));
        var response = dtos
            .Select(x => new WarehouseStockItemResponse(x.ItemId, x.Name, x.Qty, x.UnitCode))
            .ToList();
        return Ok(response);
    }

    // -------------------------
    //  CREATE
    // -------------------------
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(CreateWarehouseResponse), StatusCodes.Status201Created)]
    [SwaggerRequestExample(typeof(CreateWarehouseRequest), typeof(CreateWarehouseRequestExample))]
    [SwaggerResponseExample(201, typeof(CreateWarehouseResponseExample))]
    public async Task<IActionResult> Create([FromBody] CreateWarehouseRequest req)
    {
        var id = await _mediator.Send(new CreateWarehouseCommand(req.Name, req.Type));
        return Created(string.Empty, new CreateWarehouseResponse(id));
    }

    // -------------------------
    //  UPDATE
    // -------------------------
    [HttpPut("{id:guid}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerRequestExample(typeof(UpdateWarehouseRequest), typeof(UpdateWarehouseRequestExample))]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateWarehouseRequest req)
    {
        await _mediator.Send(new UpdateWarehouseCommand(id, req.Name, req.Type));
        return Ok();
    }

    // -------------------------
    //  DEACTIVATE
    // -------------------------
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Deactivate(Guid id)
    {
        await _mediator.Send(new DeactivateWarehouseCommand(id));
        return Ok();
    }

    // -------------------------
    //  MATERIALS
    // -------------------------
    [HttpPost("{id:guid}/materials")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerRequestExample(typeof(AddMaterialToWarehouseRequest), typeof(AddMaterialToWarehouseRequestExample))]
    public async Task<IActionResult> AddMaterial(Guid id, [FromBody] AddMaterialToWarehouseRequest req)
    {
        await _mediator.Send(new AddMaterialToWarehouseCommand(id, req.MaterialId, req.Qty));
        return Ok();
    }

    [HttpDelete("{id:guid}/materials/{materialId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> RemoveMaterial(Guid id, Guid materialId)
    {
        await _mediator.Send(new RemoveMaterialFromWarehouseCommand(id, materialId));
        return Ok();
    }

    [HttpPut("{id:guid}/materials/{materialId:guid}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerRequestExample(typeof(UpdateWarehouseMaterialQtyRequest), typeof(UpdateWarehouseMaterialQtyRequestExample))]
    public async Task<IActionResult> UpdateMaterialQty(Guid id, Guid materialId, [FromBody] UpdateWarehouseMaterialQtyRequest req)
    {
        await _mediator.Send(new UpdateWarehouseMaterialQtyCommand(id, materialId, req.Qty));
        return Ok();
    }

    // -------------------------
    //  TRANSFER MATERIALS
    // -------------------------
    [HttpPost("materials/transfer")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerRequestExample(typeof(TransferMaterialsRequest), typeof(TransferMaterialsRequestExample))]
    public async Task<IActionResult> TransferMaterials([FromBody] TransferMaterialsRequest req)
    {
        await _mediator.Send(new TransferMaterialsCommand(
            req.FromWarehouseId,
            req.ToWarehouseId,
            req.Items.Select(i => new MyFactory.Application.DTOs.Warehouses.TransferMaterialItemDto(i.MaterialId, i.Qty)).ToList()));
        return Ok();
    }

    // -------------------------
    //  TRANSFER PRODUCTS
    // -------------------------
    [HttpPost("products/transfer")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerRequestExample(typeof(TransferProductsRequest), typeof(TransferProductsRequestExample))]
    public async Task<IActionResult> TransferProducts([FromBody] TransferProductsRequest req)
    {
        await _mediator.Send(new TransferProductsCommand(
            req.FromWarehouseId,
            req.ToWarehouseId,
            req.Items.Select(i => new MyFactory.Application.DTOs.Warehouses.TransferProductItemDto(i.ProductId, i.Qty)).ToList()));
        return Ok();
    }
}
