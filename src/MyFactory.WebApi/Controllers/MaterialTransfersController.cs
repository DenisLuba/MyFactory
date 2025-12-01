using Microsoft.AspNetCore.Mvc;
using MyFactory.WebApi.Contracts.MaterialTransfers;
using MyFactory.WebApi.SwaggerExamples.MaterialTransfers;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/material-transfers")]
[Produces("application/json")]
public class MaterialTransfersController : ControllerBase
{
    private static readonly Guid Transfer001 = Guid.Parse("aaaa1111-2222-3333-4444-555555555555");
    private static readonly Guid Transfer002 = Guid.Parse("bbbb1111-2222-3333-4444-555555555555");

    private static readonly List<MaterialTransferCardResponse> Transfers =
    [
        CreateTransfer(
            Transfer001,
            "TR-001",
            new DateTime(2025, 11, 10),
            "PO-15",
            "Основной",
            MaterialTransferStatus.Submitted,
            new List<MaterialTransferItemDto>
            {
                new(
                    Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    "Ткань Ситец",
                    15,
                    "м",
                    180,
                    2700),
                new(
                    Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    "Молния 20 см",
                    100,
                    "шт",
                    12,
                    1200)
            }),
        CreateTransfer(
            Transfer002,
            "TR-002",
            new DateTime(2025, 11, 12),
            "PO-16",
            "Фурнитура",
            MaterialTransferStatus.Draft,
            new List<MaterialTransferItemDto>
            {
                new(
                    Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    "Пуговица пластик",
                    200,
                    "шт",
                    6.5m,
                    1300)
            })
    ];

    [HttpGet]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(MaterialTransferListResponseExample))]
    [ProducesResponseType(typeof(IEnumerable<MaterialTransferListResponse>), StatusCodes.Status200OK)]
    public IActionResult List([FromQuery] DateTime? date = null, [FromQuery] string? warehouse = null, [FromQuery] string? productionOrder = null)
    {
        var query = Transfers.AsEnumerable();

        if (date.HasValue)
        {
            query = query.Where(x => x.Date.Date == date.Value.Date);
        }

        if (!string.IsNullOrWhiteSpace(warehouse))
        {
            query = query.Where(x => x.Warehouse.Equals(warehouse, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(productionOrder))
        {
            query = query.Where(x => x.ProductionOrder.Equals(productionOrder, StringComparison.OrdinalIgnoreCase));
        }

        var result = query
            .Select(x => new MaterialTransferListResponse(
                x.TransferId,
                x.DocumentNumber,
                x.Date,
                x.ProductionOrder,
                x.Warehouse,
                x.TotalAmount,
                x.Status))
            .OrderByDescending(x => x.Date)
            .ToList();

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(MaterialTransferCardResponseExample))]
    [ProducesResponseType(typeof(MaterialTransferCardResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Get(Guid id)
    {
        var transfer = Transfers.FirstOrDefault(x => x.TransferId == id);
        return transfer is null ? NotFound() : Ok(transfer);
    }

    [HttpPost]
    [Consumes("application/json")]
    [SwaggerRequestExample(typeof(MaterialTransferCreateRequest), typeof(MaterialTransferCreateRequestExample))]
    [SwaggerResponseExample(StatusCodes.Status201Created, typeof(MaterialTransferCreateResponseExample))]
    [ProducesResponseType(typeof(MaterialTransferCreateResponse), StatusCodes.Status201Created)]
    public IActionResult Create([FromBody] MaterialTransferCreateRequest request)
    {
        var transferId = Guid.NewGuid();
        var documentNumber = $"TR-{Transfers.Count + 1:000}";
        var card = BuildCardFromRequest(
            transferId,
            documentNumber,
            request.Date,
            request.ProductionOrder,
            request.Warehouse,
            request.Items,
            MaterialTransferStatus.Draft);
        Transfers.Add(card);

        return Created($"api/material-transfers/{transferId}", new MaterialTransferCreateResponse(transferId, card.Status));
    }

    [HttpPut("{id:guid}")]
    [Consumes("application/json")]
    [SwaggerRequestExample(typeof(MaterialTransferUpdateRequest), typeof(MaterialTransferUpdateRequestExample))]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(MaterialTransferUpdateResponseExample))]
    [ProducesResponseType(typeof(MaterialTransferUpdateResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(Guid id, [FromBody] MaterialTransferUpdateRequest request)
    {
        var existingIndex = Transfers.FindIndex(x => x.TransferId == id);
        if (existingIndex < 0)
        {
            return NotFound();
        }

        var updated = BuildCardFromRequest(
            id,
            Transfers[existingIndex].DocumentNumber,
            request.Date,
            request.ProductionOrder,
            request.Warehouse,
            request.Items,
            Transfers[existingIndex].Status);
        Transfers[existingIndex] = updated;

        return Ok(new MaterialTransferUpdateResponse(id, updated.Status));
    }

    [HttpDelete("{id:guid}")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(MaterialTransferDeleteResponseExample))]
    [ProducesResponseType(typeof(MaterialTransferDeleteResponse), StatusCodes.Status200OK)]
    public IActionResult Delete(Guid id)
    {
        var removed = Transfers.RemoveAll(x => x.TransferId == id) > 0;
        return Ok(new MaterialTransferDeleteResponse(id, removed));
    }

    [HttpPost("{id:guid}/submit")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(MaterialTransferSubmitResponseExample))]
    [ProducesResponseType(typeof(MaterialTransferSubmitResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Submit(Guid id)
    {
        var index = Transfers.FindIndex(x => x.TransferId == id);
        if (index < 0)
        {
            return NotFound();
        }

        var existing = Transfers[index];
        var submitted = existing with { Status = MaterialTransferStatus.Submitted };
        Transfers[index] = submitted;

        return Ok(new MaterialTransferSubmitResponse(id, submitted.Status, DateTime.UtcNow));
    }

    private static MaterialTransferCardResponse BuildCardFromRequest(
        Guid transferId,
        string documentNumber,
        DateTime date,
        string productionOrder,
        string warehouse,
        IReadOnlyList<MaterialTransferItemRequest> requestItems,
        MaterialTransferStatus status)
    {
        var items = requestItems
            .Select(item => new MaterialTransferItemDto(
                item.MaterialId,
                item.MaterialName,
                item.Quantity,
                item.Unit,
                item.Price,
                item.Quantity * item.Price))
            .ToList();

        var total = items.Sum(i => i.LineTotal);

        return new MaterialTransferCardResponse(
            transferId,
            documentNumber,
            date,
            productionOrder,
            warehouse,
            total,
            status,
            items);
    }

    private static MaterialTransferCardResponse CreateTransfer(
        Guid transferId,
        string documentNumber,
        DateTime date,
        string productionOrder,
        string warehouse,
        MaterialTransferStatus status,
        IReadOnlyList<MaterialTransferItemDto> items)
    {
        var total = items.Sum(i => i.LineTotal);
        return new MaterialTransferCardResponse(
            transferId,
            documentNumber,
            date,
            productionOrder,
            warehouse,
            total,
            status,
            items);
    }
}
