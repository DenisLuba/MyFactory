using MediatR;

namespace MyFactory.Application.Features.ExpenseTypes.UpdateExpenseType;

public sealed record UpdateExpenseTypeCommand(Guid Id, string Name, string? Description) : IRequest;
