using System;
using MediatR;
using MyFactory.Domain.Entities.Production;

namespace MyFactory.Application.Features.ProductionOrders.UpdateProductionStageEmployee;

public sealed record UpdateProductionStageEmployeeCommand(
    Guid OperationId,
    ProductionOrderStatus Stage,
    Guid ProductionOrderId,
    Guid EmployeeId,
    int QtyPlanned,
    int Qty,
    DateOnly Date,
    decimal? HoursWorked
) : IRequest;
