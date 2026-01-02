using MediatR;

namespace MyFactory.Application.Features.Advances.AddCashAdvanceAmount;

public sealed record AddCashAdvanceAmountCommand(
    Guid CashAdvanceId,
    DateOnly IssueDate,
    decimal Amount
) : IRequest;
