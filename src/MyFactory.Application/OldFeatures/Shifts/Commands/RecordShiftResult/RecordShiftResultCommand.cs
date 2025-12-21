using System;
using MediatR;
using MyFactory.Application.DTOs.Shifts;

namespace MyFactory.Application.OldFeatures.Shifts.Commands.RecordShiftResult;

public sealed record RecordShiftResultCommand(
    Guid ShiftPlanId,
    decimal ActualQuantity,
    decimal HoursWorked,
    DateTime RecordedAt) : IRequest<ShiftPlanDto>;
