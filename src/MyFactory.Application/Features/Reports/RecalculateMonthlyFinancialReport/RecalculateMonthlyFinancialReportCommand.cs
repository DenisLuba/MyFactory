using MediatR;

namespace MyFactory.Application.Features.Reports.RecalculateMonthlyFinancialReport;

public sealed record RecalculateMonthlyFinancialReportCommand(int Year, int Month) : IRequest;
