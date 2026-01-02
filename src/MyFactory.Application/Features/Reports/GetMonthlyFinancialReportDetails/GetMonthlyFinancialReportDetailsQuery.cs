using MediatR;
using MyFactory.Application.DTOs.Reports;

namespace MyFactory.Application.Features.Reports.GetMonthlyFinancialReportDetails;

public sealed record GetMonthlyFinancialReportDetailsQuery(int Year, int Month) : IRequest<MonthlyFinancialReportDetailsDto>;
