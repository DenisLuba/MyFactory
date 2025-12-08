using System;
using MediatR;
using MyFactory.Application.DTOs.Finance;

namespace MyFactory.Application.Features.Finance.Commands.RevenueReports;

public sealed record MarkRevenuePaidCommand(Guid RevenueReportId, DateOnly PaymentDate) : IRequest<RevenueReportDto>;
