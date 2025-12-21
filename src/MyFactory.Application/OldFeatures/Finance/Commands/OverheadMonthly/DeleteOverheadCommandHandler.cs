using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Finance;

namespace MyFactory.Application.OldFeatures.Finance.Commands.OverheadMonthly;

public sealed class DeleteOverheadCommandHandler : IRequestHandler<DeleteOverheadCommand, OverheadMonthlyDto>
{
    private readonly IApplicationDbContext _context;

    public DeleteOverheadCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<OverheadMonthlyDto> Handle(DeleteOverheadCommand request, CancellationToken cancellationToken)
    {
        var overhead = await _context.OverheadMonthlyEntries
            .Include(entity => entity.ExpenseType)
            .FirstOrDefaultAsync(entity => entity.Id == request.OverheadMonthlyId, cancellationToken)
            ?? throw new InvalidOperationException("Overhead entry not found.");

        var dto = OverheadMonthlyDto.FromEntity(overhead, overhead.ExpenseType?.Name ?? string.Empty);

        _context.OverheadMonthlyEntries.Remove(overhead);
        await _context.SaveChangesAsync(cancellationToken);

        return dto;
    }
}
