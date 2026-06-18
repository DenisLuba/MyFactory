using MediatR;
using MyFactory.Application.DTOs.ExpenseTypes;

namespace MyFactory.Application.Features.ExpenseTypes.GetExpenseTypes;

public sealed record GetExpenseTypesQuery : IRequest<IReadOnlyList<ExpenseTypeDto>>;
