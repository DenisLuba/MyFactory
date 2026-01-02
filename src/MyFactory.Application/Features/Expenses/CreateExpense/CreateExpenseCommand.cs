using MediatR;

namespace MyFactory.Application.Features.Expenses.CreateExpense;

public sealed record CreateExpenseCommand(
    Guid ExpenseTypeId,
    DateOnly ExpenseDate,
    decimal Amount,
    string? Description
) : IRequest<Guid>;
