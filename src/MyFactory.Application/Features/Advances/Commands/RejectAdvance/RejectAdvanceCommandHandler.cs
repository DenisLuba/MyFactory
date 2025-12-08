using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Advances;
using MyFactory.Domain.Entities.Finance;

namespace MyFactory.Application.Features.Advances.Commands.RejectAdvance;

public sealed class RejectAdvanceCommandHandler : IRequestHandler<RejectAdvanceCommand, AdvanceDto>
{
    private readonly IApplicationDbContext _context;

    public RejectAdvanceCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AdvanceDto> Handle(RejectAdvanceCommand request, CancellationToken cancellationToken)
    {
        var advance = await _context.Advances
            .Include(entity => entity.Employee)
            .Include(entity => entity.Reports)
            .FirstOrDefaultAsync(entity => entity.Id == request.AdvanceId, cancellationToken)
            ?? throw new InvalidOperationException("Advance not found.");

        if (advance.Status != AdvanceStatus.Draft)
        {
            throw new InvalidOperationException("Only draft advances can be rejected.");
        }

        if (advance.Reports.Count > 0)
        {
            throw new InvalidOperationException("Draft advances cannot have reports.");
        }

        var employeeName = advance.Employee?.FullName ?? string.Empty;
        _context.Advances.Remove(advance);
        await _context.SaveChangesAsync(cancellationToken);

        return AdvanceDto.FromEntity(advance, employeeName);
    }
}
