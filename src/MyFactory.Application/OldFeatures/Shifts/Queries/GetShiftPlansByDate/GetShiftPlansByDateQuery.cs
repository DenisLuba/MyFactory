using System;
using System.Collections.Generic;
using MediatR;
using MyFactory.Application.DTOs.Shifts;

namespace MyFactory.Application.OldFeatures.Shifts.Queries.GetShiftPlansByDate;

public sealed record GetShiftPlansByDateQuery(
    DateOnly ShiftDate,
    Guid? EmployeeId) : IRequest<IReadOnlyCollection<ShiftPlanDto>>;
