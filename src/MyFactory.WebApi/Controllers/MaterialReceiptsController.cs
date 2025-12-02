using Microsoft.AspNetCore.Mvc;
using System.Linq;
using MyFactory.WebApi.Contracts.WarehouseMaterials;
using MyFactory.WebApi.SwaggerExamples.WarehouseMaterials;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/material-receipts")]
[Produces("application/json")]
public class MaterialReceiptsController : ControllerBase
{
    private static readonly Guid ReceiptId = Guid.Parse("aaaaaaaa-0000-0000-0000-000000000001");
    private static readonly Guid Line1Id = Guid.Parse("bbbbbbbb-0000-0000-0000-000000000001");
    private static readonly Guid Line2Id = Guid.Parse("bbbbbbbb-0000-0000-0000-000000000002");

    private static readonly List<MaterialReceiptLineResponse> Lines =
    [
        new MaterialReceiptLineResponse(
            Id: Line1Id,
            MaterialId: Guid.Parse("11111111-1111-1111-1111-111111111111"),
            MaterialName: "Ткань Ситец",
            Quantity: 50,
            Unit: "м",
            Price: 150m,
            Amount: 7500m
        ),
        new MaterialReceiptLineResponse(
            Id: Line2Id,
            MaterialId: Guid.Parse("22222222-2222-2222-2222-222222222222"),
            MaterialName: "Нитки хлопковые",
            Quantity: 200,
            Unit: "шт",
            Price: 2.5m,
            Amount: 500m
        )
    ];

    // GET /api/material-receipts
    [HttpGet]
    [SwaggerResponseExample(200, typeof(MaterialReceiptListResponseExample))]
    [ProducesResponseType(typeof(IEnumerable<MaterialReceiptListResponse>), StatusCodes.Status200OK)]
    public IActionResult List()
        => Ok(new MaterialReceiptListResponse[]
        {
            new(
                Id: ReceiptId,
                DocumentNumber: "RC-001",
                DocumentDate: new DateTime(2025, 11, 1),
                SupplierName: "ТексМаркет",
                WarehouseName: "Основной склад",
                TotalAmount: Lines.Sum(l => l.Amount),
                Status: MaterialReceiptStatus.Draft
            )
        });

    // GET /api/material-receipts/{id}
    [HttpGet("{id:guid}")]
    [SwaggerResponseExample(200, typeof(MaterialReceiptCardResponseExample))]
    [ProducesResponseType(typeof(MaterialReceiptCardResponse), StatusCodes.Status200OK)]
    public IActionResult Get(Guid id)
        => Ok(new MaterialReceiptCardResponse(
            Id: id,
            DocumentNumber: "RC-001",
            DocumentDate: new DateTime(2025, 11, 1),
            SupplierName: "ТексМаркет",
            WarehouseName: "Основной склад",
            TotalAmount: Lines.Sum(l => l.Amount),
            Status: MaterialReceiptStatus.Draft,
            Comment: "Черновик поступления"
        ));

    // POST /api/material-receipts
    [HttpPost]
    [Consumes("application/json")]
    [SwaggerRequestExample(typeof(MaterialReceiptUpsertRequest), typeof(MaterialReceiptUpsertRequestExample))]
    [SwaggerResponseExample(201, typeof(MaterialReceiptUpsertResponseExample))]
    [ProducesResponseType(typeof(MaterialReceiptUpsertResponse), StatusCodes.Status201Created)]
    public IActionResult Create([FromBody] MaterialReceiptUpsertRequest request)
        => Created(
            string.Empty,
            new MaterialReceiptUpsertResponse(Guid.NewGuid(), MaterialReceiptStatus.Draft)
        );

    // PUT /api/material-receipts/{id}
    [HttpPut("{id:guid}")]
    [Consumes("application/json")]
    [SwaggerRequestExample(typeof(MaterialReceiptUpsertRequest), typeof(MaterialReceiptUpsertRequestExample))]
    [SwaggerResponseExample(200, typeof(MaterialReceiptUpsertResponseExample))]
    [ProducesResponseType(typeof(MaterialReceiptUpsertResponse), StatusCodes.Status200OK)]
    public IActionResult Update(Guid id, [FromBody] MaterialReceiptUpsertRequest request)
        => Ok(new MaterialReceiptUpsertResponse(id, MaterialReceiptStatus.Updated));

    // POST /api/material-receipts/{id}/post
    [HttpPost("{id:guid}/post")]
    [SwaggerResponseExample(200, typeof(MaterialReceiptPostResponseExample))]
    [ProducesResponseType(typeof(MaterialReceiptPostResponse), StatusCodes.Status200OK)]
    public IActionResult Post(Guid id)
        => Ok(new MaterialReceiptPostResponse(id, MaterialReceiptStatus.Posted, DateTime.UtcNow));

    // GET /api/material-receipts/{id}/lines
    [HttpGet("{id:guid}/lines")]
    [SwaggerResponseExample(200, typeof(MaterialReceiptLineResponseExample))]
    [ProducesResponseType(typeof(IEnumerable<MaterialReceiptLineResponse>), StatusCodes.Status200OK)]
    public IActionResult GetLines(Guid id) => Ok(Lines);

    // POST /api/material-receipts/{id}/lines
    [HttpPost("{id:guid}/lines")]
    [Consumes("application/json")]
    [SwaggerRequestExample(typeof(MaterialReceiptLineUpsertRequest), typeof(MaterialReceiptLineUpsertRequestExample))]
    [SwaggerResponseExample(201, typeof(MaterialReceiptLineUpsertResponseExample))]
    [ProducesResponseType(typeof(MaterialReceiptLineUpsertResponse), StatusCodes.Status201Created)]
    public IActionResult AddLine(Guid id, [FromBody] MaterialReceiptLineUpsertRequest request)
    {
        var line = new MaterialReceiptLineResponse(
            Id: Guid.NewGuid(),
            MaterialId: request.MaterialId,
            MaterialName: "Материал " + request.MaterialId.ToString()[..4],
            Quantity: request.Quantity,
            Unit: request.Unit,
            Price: request.Price,
            Amount: request.Price * request.Quantity
        );

        return Created(
            string.Empty,
            new MaterialReceiptLineUpsertResponse(id, line, MaterialReceiptStatus.LineAdded)
        );
    }

    // PUT /api/material-receipts/{id}/lines/{lineId}
    [HttpPut("{id:guid}/lines/{lineId:guid}")]
    [Consumes("application/json")]
    [SwaggerRequestExample(typeof(MaterialReceiptLineUpsertRequest), typeof(MaterialReceiptLineUpsertRequestExample))]
    [SwaggerResponseExample(200, typeof(MaterialReceiptLineUpsertResponseExample))]
    [ProducesResponseType(typeof(MaterialReceiptLineUpsertResponse), StatusCodes.Status200OK)]
    public IActionResult UpdateLine(Guid id, Guid lineId, [FromBody] MaterialReceiptLineUpsertRequest request)
    {
        var line = new MaterialReceiptLineResponse(
            Id: lineId,
            MaterialId: request.MaterialId,
            MaterialName: "Материал " + request.MaterialId.ToString()[..4],
            Quantity: request.Quantity,
            Unit: request.Unit,
            Price: request.Price,
            Amount: request.Price * request.Quantity
        );

        return Ok(new MaterialReceiptLineUpsertResponse(id, line, MaterialReceiptStatus.LineUpdated));
    }

    // DELETE /api/material-receipts/{id}/lines/{lineId}
    [HttpDelete("{id:guid}/lines/{lineId:guid}")]
    [SwaggerResponseExample(200, typeof(MaterialReceiptLineDeleteResponseExample))]
    [ProducesResponseType(typeof(MaterialReceiptLineDeleteResponse), StatusCodes.Status200OK)]
    public IActionResult DeleteLine(Guid id, Guid lineId)
        => Ok(new MaterialReceiptLineDeleteResponse(id, lineId, MaterialReceiptStatus.LineDeleted));
}
