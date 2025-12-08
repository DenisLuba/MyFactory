using MediatR;
using MyFactory.Application.DTOs.Finance;

namespace MyFactory.Application.Features.Finance.Commands.ExpenseTypes;

public sealed record CreateExpenseTypeCommand(string Name, string Category) : IRequest<ExpenseTypeDto>;
