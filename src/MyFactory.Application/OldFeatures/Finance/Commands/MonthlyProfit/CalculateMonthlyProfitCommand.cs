using MediatR;
using MyFactory.Application.DTOs.Finance;

namespace MyFactory.Application.OldFeatures.Finance.Commands.MonthlyProfit;

public sealed record CalculateMonthlyProfitCommand(int PeriodMonth, int PeriodYear) : IRequest<MonthlyProfitDto>;
