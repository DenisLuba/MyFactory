using System.Collections.Generic;
using MediatR;
using MyFactory.Application.DTOs.Finance;

namespace MyFactory.Application.OldFeatures.Finance.Queries.GetExpenseTypes;

public sealed record GetExpenseTypesQuery : IRequest<IReadOnlyCollection<ExpenseTypeDto>>;
