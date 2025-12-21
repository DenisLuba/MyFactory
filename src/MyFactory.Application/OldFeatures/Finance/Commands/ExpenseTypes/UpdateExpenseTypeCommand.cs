using System;
using MediatR;
using MyFactory.Application.DTOs.Finance;

namespace MyFactory.Application.OldFeatures.Finance.Commands.ExpenseTypes;

public sealed record UpdateExpenseTypeCommand(Guid Id, string Name, string Category) : IRequest<ExpenseTypeDto>;
