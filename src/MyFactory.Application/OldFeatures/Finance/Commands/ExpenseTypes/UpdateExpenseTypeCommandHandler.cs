using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Finance;

namespace MyFactory.Application.OldFeatures.Finance.Commands.ExpenseTypes;

public sealed class UpdateExpenseTypeCommandHandler : IRequestHandler<UpdateExpenseTypeCommand, ExpenseTypeDto>
{
    private readonly IApplicationDbContext _context;

    public UpdateExpenseTypeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ExpenseTypeDto> Handle(UpdateExpenseTypeCommand request, CancellationToken cancellationToken)
    {
        var expenseType = await _context.ExpenseTypes
            .FirstOrDefaultAsync(entity => entity.Id == request.Id, cancellationToken)
            ?? throw new InvalidOperationException("Expense type not found.");

        var normalizedName = request.Name.Trim();
        var normalizedCategory = request.Category.Trim();

        var duplicateExists = await _context.ExpenseTypes
            .AsNoTracking()
            .AnyAsync(entity => entity.Id != expenseType.Id && entity.Name == normalizedName && entity.Category == normalizedCategory, cancellationToken);

        if (duplicateExists)
        {
            throw new InvalidOperationException("Expense type with the same name and category already exists.");
        }

        expenseType.UpdateName(normalizedName);
        expenseType.UpdateCategory(normalizedCategory);

        await _context.SaveChangesAsync(cancellationToken);

        return ExpenseTypeDto.FromEntity(expenseType);
    }
}
