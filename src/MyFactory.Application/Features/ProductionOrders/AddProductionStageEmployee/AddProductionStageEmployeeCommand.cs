using System;
using MediatR;
using MyFactory.Domain.Entities.Production;

namespace MyFactory.Application.Features.ProductionOrders.AddProductionStageEmployee;

public sealed record AddProductionStageEmployeeCommand(
    Guid ProductionOrderId,
    ProductionOrderStatus Stage,
    Guid EmployeeId,
    int QtyPlanned,
    int Qty,
    DateOnly Date,
    decimal? HoursWorked
) : IRequest;
