using System.Collections.Generic;
using MediatR;
using MyFactory.Application.DTOs.Finance;

namespace MyFactory.Application.Features.Finance.Queries.GetExpenseTypes;

public sealed record GetExpenseTypesQuery : IRequest<IReadOnlyCollection<ExpenseTypeDto>>;
