using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Advances;
using MyFactory.Domain.Entities.Finance;

namespace MyFactory.Application.Features.Advances.Commands.AddAdvanceReport;

public sealed class AddAdvanceReportCommandHandler : IRequestHandler<AddAdvanceReportCommand, AdvanceDto>
{
    private readonly IApplicationDbContext _context;

    public AddAdvanceReportCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AdvanceDto> Handle(AddAdvanceReportCommand request, CancellationToken cancellationToken)
    {
        var advance = await _context.Advances
            .Include(entity => entity.Employee)
            .Include(entity => entity.Reports)
            .FirstOrDefaultAsync(entity => entity.Id == request.AdvanceId, cancellationToken)
            ?? throw new InvalidOperationException("Advance not found.");

        if (advance.Status != AdvanceStatus.Issued)
        {
            throw new InvalidOperationException("Reports can only be added to issued advances.");
        }

        if (request.Amount > advance.RemainingAmount)
        {
            throw new InvalidOperationException("Report amount exceeds remaining balance.");
        }

        var fileExists = await _context.FileResources
            .AsNoTracking()
            .AnyAsync(file => file.Id == request.FileId, cancellationToken);

        if (!fileExists)
        {
            throw new InvalidOperationException("Referenced file not found.");
        }

        var report = advance.AddReport(request.Description, request.Amount, request.FileId, request.SpentAt);
        await _context.AdvanceReports.AddAsync(report, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var employeeName = advance.Employee?.FullName ?? string.Empty;
        return AdvanceDto.FromEntity(advance, employeeName);
    }
}
