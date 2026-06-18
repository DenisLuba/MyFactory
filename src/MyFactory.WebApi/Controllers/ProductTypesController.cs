using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyFactory.Application.Features.ProductTypes.CreateProductType;
using MyFactory.Application.Features.ProductTypes.DeleteProductType;
using MyFactory.Application.Features.ProductTypes.GetProductTypeById;
using MyFactory.Application.Features.ProductTypes.GetProductTypeByProductId;
using MyFactory.Application.Features.ProductTypes.GetProductTypes;
using MyFactory.Application.Features.ProductTypes.UpdateProductType;
using MyFactory.WebApi.Contracts.ProductTypes;
using MyFactory.WebApi.SwaggerExamples.ProductTypes;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/producttypes")]
[Produces("application/json")]
public class ProductTypesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductTypesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // -------------------------
    //  LIST
    // -------------------------
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ProductTypeResponse>), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(ProductTypeListResponseExample))]
    public async Task<IActionResult> GetList([FromQuery] string? search)
    {
        var dtos = await _mediator.Send(new GetProductTypesQuery(search));
        var response = dtos
            .Select(x => new ProductTypeResponse(x.Id, x.Type, x.Description))
            .ToList();
        return Ok(response);
    }

    // -------------------------
    //  DETAILS
    // -------------------------
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ProductTypeResponse), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(ProductTypeDetailsResponseExample))]
    public async Task<IActionResult> GetDetails(Guid id)
    {
        var dto = await _mediator.Send(new GetProductTypeByIdQuery(id));
        if (dto is null)
            return NotFound();

        var response = new ProductTypeResponse(dto.Id, dto.Type, dto.Description);
        return Ok(response);
    }

    // -------------------------
    //  GET BY PRODUCT
    // -------------------------
    [HttpGet("by-product/{productId:guid}")]
    [ProducesResponseType(typeof(ProductTypeResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByProduct(Guid productId)
    {
        var dto = await _mediator.Send(new GetProductTypeByProductIdQuery(productId));
        if (dto is null)
            return NotFound();

        var response = new ProductTypeResponse(dto.Id, dto.Type, dto.Description);
        return Ok(response);
    }

    // -------------------------
    //  CREATE
    // -------------------------
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(CreateProductTypeResponse), StatusCodes.Status201Created)]
    [SwaggerRequestExample(typeof(CreateProductTypeRequest), typeof(CreateProductTypeRequestExample))]
    [SwaggerResponseExample(201, typeof(CreateProductTypeResponseExample))]
    public async Task<IActionResult> Create([FromBody] CreateProductTypeRequest req)
    {
        var id = await _mediator.Send(new CreateProductTypeCommand(req.Type, req.Description));
        return Created(string.Empty, new CreateProductTypeResponse(id));
    }

    // -------------------------
    //  UPDATE
    // -------------------------
    [HttpPut("{id:guid}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [SwaggerRequestExample(typeof(UpdateProductTypeRequest), typeof(UpdateProductTypeRequestExample))]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductTypeRequest req)
    {
        await _mediator.Send(new UpdateProductTypeCommand(id, req.Type, req.Description));
        return NoContent();
    }

    // -------------------------
    //  DELETE
    // -------------------------
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteProductTypeCommand(id));
        return NoContent();
    }
}
