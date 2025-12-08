using System;
using MediatR;
using MyFactory.Application.DTOs.Advances;

namespace MyFactory.Application.Features.Advances.Commands.AddAdvanceReport;

public sealed record AddAdvanceReportCommand(
    Guid AdvanceId,
    string Description,
    decimal Amount,
    DateOnly ReportedAt) : IRequest<AdvanceDto>;
