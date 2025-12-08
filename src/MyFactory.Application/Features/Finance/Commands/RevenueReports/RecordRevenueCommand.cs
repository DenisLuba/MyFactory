using System;
using MediatR;
using MyFactory.Application.DTOs.Finance;

namespace MyFactory.Application.Features.Finance.Commands.RevenueReports;

public sealed record RecordRevenueCommand(
    int PeriodMonth,
    int PeriodYear,
    Guid SpecificationId,
    decimal Quantity,
    decimal UnitPrice,
    bool IsPaid,
    DateOnly? PaymentDate) : IRequest<RevenueReportDto>;
