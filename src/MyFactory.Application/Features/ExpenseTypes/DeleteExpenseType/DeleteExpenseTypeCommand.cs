using MediatR;

namespace MyFactory.Application.Features.ExpenseTypes.DeleteExpenseType;

public sealed record DeleteExpenseTypeCommand(Guid Id) : IRequest;
