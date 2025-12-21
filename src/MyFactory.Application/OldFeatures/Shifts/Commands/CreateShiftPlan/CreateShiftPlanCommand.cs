using System;
using MediatR;
using MyFactory.Application.DTOs.Shifts;

namespace MyFactory.Application.OldFeatures.Shifts.Commands.CreateShiftPlan;

public sealed record CreateShiftPlanCommand(
    Guid EmployeeId,
    Guid SpecificationId,
    DateOnly ShiftDate,
    string ShiftType,
    decimal PlannedQuantity) : IRequest<ShiftPlanDto>;
