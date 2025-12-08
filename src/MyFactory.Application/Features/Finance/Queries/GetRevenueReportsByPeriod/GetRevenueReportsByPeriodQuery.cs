using System.Collections.Generic;
using MediatR;
using MyFactory.Application.DTOs.Finance;

namespace MyFactory.Application.Features.Finance.Queries.GetRevenueReportsByPeriod;

public sealed record GetRevenueReportsByPeriodQuery(int PeriodMonth, int PeriodYear) : IRequest<IReadOnlyCollection<RevenueReportDto>>;
