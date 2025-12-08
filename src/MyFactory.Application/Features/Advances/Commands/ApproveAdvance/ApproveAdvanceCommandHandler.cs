using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Advances;
using MyFactory.Domain.Entities.Finance;

namespace MyFactory.Application.Features.Advances.Commands.ApproveAdvance;

public sealed class ApproveAdvanceCommandHandler : IRequestHandler<ApproveAdvanceCommand, AdvanceDto>
{
    private readonly IApplicationDbContext _context;

    public ApproveAdvanceCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AdvanceDto> Handle(ApproveAdvanceCommand request, CancellationToken cancellationToken)
    {
        var advance = await _context.Advances
            .Include(entity => entity.Employee)
            .Include(entity => entity.Reports)
            .FirstOrDefaultAsync(entity => entity.Id == request.AdvanceId, cancellationToken)
            ?? throw new InvalidOperationException("Advance not found.");

        if (advance.Status != AdvanceStatus.Draft)
        {
            throw new InvalidOperationException("Only draft advances can be approved.");
        }

        advance.Issue();
        await _context.SaveChangesAsync(cancellationToken);

        var employeeName = advance.Employee?.FullName ?? string.Empty;
        return AdvanceDto.FromEntity(advance, employeeName);
    }
}
