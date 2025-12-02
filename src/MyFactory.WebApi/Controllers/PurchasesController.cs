using System;
using System.Collections.Concurrent;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Purchases;
using MyFactory.WebApi.SwaggerExamples.Purchases;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/purchases")]
[Produces("application/json")]
public class PurchasesController : ControllerBase
{
    private static readonly ConcurrentDictionary<Guid, PurchaseRequestDetailResponse> Store = new();

    static PurchasesController()
    {
        var seedId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        var seedLines = new[]
        {
            new PurchaseRequestLineResponse(
                LineId: Guid.Parse("bbbbbbbb-0000-0000-0000-000000000001"),
                MaterialId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa11"),
                MaterialName: "Ткань Ситец",
                Quantity: 50,
                Unit: "м",
                Price: 250m,
                TotalAmount: 12500m,
                Note: "Основная партия"),
            new PurchaseRequestLineResponse(
                LineId: Guid.Parse("bbbbbbbb-0000-0000-0000-000000000002"),
                MaterialId: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                MaterialName: "Молния",
                Quantity: 100,
                Unit: "шт",
                Price: 35m,
                TotalAmount: 3500m,
                Note: null)
        };

        var seedDetail = new PurchaseRequestDetailResponse(
            PurchaseId: seedId,
            DocumentNumber: "PR-0001",
            CreatedAt: DateTime.Today.AddDays(-2),
            WarehouseName: "Основной склад",
            SupplierId: Guid.Parse("99999999-0000-0000-0000-000000000001"),
            Comment: "Срочная закупка",
            TotalAmount: seedLines.Sum(l => l.TotalAmount),
            Status: PurchasesStatus.Draft,
            Items: seedLines);

        Store.TryAdd(seedId, seedDetail);
    }

    // -------------------------
    //  CREATE PURCHASE REQUEST
    // -------------------------
    [HttpPost("requests")]
    [Consumes("application/json")]
    [SwaggerRequestExample(typeof(PurchasesCreateRequest), typeof(PurchasesCreateRequestExample))]
    [SwaggerResponseExample(201, typeof(PurchasesCreateResponseExample))]
    [ProducesResponseType(typeof(PurchasesCreateResponse), StatusCodes.Status201Created)]
    public IActionResult CreatePurchase([FromBody] PurchasesCreateRequest request)
    {
        var purchaseId = Guid.NewGuid();
        var detail = MapToDetail(purchaseId, request, PurchasesStatus.Draft);
        Store[purchaseId] = detail;

        var response = new PurchasesCreateResponse(purchaseId, detail.Status);
        return CreatedAtAction(nameof(GetPurchase), new { id = purchaseId }, response);
    }

    // -------------------------
    //  UPDATE PURCHASE REQUEST
    // -------------------------
    [HttpPut("requests/{id:guid}")]
    [Consumes("application/json")]
    [SwaggerRequestExample(typeof(PurchasesCreateRequest), typeof(PurchasesCreateRequestExample))]
    [SwaggerResponseExample(200, typeof(PurchasesCreateResponseExample))]
    [ProducesResponseType(typeof(PurchasesCreateResponse), StatusCodes.Status200OK)]
    public IActionResult UpdatePurchase(Guid id, [FromBody] PurchasesCreateRequest request)
    {
        if (!Store.TryGetValue(id, out var existing))
        {
            return NotFound();
        }

        var nextStatus = existing.Status == PurchasesStatus.ConvertedToOrder
            ? PurchasesStatus.ConvertedToOrder
            : PurchasesStatus.Created;

        var updated = MapToDetail(id, request, nextStatus);
        Store[id] = updated;

        return Ok(new PurchasesCreateResponse(id, updated.Status));
    }

    // -------------------------
    //  LIST PURCHASE REQUESTS
    // -------------------------
    [HttpGet("requests")]
    [SwaggerResponseExample(200, typeof(PurchasesResponseExample))]
    [ProducesResponseType(typeof(IEnumerable<PurchasesResponse>), StatusCodes.Status200OK)]
    public IActionResult PurchasesList()
    {
        var response = Store.Values
            .OrderByDescending(p => p.CreatedAt)
            .Select(detail => new PurchasesResponse(
                PurchaseId: detail.PurchaseId,
                DocumentNumber: detail.DocumentNumber,
                CreatedAt: detail.CreatedAt,
                TotalAmount: detail.TotalAmount,
                ItemsSummary: BuildItemsSummary(detail),
                Status: detail.Status));

        return Ok(response);
    }

    // -------------------------
    //  GET REQUEST CARD
    // -------------------------
    [HttpGet("requests/{id:guid}")]
    [SwaggerResponseExample(200, typeof(PurchaseRequestDetailResponseExample))]
    [ProducesResponseType(typeof(PurchaseRequestDetailResponse), StatusCodes.Status200OK)]
    public IActionResult GetPurchase(Guid id)
    {
        if (!Store.TryGetValue(id, out var detail))
        {
            return NotFound();
        }

        return Ok(detail);
    }

    // -------------------------
    //  DELETE REQUEST
    // -------------------------
    [HttpDelete("requests/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult DeletePurchase(Guid id)
        => Store.TryRemove(id, out _)
            ? NoContent()
            : NotFound();

    // -------------------------
    //  CONVERT REQUEST → ORDER
    // -------------------------
    [HttpPost("requests/{id:guid}/convert-to-order")]
    [SwaggerResponseExample(200, typeof(PurchasesConvertToOrderResponseExample))]
    [ProducesResponseType(typeof(PurchasesConvertToOrderResponse), StatusCodes.Status200OK)]
    public IActionResult ConvertToOrder(Guid id)
    {
        if (!Store.TryGetValue(id, out var detail))
        {
            return NotFound();
        }

        var converted = detail with { Status = PurchasesStatus.ConvertedToOrder };
        Store[id] = converted;

        return Ok(new PurchasesConvertToOrderResponse(id, converted.Status));
    }

    private static PurchaseRequestDetailResponse MapToDetail(Guid purchaseId, PurchasesCreateRequest request, PurchasesStatus status)
    {
        var items = request.Items ?? Array.Empty<PurchaseItemRequest>();
        var lines = items
            .Select(item => new PurchaseRequestLineResponse(
                LineId: Guid.NewGuid(),
                MaterialId: item.MaterialId,
                MaterialName: item.MaterialName,
                Quantity: item.Quantity,
                Unit: item.Unit,
                Price: item.Price,
                TotalAmount: item.Price * (decimal)item.Quantity,
                Note: item.Note))
            .ToArray();

        var total = lines.Sum(l => l.TotalAmount);

        return new PurchaseRequestDetailResponse(
            PurchaseId: purchaseId,
            DocumentNumber: request.DocumentNumber,
            CreatedAt: request.CreatedAt,
            WarehouseName: request.WarehouseName,
            SupplierId: request.SupplierId,
            Comment: request.Comment,
            TotalAmount: total,
            Status: status,
            Items: lines);
    }

    private static string[] BuildItemsSummary(PurchaseRequestDetailResponse detail)
        => detail.Items
            .Select(line => $"{line.MaterialName} ({line.Quantity:0.###} {line.Unit})")
            .ToArray();
}
