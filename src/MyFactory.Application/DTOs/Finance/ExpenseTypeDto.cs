using System;
using MyFactory.Domain.Entities.Finance;

namespace MyFactory.Application.DTOs.Finance;

public sealed record ExpenseTypeDto(Guid Id, string Name, string Category)
{
    public static ExpenseTypeDto FromEntity(ExpenseType expenseType)
    {
        return new ExpenseTypeDto(expenseType.Id, expenseType.Name, expenseType.Category);
    }
}
