using System;
using MediatR;
using MyFactory.Application.DTOs.Advances;

namespace MyFactory.Application.OldFeatures.Advances.Commands.ApproveAdvance;

public sealed record ApproveAdvanceCommand(Guid AdvanceId) : IRequest<AdvanceDto>;
