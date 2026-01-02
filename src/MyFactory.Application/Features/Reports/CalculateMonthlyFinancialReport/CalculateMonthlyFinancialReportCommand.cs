using MediatR;

namespace MyFactory.Application.Features.Reports.CalculateMonthlyFinancialReport;

public sealed record CalculateMonthlyFinancialReportCommand(int Year, int Month) : IRequest<Guid>;
