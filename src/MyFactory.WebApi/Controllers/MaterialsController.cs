using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyFactory.Application.Features.Materials.CreateMaterial;
using MyFactory.Application.Features.Materials.GetMaterialDetails;
using MyFactory.Application.Features.Materials.GetMaterials;
using MyFactory.Application.Features.Materials.RemoveMaterial;
using MyFactory.Application.Features.Materials.UpdateMaterial;
using MyFactory.WebApi.Contracts.Materials;
using MyFactory.WebApi.SwaggerExamples.Materials;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/materials")]
[Produces("application/json")]
public class MaterialsController : ControllerBase
{
    private readonly IMediator _mediator;

    public MaterialsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // -------------------------
    //  LIST
    // -------------------------
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<MaterialListItemResponse>), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(MaterialListResponseExample))]
    public async Task<IActionResult> GetList(
        [FromQuery] string? materialName,
        [FromQuery] string? materialType,
        [FromQuery] bool? isActive,
        [FromQuery] Guid? warehouseId)
    {
        var filter = new MaterialFilter
        {
            MaterialName = materialName,
            MaterialType = materialType,
            IsActive = isActive,
            WarehouseId = warehouseId
        };

        var dtos = await _mediator.Send(new GetMaterialsQuery(filter));

        var response = dtos
            .Select(x => new MaterialListItemResponse(
                x.Id,
                x.MaterialType,
                x.Name,
                x.TotalQty,
                x.UnitCode))
            .ToList();

        return Ok(response);
    }

    // -------------------------
    //  DETAILS
    // -------------------------
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(MaterialDetailsResponse), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(MaterialDetailsResponseExample))]
    public async Task<IActionResult> GetDetails(Guid id)
    {
        var dto = await _mediator.Send(new GetMaterialDetailsQuery(id));

        var response = new MaterialDetailsResponse(
            dto.Id,
            dto.Name,
            dto.MaterialType,
            dto.UnitCode,
            dto.Color,
            dto.TotalQty,
            dto.Warehouses
                .Select(w => new WarehouseQtyResponse(
                    w.WarehouseId,
                    w.WarehouseName,
                    w.Qty,
                    w.UnitCode))
                .ToList(),
            dto.PurchaseHistory
                .Select(ph => new MaterialPurchaseHistoryItemResponse(
                    ph.SupplierId,
                    ph.SupplierName,
                    ph.Qty,
                    ph.UnitPrice,
                    ph.PurchaseDate))
                .ToList());

        return Ok(response);
    }

    // -------------------------
    //  CREATE
    // -------------------------
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(CreateMaterialResponse), StatusCodes.Status201Created)]
    [SwaggerRequestExample(typeof(CreateMaterialRequest), typeof(CreateMaterialRequestExample))]
    [SwaggerResponseExample(201, typeof(CreateMaterialResponseExample))]
    public async Task<IActionResult> Create([FromBody] CreateMaterialRequest req)
    {
        var id = await _mediator.Send(new CreateMaterialCommand(req.Name, req.MaterialTypeId, req.UnitId, req.Color));
        var response = new CreateMaterialResponse(id);
        return CreatedAtAction(nameof(GetDetails), new { id }, response);
    }

    // -------------------------
    //  UPDATE
    // -------------------------
    [HttpPut("{id:guid}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [SwaggerRequestExample(typeof(UpdateMaterialRequest), typeof(UpdateMaterialRequestExample))]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateMaterialRequest req)
    {
        await _mediator.Send(new UpdateMaterialCommand
        {
            MaterialId = id,
            Name = req.Name,
            MaterialTypeId = req.MaterialTypeId,
            UnitId = req.UnitId,
            Color = req.Color
        });

        return NoContent();
    }

    // -------------------------
    //  REMOVE
    // -------------------------
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Remove(Guid id)
    {
        await _mediator.Send(new RemoveMaterialCommand(id));
        return NoContent();
    }
}
