using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyFactory.Application.Features.ProductionOrders.AddProductionStageEmployee;
using MyFactory.Application.Features.ProductionOrders.CancelProductionOrder;
using MyFactory.Application.Features.ProductionOrders.CompleteProductionStage;
using MyFactory.Application.Features.ProductionOrders.CreateProductionOrder;
using MyFactory.Application.Features.ProductionOrders.DeleteProductionOrder;
using MyFactory.Application.Features.ProductionOrders.GetProductionOrderDetails;
using MyFactory.Application.Features.ProductionOrders.GetProductionOrderMaterialIssueDetails;
using MyFactory.Application.Features.ProductionOrders.GetProductionOrderMaterials;
using MyFactory.Application.Features.ProductionOrders.GetProductionOrderShipments;
using MyFactory.Application.Features.ProductionOrders.GetProductionOrders;
using MyFactory.Application.Features.ProductionOrders.GetProductionOrdersBySalesOrder;
using MyFactory.Application.Features.ProductionOrders.GetProductionStageEmployees;
using MyFactory.Application.Features.ProductionOrders.GetProductionStages;
using MyFactory.Application.Features.ProductionOrders.IssueMaterialsToProduction;
using MyFactory.Application.Features.ProductionOrders.RemoveProductionStageEmployee;
using MyFactory.Application.Features.ProductionOrders.ShipFinishedGoods;
using MyFactory.Application.Features.ProductionOrders.StartProductionStage;
using MyFactory.Application.Features.ProductionOrders.UpdateProductionOrder;
using MyFactory.Application.Features.ProductionOrders.UpdateProductionStageEmployee;
using MyFactory.Domain.Entities.Production;
using MyFactory.WebApi.Contracts.ProductionOrders;
using MyFactory.WebApi.SwaggerExamples.ProductionOrders;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/production-orders")]
[Produces("application/json")]
public class ProductionOrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductionOrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // -------------------------
    //  LIST
    // -------------------------
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ProductionOrderListItemResponse>), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(ProductionOrderListResponseExample))]
    public async Task<IActionResult> GetList()
    {
        var dtos = await _mediator.Send(new GetProductionOrdersQuery());
        var response = dtos
            .Select(x => new ProductionOrderListItemResponse(
                x.Id,
                x.ProductionOrderNumber,
                x.SalesOrderNumber,
                x.ProductName,
                x.QtyPlanned,
                x.QtyFinished,
                x.Status))
            .ToList();
        return Ok(response);
    }

    // -------------------------
    //  LIST BY SALES ORDER
    // -------------------------
    [HttpGet("sales-order/{salesOrderId:guid}")]
    [ProducesResponseType(typeof(IReadOnlyList<ProductionOrderListItemResponse>), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(ProductionOrderListResponseExample))]
    public async Task<IActionResult> GetBySalesOrder(Guid salesOrderId)
    {
        var dtos = await _mediator.Send(new GetProductionOrdersBySalesOrderQuery(salesOrderId));
        var response = dtos
            .Select(x => new ProductionOrderListItemResponse(
                x.Id,
                x.ProductionOrderNumber,
                x.SalesOrderNumber,
                x.ProductName,
                x.QtyPlanned,
                x.QtyFinished,
                x.Status))
            .ToList();
        return Ok(response);
    }

    // -------------------------
    //  DETAILS
    // -------------------------
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ProductionOrderDetailsResponse), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(ProductionOrderDetailsResponseExample))]
    public async Task<IActionResult> GetDetails(Guid id)
    {
        var dto = await _mediator.Send(new GetProductionOrderDetailsQuery(id));
        var response = new ProductionOrderDetailsResponse(
            dto.Id,
            dto.ProductionOrderNumber,
            dto.SalesOrderItemId,
            dto.DepartmentId,
            dto.QtyPlanned,
            dto.QtyCut,
            dto.QtySewn,
            dto.QtyPacked,
            dto.QtyFinished,
            dto.Status);
        return Ok(response);
    }

    // -------------------------
    //  CREATE
    // -------------------------
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(CreateProductionOrderResponse), StatusCodes.Status201Created)]
    [SwaggerRequestExample(typeof(CreateProductionOrderRequest), typeof(CreateProductionOrderRequestExample))]
    [SwaggerResponseExample(201, typeof(CreateProductionOrderResponseExample))]
    public async Task<IActionResult> Create([FromBody] CreateProductionOrderRequest req)
    {
        var id = await _mediator.Send(new CreateProductionOrderCommand
        {
            SalesOrderItemId = req.SalesOrderItemId,
            DepartmentId = req.DepartmentId,
            QtyPlanned = req.QtyPlanned
        });

        return Created(string.Empty, new CreateProductionOrderResponse(id));
    }

    // -------------------------
    //  UPDATE
    // -------------------------
    [HttpPut("{id:guid}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [SwaggerRequestExample(typeof(UpdateProductionOrderRequest), typeof(UpdateProductionOrderRequestExample))]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductionOrderRequest req)
    {
        await _mediator.Send(new UpdateProductionOrderCommand
        {
            ProductionOrderId = id,
            DepartmentId = req.DepartmentId,
            QtyPlanned = req.QtyPlanned
        });

        return NoContent();
    }

    // -------------------------
    //  DELETE
    // -------------------------
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteProductionOrderCommand(id));
        return NoContent();
    }

    // -------------------------
    //  CANCEL
    // -------------------------
    [HttpPost("{id:guid}/cancel")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Cancel(Guid id)
    {
        await _mediator.Send(new CancelProductionOrderCommand(id));
        return NoContent();
    }

    // -------------------------
    //  START STAGE
    // -------------------------
    [HttpPost("{id:guid}/start-stage")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [SwaggerRequestExample(typeof(StartProductionStageRequest), typeof(StartProductionStageRequestExample))]
    public async Task<IActionResult> StartStage(Guid id, [FromBody] StartProductionStageRequest req)
    {
        await _mediator.Send(new StartProductionStageCommand(id, req.TargetStatus));
        return NoContent();
    }

    // -------------------------
    //  COMPLETE STAGE
    // -------------------------
    [HttpPost("{id:guid}/complete-stage")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [SwaggerRequestExample(typeof(CompleteProductionStageRequest), typeof(CompleteProductionStageRequestExample))]
    public async Task<IActionResult> CompleteStage(Guid id, [FromBody] CompleteProductionStageRequest req)
    {
        await _mediator.Send(new CompleteProductionStageCommand(id, req.Qty));
        return NoContent();
    }

    // -------------------------
    //  MATERIALS
    // -------------------------
    [HttpGet("{id:guid}/materials")]
    [ProducesResponseType(typeof(IReadOnlyList<ProductionOrderMaterialResponse>), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(ProductionOrderMaterialsResponseExample))]
    public async Task<IActionResult> GetMaterials(Guid id)
    {
        var dtos = await _mediator.Send(new GetProductionOrderMaterialsQuery(id));
        var response = dtos
            .Select(x => new ProductionOrderMaterialResponse(
                x.MaterialId,
                x.MaterialName,
                x.RequiredQty,
                x.AvailableQty,
                x.MissingQty))
            .ToList();
        return Ok(response);
    }

    // -------------------------
    //  MATERIAL ISSUE DETAILS
    // -------------------------
    [HttpGet("{id:guid}/materials/{materialId:guid}/issue-details")]
    [ProducesResponseType(typeof(ProductionOrderMaterialIssueDetailsResponse), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(ProductionOrderMaterialIssueDetailsResponseExample))]
    public async Task<IActionResult> GetMaterialIssueDetails(Guid id, Guid materialId)
    {
        var dto = await _mediator.Send(new GetProductionOrderMaterialIssueDetailsQuery
        {
            ProductionOrderId = id,
            MaterialId = materialId
        });

        var response = new ProductionOrderMaterialIssueDetailsResponse(
            new ProductionOrderMaterialResponse(
                dto.Material.MaterialId,
                dto.Material.MaterialName,
                dto.Material.RequiredQty,
                dto.Material.AvailableQty,
                dto.Material.MissingQty),
            dto.Warehouses
                .Select(w => new ProductionOrderMaterialWarehouseResponse(
                    w.WarehouseId,
                    w.WarehouseName,
                    w.AvailableQty))
                .ToList());

        return Ok(response);
    }

    // -------------------------
    //  ISSUE MATERIALS
    // -------------------------
    [HttpPost("{id:guid}/materials/issue")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [SwaggerRequestExample(typeof(IssueMaterialsToProductionRequest), typeof(IssueMaterialsToProductionRequestExample))]
    public async Task<IActionResult> IssueMaterials(Guid id, [FromBody] IssueMaterialsToProductionRequest req)
    {
        await _mediator.Send(new IssueMaterialsToProductionCommand
        {
            ProductionOrderId = id,
            Materials = req.Materials
                .Select(m => new MyFactory.Application.DTOs.ProductionOrders.IssueMaterialLineDto
                {
                    MaterialId = m.MaterialId,
                    WarehouseId = m.WarehouseId,
                    Qty = m.Qty
                })
                .ToList()
        });

        return NoContent();
    }

    // -------------------------
    //  STAGES SUMMARY
    // -------------------------
    [HttpGet("{id:guid}/stages")]
    [ProducesResponseType(typeof(IReadOnlyList<ProductionStageSummaryResponse>), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(ProductionStagesResponseExample))]
    public async Task<IActionResult> GetStages(Guid id)
    {
        var dtos = await _mediator.Send(new GetProductionStagesQuery(id));
        var response = dtos
            .Select(x => new ProductionStageSummaryResponse(x.Stage, x.CompletedQty, x.RemainingQty))
            .ToList();
        return Ok(response);
    }

    // -------------------------
    //  STAGE EMPLOYEES
    // -------------------------
    [HttpGet("{id:guid}/stages/{stage}")]
    [ProducesResponseType(typeof(IReadOnlyList<ProductionStageEmployeeResponse>), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(ProductionStageEmployeesResponseExample))]
    public async Task<IActionResult> GetStageEmployees(Guid id, ProductionOrderStatus stage)
    {
        var dtos = await _mediator.Send(new GetProductionStageEmployeesQuery(id, stage));

        var response = dtos
            .Select(x => new ProductionStageEmployeeResponse(
                x.EmployeeId,
                x.EmployeeName,
                x.PlanPerHour,
                x.AssignedQty,
                x.CompletedQty))
            .ToList();
        return Ok(response);
    }

    // -------------------------
    //  ADD STAGE EMPLOYEE
    // -------------------------
    [HttpPost("{id:guid}/stages/{stage}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [SwaggerRequestExample(typeof(AddProductionStageEmployeeRequest), typeof(AddProductionStageEmployeeRequestExample))]
    public async Task<IActionResult> AddStageEmployee(Guid id, ProductionOrderStatus stage, [FromBody] AddProductionStageEmployeeRequest req)
    {
        await _mediator.Send(new AddProductionStageEmployeeCommand(
            id,
            stage,
            req.EmployeeId,
            req.QtyPlanned,
            req.QtyCompleted,
            req.Date,
            req.HoursWorked));

        return NoContent();
    }

    // -------------------------
    //  UPDATE STAGE EMPLOYEE
    // -------------------------
    [HttpPut("{id:guid}/stages/{stage}/employees/{operationId:guid}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [SwaggerRequestExample(typeof(UpdateProductionStageEmployeeRequest), typeof(UpdateProductionStageEmployeeRequestExample))]
    public async Task<IActionResult> UpdateStageEmployee(
        Guid id,
        ProductionOrderStatus stage,
        Guid operationId,
        [FromBody] UpdateProductionStageEmployeeRequest req)
    {
        await _mediator.Send(new UpdateProductionStageEmployeeCommand(
            operationId,
            stage,
            id,
            req.EmployeeId,
            req.QtyPlanned,
            req.Qty,
            req.Date,
            req.HoursWorked));

        return NoContent();
    }

    // -------------------------
    //  REMOVE STAGE EMPLOYEE
    // -------------------------
    [HttpDelete("{id:guid}/stages/{stage}/employees/{operationId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RemoveStageEmployee(Guid id, ProductionOrderStatus stage, Guid operationId)
    {
        await _mediator.Send(new RemoveProductionStageEmployeeCommand(id, operationId, stage));
        return NoContent();
    }

    // -------------------------
    //  SHIP FINISHED GOODS
    // -------------------------
    [HttpPost("{id:guid}/ship")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [SwaggerRequestExample(typeof(ShipFinishedGoodsRequest), typeof(ShipFinishedGoodsRequestExample))]
    public async Task<IActionResult> Ship(Guid id, [FromBody] ShipFinishedGoodsRequest req)
    {
        await _mediator.Send(new ShipFinishedGoodsCommand
        {
            ProductionOrderId = id,
            FromWarehouseId = req.FromWarehouseId,
            ToWarehouseId = req.ToWarehouseId,
            Qty = req.Qty
        });

        return NoContent();
    }

    // -------------------------
    //  SHIPMENTS
    // -------------------------
    [HttpGet("{id:guid}/shipments")]
    [ProducesResponseType(typeof(IReadOnlyList<ProductionOrderShipmentResponse>), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(ProductionOrderShipmentsResponseExample))]
    public async Task<IActionResult> GetShipments(Guid id)
    {
        var dtos = await _mediator.Send(new GetProductionOrderShipmentsQuery(id));
        var response = dtos
            .Select(x => new ProductionOrderShipmentResponse(
                x.WarehouseId,
                x.WarehouseName,
                x.Qty,
                x.ShipmentDate))
            .ToList();
        return Ok(response);
    }
}
