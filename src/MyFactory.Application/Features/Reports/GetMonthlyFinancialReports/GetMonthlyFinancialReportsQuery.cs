using MediatR;
using MyFactory.Application.DTOs.Reports;

namespace MyFactory.Application.Features.Reports.GetMonthlyFinancialReports;

public sealed record GetMonthlyFinancialReportsQuery : IRequest<IReadOnlyList<MonthlyFinancialReportListItemDto>>;
