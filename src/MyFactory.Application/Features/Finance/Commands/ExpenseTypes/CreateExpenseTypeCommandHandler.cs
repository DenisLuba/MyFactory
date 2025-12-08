using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Finance;
using MyFactory.Domain.Entities.Finance;

namespace MyFactory.Application.Features.Finance.Commands.ExpenseTypes;

public sealed class CreateExpenseTypeCommandHandler : IRequestHandler<CreateExpenseTypeCommand, ExpenseTypeDto>
{
    private readonly IApplicationDbContext _context;

    public CreateExpenseTypeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ExpenseTypeDto> Handle(CreateExpenseTypeCommand request, CancellationToken cancellationToken)
    {
        var normalizedName = request.Name.Trim();
        var normalizedCategory = request.Category.Trim();

        var exists = await _context.ExpenseTypes
            .AsNoTracking()
            .AnyAsync(expenseType => expenseType.Name == normalizedName && expenseType.Category == normalizedCategory, cancellationToken);

        if (exists)
        {
            throw new InvalidOperationException("Expense type with the same name and category already exists.");
        }

        var expenseType = new ExpenseType(normalizedName, normalizedCategory);
        await _context.ExpenseTypes.AddAsync(expenseType, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return ExpenseTypeDto.FromEntity(expenseType);
    }
}
