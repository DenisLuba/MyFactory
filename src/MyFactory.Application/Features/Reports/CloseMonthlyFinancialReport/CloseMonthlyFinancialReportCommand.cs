using MediatR;

namespace MyFactory.Application.Features.Reports.CloseMonthlyFinancialReport;

public sealed record CloseMonthlyFinancialReportCommand(int Year, int Month) : IRequest;
