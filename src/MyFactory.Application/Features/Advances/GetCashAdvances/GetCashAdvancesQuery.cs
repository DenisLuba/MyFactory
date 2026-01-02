using MediatR;
using MyFactory.Application.DTOs.Advances;

namespace MyFactory.Application.Features.Advances.GetCashAdvances;

public sealed record GetCashAdvancesQuery
(
    DateOnly? From = null, 
    DateOnly? To = null, 
    Guid? EmployeeId = null
) : IRequest<IReadOnlyList<CashAdvanceListItemDto>>;
