using System;
using MediatR;
using MyFactory.Application.DTOs.Finance;

namespace MyFactory.Application.Features.Finance.Commands.OverheadMonthly;

public sealed record RecordOverheadCommand(
    int PeriodMonth,
    int PeriodYear,
    Guid ExpenseTypeId,
    decimal Amount,
    string? Notes) : IRequest<OverheadMonthlyDto>;
