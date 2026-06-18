using MediatR;

namespace MyFactory.Application.Features.Expenses.UpdateExpense;

public sealed record UpdateExpenseCommand(
    Guid Id,
    DateOnly ExpenseDate,
    Guid ExpenseTypeId,
    decimal Amount,
    string? Description
) : IRequest;
