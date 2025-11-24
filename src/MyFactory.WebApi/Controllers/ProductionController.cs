using Microsoft.AspNetCore.Mvc;
using MyFactory.WebApi.Contracts.Production;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/production")]
[Produces("application/json")]
public class ProductionController : ControllerBase
{
    // ----------------------------
    // POST /api/production/orders
    // ----------------------------
    [HttpPost("orders")]
    [Consumes("application/json")]
    [SwaggerRequestExample(typeof(ProductionCreateOrderRequest), typeof(ProductionCreateOrderRequestExample))]
    [SwaggerResponseExample(201, typeof(ProductionCreateOrderResponseExample))]
    [ProducesResponseType(typeof(ProductionCreateOrderResponse), StatusCodes.Status201Created)]
    public IActionResult CreateOrder([FromBody] ProductionCreateOrderRequest request)
        => Created(
            "",
            new ProductionCreateOrderResponse(
                OrderId: Guid.NewGuid(),
                Status: ProductionStatus.Created
            )
        );

    // ----------------------------
    // GET /api/production/orders/{id}
    // ----------------------------
    [HttpGet("orders/{id}")]
    [SwaggerResponseExample(200, typeof(ProductionGetOrderResponseExample))]
    [ProducesResponseType(typeof(ProductionGetOrderResponse), StatusCodes.Status200OK)]
    public IActionResult GetOrder(Guid id)
        => Ok(new ProductionGetOrderResponse(
            Id: id,
            SpecificationId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            QtyOrdered: 10,
            Allocation:
            [
                new Allocation(Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"), 8)
            ]
        ));

    // ----------------------------
    // GET /api/production/orders/{id}/status
    // ----------------------------
    [HttpGet("orders/{id}/status")]
    [SwaggerResponseExample(200, typeof(ProductionGetOrderStatusResponseExample))]
    [ProducesResponseType(typeof(ProductionGetOrderStatusResponse), StatusCodes.Status200OK)]
    public IActionResult GetOrderStatus(Guid id)
        => Ok(new ProductionGetOrderStatusResponse(
            Id: id,
            Status: ProductionStatus.Allocated
        ));

    // ----------------------------
    // POST /api/production/orders/{id}/allocate
    // ----------------------------
    [HttpPost("orders/{id}/allocate")]
    [Consumes("application/json")]
    [SwaggerRequestExample(typeof(ProductionAllocateRequest), typeof(ProductionAllocateRequestExample))]
    [SwaggerResponseExample(200, typeof(ProductionAllocateResponseExample))]
    [ProducesResponseType(typeof(ProductionAllocateResponse), StatusCodes.Status200OK)]
    public IActionResult Allocate(Guid id, [FromBody] ProductionAllocateRequest request)
        => Ok(new ProductionAllocateResponse(
            OrderId: id,
            Status: ProductionStatus.Allocated
        ));

    // ----------------------------
    // POST /api/production/orders/{id}/stages
    // ----------------------------
    [HttpPost("orders/{id}/stages")]
    [Consumes("application/json")]
    [SwaggerRequestExample(typeof(ProductionRecordStageRequest), typeof(ProductionRecordStageRequestExample))]
    [SwaggerResponseExample(200, typeof(ProductionRecordStageResponseExample))]
    [ProducesResponseType(typeof(ProductionRecordStageResponse), StatusCodes.Status200OK)]
    public IActionResult RecordStage(Guid id, [FromBody] ProductionRecordStageRequest request)
        => Ok(new ProductionRecordStageResponse(
            OrderId: id,
            Status: ProductionStatus.StageRecorded
        ));

    // ----------------------------
    // POST /api/production/orders/{id}/assign
    // ----------------------------
    [HttpPost("orders/{id}/assign")]
    [Consumes("application/json")]
    [SwaggerRequestExample(typeof(ProductionAssignWorkerRequest), typeof(ProductionAssignWorkerRequestExample))]
    [SwaggerResponseExample(200, typeof(ProductionAssignWorkerResponseExample))]
    [ProducesResponseType(typeof(ProductionAssignWorkerResponse), StatusCodes.Status200OK)]
    public IActionResult AssignWorker(Guid id, [FromBody] ProductionAssignWorkerRequest request)
        => Ok(new ProductionAssignWorkerResponse(
            OrderId: id,
            Status: ProductionStatus.WorkerAssigned
        ));
}
