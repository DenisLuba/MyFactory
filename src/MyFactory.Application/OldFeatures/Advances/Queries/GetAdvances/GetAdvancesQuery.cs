using System;
using System.Collections.Generic;
using MediatR;
using MyFactory.Application.DTOs.Advances;
using MyFactory.Domain.Entities.Finance;

namespace MyFactory.Application.OldFeatures.Advances.Queries.GetAdvances;

public sealed record GetAdvancesQuery(
    string? Status,
    Guid? EmployeeId) : IRequest<IReadOnlyCollection<AdvanceDto>>;
