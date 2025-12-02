using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Specifications;
using MyFactory.WebApi.SwaggerExamples.Specifications;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/specifications")]
[Produces("application/json")]
public class SpecificationsController : ControllerBase
{
    // -------------------------------------------------------------
    //  GET /api/specifications
    // -------------------------------------------------------------
    [HttpGet]
    [SwaggerResponseExample(200, typeof(SpecificationsListResponseExample))]
    [ProducesResponseType(typeof(IEnumerable<SpecificationsListResponse>), StatusCodes.Status200OK)]
    public IActionResult List()
        => Ok(new[]
        {
            new SpecificationsListResponse(
                Id: Guid.Parse("22222222-2222-2222-2222-222222222000"),
                Sku: "SP-001",
                Name: "Пижама женская",
                PlanPerHour: 2.5,
                Status: SpecificationsStatus.Updated,
                ImagesCount: 3
            ),
            new SpecificationsListResponse(
                Id: Guid.Parse("33333333-3333-3333-3333-333333333000"),
                Sku: "SP-002",
                Name: "Халат махровый",
                PlanPerHour: 1.3,
                Status: SpecificationsStatus.Created,
                ImagesCount: 1
            )
        });

    // -------------------------------------------------------------
    //  GET /api/specifications/{id}
    // -------------------------------------------------------------
    [HttpGet("{id}")]
    [SwaggerResponseExample(200, typeof(SpecificationsGetResponseExample))]
    [ProducesResponseType(typeof(SpecificationsGetResponse), StatusCodes.Status200OK)]
    public IActionResult Get(Guid id)
        => Ok(
            new SpecificationsGetResponse(
                Id: id,
                Sku: "SP-001",
                Name: "Пижама женская",
                PlanPerHour: 2.5,
                Description: "Классический комплект для сна",
                Status: SpecificationsStatus.Updated,
                ImagesCount: 3
            )
        );

    // -------------------------------------------------------------
    // GET /api/specifications/{id}/bom
    // -------------------------------------------------------------
    [HttpGet("{id}/bom")]
    [SwaggerResponseExample(200, typeof(SpecificationBomItemsResponseExample))]
    [ProducesResponseType(typeof(IEnumerable<SpecificationBomItemResponse>), StatusCodes.Status200OK)]
    public IActionResult GetBom(Guid id)
        => Ok(new[]
        {
            new SpecificationBomItemResponse(
                Id: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"),
                Material: "Ткань Ситец",
                Quantity: 1.8,
                Unit: "м",
                Price: 180,
                Cost: 324
            ),
            new SpecificationBomItemResponse(
                Id: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"),
                Material: "Фурнитура",
                Quantity: 1,
                Unit: "комплект",
                Price: 60,
                Cost: 60
            )
        });

    // -------------------------------------------------------------
    // GET /api/specifications/{id}/operations
    // -------------------------------------------------------------
    [HttpGet("{id}/operations")]
    [SwaggerResponseExample(200, typeof(SpecificationOperationItemsResponseExample))]
    [ProducesResponseType(typeof(IEnumerable<SpecificationOperationItemResponse>), StatusCodes.Status200OK)]
    public IActionResult GetOperations(Guid id)
        => Ok(new[]
        {
            new SpecificationOperationItemResponse(
                Id: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb1"),
                Operation: "Раскрой",
                Minutes: 8,
                Cost: 24
            ),
            new SpecificationOperationItemResponse(
                Id: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb2"),
                Operation: "Сборка",
                Minutes: 12,
                Cost: 44
            )
        });

    // -------------------------------------------------------------
    // POST /api/specifications
    // -------------------------------------------------------------
    [HttpPost]
    [Consumes("application/json")]
    [SwaggerRequestExample(typeof(SpecificationsCreateRequest), typeof(SpecificationsCreateRequestExample))]
    [SwaggerResponseExample(201, typeof(SpecificationsCreateResponseExample))]
    [ProducesResponseType(typeof(SpecificationsCreateResponse), StatusCodes.Status201Created)]
    public IActionResult Create([FromBody] SpecificationsCreateRequest dto)
        => Created("",
            new SpecificationsCreateResponse(
                Id: Guid.NewGuid(),
                Status: SpecificationsStatus.Created
            )
        );

    // -------------------------------------------------------------
    // PUT /api/specifications/{id}
    // -------------------------------------------------------------
    [HttpPut("{id}")]
    [Consumes("application/json")]
    [SwaggerRequestExample(typeof(SpecificationsUpdateRequest), typeof(SpecificationsUpdateRequestExample))]
    [SwaggerResponseExample(200, typeof(SpecificationsUpdateResponseExample))]
    [ProducesResponseType(typeof(SpecificationsUpdateResponse), StatusCodes.Status200OK)]
    public IActionResult Update(Guid id, [FromBody] SpecificationsUpdateRequest dto)
        => Ok(new SpecificationsUpdateResponse(id, SpecificationsStatus.Updated));

    // -------------------------------------------------------------
    // POST /api/specifications/{id}/bom
    // -------------------------------------------------------------
    [HttpPost("{id}/bom")]
    [Consumes("application/json")]
    [SwaggerRequestExample(typeof(SpecificationsAddBomRequest), typeof(SpecificationsAddBomRequestExample))]
    [SwaggerResponseExample(200, typeof(SpecificationsAddBomResponseExample))]
    [ProducesResponseType(typeof(SpecificationsAddBomResponse), StatusCodes.Status200OK)]
    public IActionResult AddBom(Guid id, [FromBody] SpecificationsAddBomRequest dto)
        => Ok(new SpecificationsAddBomResponse
        (
            SpecificationId: id,
            Item: new SpecificationBomItemResponse(
                Id: Guid.NewGuid(),
                Material: "Ткань Ситец",
                Quantity: dto.Qty,
                Unit: dto.Unit,
                Price: dto.Price,
                Cost: (decimal)dto.Qty * dto.Price
            ),
            Status: SpecificationsStatus.BomAdded
        ));

    // -------------------------------------------------------------
    // DELETE /api/specifications/{id}/bom/{bomId}
    // -------------------------------------------------------------
    [HttpDelete("{id}/bom/{bomId}")]
    [ProducesResponseType(typeof(SpecificationsDeleteBomItemResponse), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(SpecificationsDeleteBomItemResponseExample))]
    public IActionResult DeleteBomItem(Guid id, Guid bomId)
        => Ok(new SpecificationsDeleteBomItemResponse(id, bomId, SpecificationsStatus.BomDeleted));

    // -------------------------------------------------------------
    // POST /api/specifications/{id}/operations
    // -------------------------------------------------------------
    [HttpPost("{id}/operations")]
    [Consumes("application/json")]
    [SwaggerRequestExample(typeof(SpecificationsAddOperationRequest), typeof(SpecificationsAddOperationRequestExample))]
    [SwaggerResponseExample(200, typeof(SpecificationsAddOperationResponseExample))]
    [ProducesResponseType(typeof(SpecificationsAddOperationResponse), StatusCodes.Status200OK)]
    public IActionResult AddOperation(Guid id, [FromBody] SpecificationsAddOperationRequest dto)
        => Ok(new SpecificationsAddOperationResponse
        (
            SpecificationId: id,
            Item: new SpecificationOperationItemResponse(
                Id: Guid.NewGuid(),
                Operation: "Сборка",
                Minutes: dto.Minutes,
                Cost: dto.Cost
            ),
            Status: SpecificationsStatus.OperationAdded
        ));

    // -------------------------------------------------------------
    // POST /api/specifications/{id}/images
    // -------------------------------------------------------------
    [HttpPost("{id}/images")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(SpecificationsUploadImageResponse), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(SpecificationsUploadImageResponseExample))]
    public IActionResult UploadImage(Guid id)
        => Ok(new SpecificationsUploadImageResponse(id, SpecificationsStatus.ImageUploaded));

    // -------------------------------------------------------------
    // GET /api/specifications/{id}/cost
    // -------------------------------------------------------------
    [HttpGet("{id}/cost")]
    [SwaggerResponseExample(200, typeof(SpecificationsCostResponseExample))]
    [ProducesResponseType(typeof(SpecificationsCostResponse), StatusCodes.Status200OK)]
    public IActionResult Cost(Guid id, [FromQuery] DateTime? asOf = null)
        => Ok(new SpecificationsCostResponse(
            SpecificationId: id,
            AsOfDate: asOf ?? DateTime.UtcNow,
            MaterialsCost: 336,
            OperationsCost: 68,
            WorkshopExpenses: 40,
            TotalCost: 444
        ));
}
