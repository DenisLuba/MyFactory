using MediatR;

namespace MyFactory.Application.Features.Reports.ApproveMonthlyFinancialReport;

public sealed record ApproveMonthlyFinancialReportCommand(int Year, int Month) : IRequest;
