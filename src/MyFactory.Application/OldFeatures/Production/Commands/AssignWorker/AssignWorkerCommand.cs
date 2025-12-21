using System;
using MediatR;
using MyFactory.Application.DTOs.Production;

namespace MyFactory.Application.OldFeatures.Production.Commands.AssignWorker;

public sealed record AssignWorkerCommand(
    Guid StageId,
    Guid EmployeeId,
    decimal QtyAssigned,
    decimal QtyCompleted) : IRequest<ProductionOrderDto>;
