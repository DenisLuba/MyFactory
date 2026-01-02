using MediatR;

namespace MyFactory.Application.Features.Advances.CreateCashAdvanceExpense;

public sealed record CreateCashAdvanceExpenseCommand(
    Guid CashAdvanceId,
    DateOnly ExpenseDate,
    decimal Amount,
    string? Description
) : IRequest<Guid>;
