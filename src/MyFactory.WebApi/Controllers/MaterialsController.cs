using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyFactory.Application.Features.Materials.CreateMaterial;
using MyFactory.Application.Features.Materials.DeleteMaterialImage;
using MyFactory.Application.Features.Materials.GetMaterialDetails;
using MyFactory.Application.Features.Materials.GetMaterialImage;
using MyFactory.Application.Features.Materials.GetMaterialImages;
using MyFactory.Application.Features.Materials.GetMaterials;
using MyFactory.Application.Features.Materials.RemoveMaterial;
using MyFactory.Application.Features.Materials.UpdateMaterial;
using MyFactory.Application.Features.Materials.UploadMaterialImage;
using MyFactory.WebApi.Contracts.Common;
using MyFactory.WebApi.Contracts.Materials;
using MyFactory.WebApi.SwaggerExamples.Materials;
using Swashbuckle.AspNetCore.Filters;
using System.Net.Mime;

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
    [ProducesResponseType(typeof(ListResponse<MaterialListItemResponse>), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(MaterialListResponseExample))]
    public async Task<IActionResult> GetList(
        [FromQuery] string? searchName,
        [FromQuery] string? searchType,
        [FromQuery] string? sortBy,
        [FromQuery] bool sortDesc = false,
        [FromQuery] int skip = 0,
        [FromQuery] int take = 30,
        [FromQuery] bool isActive = true,
        [FromQuery] Guid? warehouseId = null)
    {
        var dto = await _mediator.Send(new GetMaterialsQuery(
            SearchName: searchName,
            SearchType: searchType,
            SortBy: sortBy,
            SortDesc: sortDesc,
            Skip: skip,
            Take: take,
            IsActive: isActive,
            WarehouseId: warehouseId));

        var response = new ListResponse<MaterialListItemResponse>(
            Items: [.. dto.Items
                .Select(m => new MaterialListItemResponse(
                    m.Id,
                    m.MaterialType,
                    m.Name,
                    m.TotalQty,
                    m.UnitCode))],
            TotalCount: dto.TotalCount,
            Take: dto.Take,
            Skip: dto.Skip,
            HasMore: dto.HasMore
        );
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
            dto.Description,
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
        var id = await _mediator.Send(new CreateMaterialCommand(req.Name, req.MaterialTypeId, req.UnitId, req.Color, req.Description));
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
            Color = req.Color,
            Description = req.Description
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


    // -------------------------
    //  IMAGES
    // -------------------------
    [HttpGet("{id:guid}/images")]
    [ProducesResponseType(typeof(IReadOnlyList<MaterialImageFileResponse>), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(MaterialImageFilesResponseExample))]
    public async Task<IActionResult> GetImages(Guid id)
    {
        var dtos = await _mediator.Send(new GetMaterialImagesQuery(id));
        var response = dtos
            .Select(x => new MaterialImageFileResponse(
                x.Id,
                x.MaterialId,
                x.FileName,
                x.ContentType,
                x.Content))
            .ToList();
        return Ok(response);
    }

    [HttpGet("images/{imageId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(MaterialImageDownloadExample))]
    public async Task<IActionResult> GetImage(Guid imageId)
    {
        var dto = await _mediator.Send(new GetMaterialImageQuery(imageId));
        if (dto is null || dto.Content is null)
            return NotFound();

        return File(dto.Content, dto.ContentType ?? MediaTypeNames.Application.Octet, dto.FileName);
    }

    [HttpPost("{id:guid}/images")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    public async Task<IActionResult> UploadImage(Guid id, IFormFile file)
    {
        if (file is null)
            return BadRequest("File is required.");

        await using var ms = new MemoryStream();
        await file.CopyToAsync(ms);
        var content = ms.ToArray();

        var imageId = await _mediator.Send(new UploadMaterialImageCommand(
            id,
            file.FileName,
            file.ContentType,
            content));

        return Created(string.Empty, imageId);
    }

    [HttpDelete("images/{imageId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteImage(Guid imageId)
    {
        await _mediator.Send(new DeleteMaterialImageCommand(imageId));
        return NoContent();
    }
}