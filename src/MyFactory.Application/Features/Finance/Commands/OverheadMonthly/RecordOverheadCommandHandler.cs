using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Finance;
using OverheadEntry = MyFactory.Domain.Entities.Reports.OverheadMonthly;

namespace MyFactory.Application.Features.Finance.Commands.OverheadMonthly;

public sealed class RecordOverheadCommandHandler : IRequestHandler<RecordOverheadCommand, OverheadMonthlyDto>
{
    private readonly IApplicationDbContext _context;

    public RecordOverheadCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<OverheadMonthlyDto> Handle(RecordOverheadCommand request, CancellationToken cancellationToken)
    {
        var expenseType = await _context.ExpenseTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(entity => entity.Id == request.ExpenseTypeId, cancellationToken)
            ?? throw new InvalidOperationException("Expense type not found.");

        var overhead = await _context.OverheadMonthlyEntries
            .FirstOrDefaultAsync(
                entity => entity.PeriodMonth == request.PeriodMonth
                    && entity.PeriodYear == request.PeriodYear
                    && entity.ExpenseTypeId == request.ExpenseTypeId,
                cancellationToken);

        if (overhead is null)
        {
            overhead = new OverheadEntry(request.PeriodMonth, request.PeriodYear, request.ExpenseTypeId, request.Amount, request.Notes);
            await _context.OverheadMonthlyEntries.AddAsync(overhead, cancellationToken);
        }
        else
        {
            overhead.UpdateAmount(request.Amount);
            overhead.UpdateNotes(request.Notes);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return OverheadMonthlyDto.FromEntity(overhead, expenseType.Name);
    }
}
