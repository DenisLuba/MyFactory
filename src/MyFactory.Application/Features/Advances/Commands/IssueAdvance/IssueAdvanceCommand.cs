using System;
using MediatR;
using MyFactory.Application.DTOs.Advances;

namespace MyFactory.Application.Features.Advances.Commands.IssueAdvance;

public sealed record IssueAdvanceCommand(
    Guid EmployeeId,
    decimal Amount,
    DateOnly IssuedAt) : IRequest<AdvanceDto>;
