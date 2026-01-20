using System.Net.Mime;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyFactory.Application.Features.Products.AddProductMaterial;
using MyFactory.Application.Features.Products.CreateProduct;
using MyFactory.Application.Features.Products.DeleteProductImage;
using MyFactory.Application.Features.Products.GetProductDetails;
using MyFactory.Application.Features.Products.GetProductImage;
using MyFactory.Application.Features.Products.GetProductImages;
using MyFactory.Application.Features.Products.GetProducts;
using MyFactory.Application.Features.Products.RemoveProductMaterial;
using MyFactory.Application.Features.Products.SetProductProductionCosts;
using MyFactory.Application.Features.Products.UpdateProduct;
using MyFactory.Application.Features.Products.UpdateProductMaterial;
using MyFactory.Application.Features.Products.UploadProductImage;
using MyFactory.WebApi.Contracts.Products;
using MyFactory.WebApi.SwaggerExamples.Products;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/products")]
[Produces("application/json")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // -------------------------
    //  LIST
    // -------------------------
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ProductListItemResponse>), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(ProductListResponseExample))]
    public async Task<IActionResult> GetList(
        [FromQuery] string? search,
        [FromQuery] string? sortBy,
        [FromQuery] bool sortDesc = false)
    {
        var dtos = await _mediator.Send(new GetProductsQuery(search, sortBy, sortDesc));
        var response = dtos
            .Select(x => new ProductListItemResponse(
                x.Id,
                x.Sku,
                x.Name,
                x.Status.ToContract(),
                x.Description,
                x.PlanPerHour,
                x.Version,
                x.CostPrice))
            .ToList();
        return Ok(response);
    }

    // -------------------------
    //  DETAILS
    // -------------------------
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ProductDetailsResponse), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(ProductDetailsResponseExample))]
    public async Task<IActionResult> GetDetails(Guid id)
    {
        var dto = await _mediator.Send(new GetProductDetailsQuery(id));
        var response = new ProductDetailsResponse(
            dto.Id,
            dto.Sku,
            dto.Name,
            dto.PlanPerHour,
            dto.Description,
            dto.Version,
            dto.Status.ToContract(),
            dto.MaterialsCost,
            dto.ProductionCost,
            dto.TotalCost,
            [.. dto.Bom.Select(b => new ProductBomItemResponse(
                b.MaterialId,
                b.MaterialName,
                b.QtyPerUnit,
                b.LastUnitPrice,
                b.TotalCost))],
            [.. dto.ProductionCosts.Select(pc => new ProductDepartmentCostResponse(
                pc.DepartmentId,
                pc.DepartmentName,
                pc.CutCost,
                pc.SewingCost,
                pc.PackCost,
                pc.Expenses,
                pc.Total))],
            [.. dto.Availability.Select(a => new ProductAvailabilityResponse(
                a.WarehouseId,
                a.WarehouseName,
                a.AvailableQty))]);
        return Ok(response);
    }

    // -------------------------
    //  CREATE
    // -------------------------
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(CreateProductResponse), StatusCodes.Status201Created)]
    [SwaggerRequestExample(typeof(CreateProductRequest), typeof(CreateProductRequestExample))]
    [SwaggerResponseExample(201, typeof(CreateProductResponseExample))]
    public async Task<IActionResult> Create([FromBody] CreateProductRequest req)
    {
        var id = await _mediator.Send(new CreateProductCommand(
            req.Name,
            req.Status.ToDomain(),
            req.PlanPerHour,
            req.Description,
            req.Version));
        return Created(string.Empty, new CreateProductResponse(id));
    }

    // -------------------------
    //  UPDATE
    // -------------------------
    [HttpPut("{id:guid}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [SwaggerRequestExample(typeof(UpdateProductRequest), typeof(UpdateProductRequestExample))]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductRequest req)
    {
        await _mediator.Send(new UpdateProductCommand(
            id,
            req.Name,
            req.PlanPerHour,
            req.Status.ToDomain(),
            req.Description,
            req.Version));
        return NoContent();
    }

    // -------------------------
    //  MATERIALS
    // -------------------------
    [HttpPost("{id:guid}/materials")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(AddProductMaterialResponse), StatusCodes.Status201Created)]
    [SwaggerRequestExample(typeof(AddProductMaterialRequest), typeof(AddProductMaterialRequestExample))]
    [SwaggerResponseExample(201, typeof(AddProductMaterialResponseExample))]
    public async Task<IActionResult> AddMaterial(Guid id, [FromBody] AddProductMaterialRequest req)
    {
        var materialId = await _mediator.Send(new AddProductMaterialCommand
        {
            ProductId = id,
            MaterialId = req.MaterialId,
            QtyPerUnit = req.QtyPerUnit
        });
        return Created(string.Empty, new AddProductMaterialResponse(materialId));
    }

    [HttpPut("materials/{productMaterialId:guid}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [SwaggerRequestExample(typeof(UpdateProductMaterialRequest), typeof(UpdateProductMaterialRequestExample))]
    public async Task<IActionResult> UpdateMaterial(Guid productMaterialId, [FromBody] UpdateProductMaterialRequest req)
    {
        await _mediator.Send(new UpdateProductMaterialCommand(productMaterialId, req.QtyPerUnit));
        return NoContent();
    }

    [HttpDelete("{productId:guid}/materials/{materialId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RemoveMaterial(Guid productId, Guid materialId)
    {
        await _mediator.Send(new RemoveProductMaterialCommand(productId, materialId));
        return NoContent();
    }

    // -------------------------
    //  PRODUCTION COSTS
    // -------------------------
    [HttpPost("{id:guid}/production-costs")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [SwaggerRequestExample(typeof(SetProductProductionCostsRequest), typeof(SetProductProductionCostsRequestExample))]
    public async Task<IActionResult> SetProductionCosts(Guid id, [FromBody] SetProductProductionCostsRequest req)
    {
        await _mediator.Send(new SetProductProductionCostsCommand(
            id,
            req.Costs.Select(c => new MyFactory.Application.DTOs.Products.ProductDepartmentCostDto
            {
                DepartmentId = c.DepartmentId,
                DepartmentName = string.Empty,
                CutCost = c.CutCost,
                SewingCost = c.SewingCost,
                PackCost = c.PackCost,
                Expenses = c.Expenses
            }).ToList()));
        return NoContent();
    }

    // -------------------------
    //  IMAGES
    // -------------------------
    [HttpGet("{id:guid}/images")]
    [ProducesResponseType(typeof(IReadOnlyList<ProductImageFileResponse>), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(ProductImageFilesResponseExample))]
    public async Task<IActionResult> GetImages(Guid id)
    {
        var dtos = await _mediator.Send(new GetProductImagesQuery(id));
        var response = dtos
            .Select(x => new ProductImageFileResponse(
                x.Id,
                x.ProductId,
                x.FileName,
                x.ContentType,
                x.Content))
            .ToList();
        return Ok(response);
    }

    [HttpGet("images/{imageId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(ProductImageDownloadExample))]
    public async Task<IActionResult> GetImage(Guid imageId)
    {
        var dto = await _mediator.Send(new GetProductImageQuery(imageId));
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

        var imageId = await _mediator.Send(new UploadProductImageCommand(
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
        await _mediator.Send(new DeleteProductImageCommand(imageId));
        return NoContent();
    }
}
