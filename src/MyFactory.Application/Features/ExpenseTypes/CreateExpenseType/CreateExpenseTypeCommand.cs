using MediatR;

namespace MyFactory.Application.Features.ExpenseTypes.CreateExpenseType;

public sealed record CreateExpenseTypeCommand(string Name, string? Description) : IRequest<Guid>;
