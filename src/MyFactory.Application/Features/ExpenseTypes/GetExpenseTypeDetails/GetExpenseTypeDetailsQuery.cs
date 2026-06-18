using MediatR;
using MyFactory.Application.DTOs.ExpenseTypes;

namespace MyFactory.Application.Features.ExpenseTypes.GetExpenseTypeDetails;

public sealed record GetExpenseTypeDetailsQuery(Guid Id) : IRequest<ExpenseTypeDto>;
