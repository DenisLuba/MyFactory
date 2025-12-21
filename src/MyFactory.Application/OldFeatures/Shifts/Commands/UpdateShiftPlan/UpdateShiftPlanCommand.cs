using System;
using MediatR;
using MyFactory.Application.DTOs.Shifts;

namespace MyFactory.Application.OldFeatures.Shifts.Commands.UpdateShiftPlan;

public sealed record UpdateShiftPlanCommand(
    Guid ShiftPlanId,
    string ShiftType,
    decimal PlannedQuantity) : IRequest<ShiftPlanDto>;
