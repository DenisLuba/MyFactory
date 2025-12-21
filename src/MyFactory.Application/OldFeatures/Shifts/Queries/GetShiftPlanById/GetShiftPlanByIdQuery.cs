using System;
using MediatR;
using MyFactory.Application.DTOs.Shifts;

namespace MyFactory.Application.OldFeatures.Shifts.Queries.GetShiftPlanById;

public sealed record GetShiftPlanByIdQuery(Guid ShiftPlanId) : IRequest<ShiftPlanDto>;
