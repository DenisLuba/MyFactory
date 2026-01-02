using MediatR;

namespace MyFactory.Application.Features.Expenses.DeleteExpense;

public sealed record DeleteExpenseCommand(Guid Id) : IRequest;
