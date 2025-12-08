using System;
using MediatR;
using MyFactory.Application.DTOs.Advances;

namespace MyFactory.Application.Features.Advances.Commands.RejectAdvance;

public sealed record RejectAdvanceCommand(Guid AdvanceId) : IRequest<AdvanceDto>;
