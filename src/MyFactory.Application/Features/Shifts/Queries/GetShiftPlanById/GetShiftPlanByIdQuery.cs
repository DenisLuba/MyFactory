using System;
using MediatR;
using MyFactory.Application.DTOs.Shifts;

namespace MyFactory.Application.Features.Shifts.Queries.GetShiftPlanById;

public sealed record GetShiftPlanByIdQuery(Guid ShiftPlanId) : IRequest<ShiftPlanDto>;
