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
                Id: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                Sku: "SP-001",
                Name: "Пижама женская",
                PlanPerHour: 2.5
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
                Bom:
                [
                    new BomItemResponse(
                        MaterialId: Guid.Parse("11111111-1111-1111-1111-111111111111"),
                        MaterialName: "Ткань Ситец",
                        Quantity: 1.8m,
                        Unit: "m",
                        Price: 180m
                    )
                ],
                Operations:
                [
                    new OperationItemResponse(
                        OperationId: Guid.Parse("22222222-2222-2222-2222-222222222222"),
                        Code: "CUT",
                        Name: "Раскрой",
                        Minutes: 6,
                        Cost: 15
                    )
                ]
            )
        );

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
        => Ok(new SpecificationsAddBomResponse(id, SpecificationsStatus.BomAdded));

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
        => Ok(new SpecificationsAddOperationResponse(id, SpecificationsStatus.OperationAdded));

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
