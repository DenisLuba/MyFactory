using Microsoft.AspNetCore.Mvc;
using MyFactory.WebApi.Contracts.MaterialTransfers;
using MyFactory.WebApi.Contracts.ProductionOrders;
using MyFactory.WebApi.SwaggerExamples.MaterialTransfers;
using MyFactory.WebApi.SwaggerExamples.ProductionOrders;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/production-orders")]
[Produces("application/json")]
public class ProductionOrdersController : ControllerBase
{
    private static readonly Guid Order1 = Guid.Parse("10000000-0000-0000-0000-000000000001");
    private static readonly Guid Order2 = Guid.Parse("10000000-0000-0000-0000-000000000002");

    private static readonly List<ProductionOrderCardResponse> Orders =
    [
        new(
            Order1,
            "PO-001",
            "Пижама женская",
            120,
            new DateTime(2025, 11, 10),
            new DateTime(2025, 11, 20),
            "Анна Смирнова",
            "В работе"),
        new(
            Order2,
            "PO-002",
            "Халат махровый",
            80,
            new DateTime(2025, 11, 12),
            new DateTime(2025, 11, 25),
            "Игорь Соколов",
            "Черновик")
    ];

    private static readonly Dictionary<Guid, List<StageDistributionItemResponse>> StageDistribution = new()
    {
        [Order1] =
        [
            new("Крой", "Екатерина Крылова", 6.5, 120, "Завершено"),
            new("Пошив", "Марина Кузнецова", 8, 60, "В работе"),
            new("Контроль качества", "Иван Нестеров", 2.5, 40, "Запланировано")
        ],
        [Order2] =
        [
            new("Крой", "Степан Корнеев", 4, 80, "В работе"),
            new("Пошив", "Алина Громова", 5, 30, "Запланировано")
        ]
    };

    private static readonly Dictionary<Guid, List<MaterialTransferItemDto>> OrderTransfers = new()
    {
        [Order1] =
        [
            new(Guid.Parse("11111111-1111-1111-1111-111111111111"), "Ткань Ситец", 50, "м", 180, 9000),
            new(Guid.Parse("22222222-2222-2222-2222-222222222222"), "Молния 20 см", 200, "шт", 12, 2400)
        ],
        [Order2] =
        [
            new(Guid.Parse("33333333-3333-3333-3333-333333333333"), "Флис плотный", 30, "м", 250, 7500)
        ]
    };

    [HttpGet]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ProductionOrderListResponseExample))]
    [ProducesResponseType(typeof(IEnumerable<ProductionOrderListResponse>), StatusCodes.Status200OK)]
    public IActionResult List()
    {
        var list = Orders
            .Select(o => new ProductionOrderListResponse(
                o.OrderId,
                o.OrderNumber,
                o.ProductName,
                o.Quantity,
                o.StartDate,
                o.EndDate,
                o.Status))
            .OrderByDescending(o => o.StartDate)
            .ToList();

        return Ok(list);
    }

    [HttpGet("{id:guid}")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ProductionOrderCardResponseExample))]
    [ProducesResponseType(typeof(ProductionOrderCardResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Get(Guid id)
    {
        var order = Orders.FirstOrDefault(o => o.OrderId == id);
        return order is null ? NotFound() : Ok(order);
    }

    [HttpPost]
    [Consumes("application/json")]
    [SwaggerRequestExample(typeof(ProductionOrderCreateRequest), typeof(ProductionOrderCreateRequestExample))]
    [SwaggerResponseExample(StatusCodes.Status201Created, typeof(ProductionOrderCreateResponseExample))]
    [ProducesResponseType(typeof(ProductionOrderCreateResponse), StatusCodes.Status201Created)]
    public IActionResult Create([FromBody] ProductionOrderCreateRequest request)
    {
        var orderId = Guid.NewGuid();
        var orderNumber = $"PO-{Orders.Count + 1:000}";
        var order = new ProductionOrderCardResponse(
            orderId,
            orderNumber,
            request.ProductName,
            request.Quantity,
            request.StartDate,
            request.EndDate,
            request.Responsible,
            request.Status);

        Orders.Add(order);
        StageDistribution[orderId] = new List<StageDistributionItemResponse>();
        OrderTransfers[orderId] = new List<MaterialTransferItemDto>();

        return Created($"api/production-orders/{orderId}", new ProductionOrderCreateResponse(orderId, orderNumber, order.Status));
    }

    [HttpPut("{id:guid}")]
    [Consumes("application/json")]
    [SwaggerRequestExample(typeof(ProductionOrderUpdateRequest), typeof(ProductionOrderUpdateRequestExample))]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ProductionOrderUpdateResponseExample))]
    [ProducesResponseType(typeof(ProductionOrderUpdateResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(Guid id, [FromBody] ProductionOrderUpdateRequest request)
    {
        var index = Orders.FindIndex(o => o.OrderId == id);
        if (index < 0)
        {
            return NotFound();
        }

        var updated = Orders[index] with
        {
            ProductName = request.ProductName,
            Quantity = request.Quantity,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Responsible = request.Responsible,
            Status = request.Status
        };

        Orders[index] = updated;

        return Ok(new ProductionOrderUpdateResponse(id, updated.Status));
    }

    [HttpDelete("{id:guid}")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ProductionOrderDeleteResponseExample))]
    [ProducesResponseType(typeof(ProductionOrderDeleteResponse), StatusCodes.Status200OK)]
    public IActionResult Delete(Guid id)
    {
        var removed = Orders.RemoveAll(o => o.OrderId == id) > 0;
        StageDistribution.Remove(id);
        OrderTransfers.Remove(id);

        return Ok(new ProductionOrderDeleteResponse(id, removed));
    }

    [HttpGet("{id:guid}/material-transfers")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(MaterialTransfersForOrderExample))]
    [ProducesResponseType(typeof(IEnumerable<MaterialTransferItemDto>), StatusCodes.Status200OK)]
    public IActionResult GetMaterialTransfers(Guid id)
    {
        OrderTransfers.TryGetValue(id, out var items);
        items ??= new List<MaterialTransferItemDto>();
        return Ok(items);
    }

    [HttpGet("{id:guid}/stage-distribution")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(StageDistributionResponseExample))]
    [ProducesResponseType(typeof(IEnumerable<StageDistributionItemResponse>), StatusCodes.Status200OK)]
    public IActionResult GetStageDistribution(Guid id)
    {
        StageDistribution.TryGetValue(id, out var items);
        items ??= new List<StageDistributionItemResponse>();
        return Ok(items);
    }
}
