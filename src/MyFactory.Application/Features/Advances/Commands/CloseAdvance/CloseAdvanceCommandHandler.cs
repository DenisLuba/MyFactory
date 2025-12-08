using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Advances;

namespace MyFactory.Application.Features.Advances.Commands.CloseAdvance;

public sealed class CloseAdvanceCommandHandler : IRequestHandler<CloseAdvanceCommand, AdvanceDto>
{
    private readonly IApplicationDbContext _context;

    public CloseAdvanceCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AdvanceDto> Handle(CloseAdvanceCommand request, CancellationToken cancellationToken)
    {
        var advance = await _context.Advances
            .Include(entity => entity.Employee)
            .Include(entity => entity.Reports)
            .FirstOrDefaultAsync(entity => entity.Id == request.AdvanceId, cancellationToken)
            ?? throw new InvalidOperationException("Advance not found.");

        advance.Close(request.ClosedAt);
        await _context.SaveChangesAsync(cancellationToken);

        var employeeName = advance.Employee?.FullName ?? string.Empty;
        return AdvanceDto.FromEntity(advance, employeeName);
    }
}
