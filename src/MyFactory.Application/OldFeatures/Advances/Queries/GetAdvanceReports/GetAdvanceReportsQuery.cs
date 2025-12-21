using System;
using System.Collections.Generic;
using MediatR;
using MyFactory.Application.DTOs.Advances;

namespace MyFactory.Application.OldFeatures.Advances.Queries.GetAdvanceReports;

public sealed record GetAdvanceReportsQuery(Guid AdvanceId) : IRequest<IReadOnlyCollection<AdvanceReportDto>>;
