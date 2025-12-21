using System;
using MediatR;
using MyFactory.Application.DTOs.Finance;

namespace MyFactory.Application.OldFeatures.Finance.Commands.RevenueReports;

public sealed record MarkRevenuePaidCommand(Guid RevenueReportId, DateOnly PaymentDate) : IRequest<RevenueReportDto>;
