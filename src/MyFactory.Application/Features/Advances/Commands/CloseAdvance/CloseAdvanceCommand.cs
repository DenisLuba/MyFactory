using System;
using MediatR;
using MyFactory.Application.DTOs.Advances;

namespace MyFactory.Application.Features.Advances.Commands.CloseAdvance;

public sealed record CloseAdvanceCommand(Guid AdvanceId, DateOnly ClosedAt) : IRequest<AdvanceDto>;
