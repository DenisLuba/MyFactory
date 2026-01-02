using MediatR;
using MyFactory.Application.DTOs.Expenses;

namespace MyFactory.Application.Features.Expenses.GetExpenses;

public sealed record GetExpensesQuery(DateOnly From, DateOnly To, Guid? ExpenseTypeId = null)
    : IRequest<IReadOnlyList<ExpenseListItemDto>>;
