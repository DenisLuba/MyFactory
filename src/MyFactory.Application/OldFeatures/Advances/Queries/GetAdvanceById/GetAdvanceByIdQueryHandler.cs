using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Advances;

namespace MyFactory.Application.OldFeatures.Advances.Queries.GetAdvanceById;

public sealed class GetAdvanceByIdQueryHandler : IRequestHandler<GetAdvanceByIdQuery, AdvanceDto>
{
    private readonly IApplicationDbContext _context;

    public GetAdvanceByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AdvanceDto> Handle(GetAdvanceByIdQuery request, CancellationToken cancellationToken)
    {
        var advance = await _context.Advances
            .AsNoTracking()
            .Include(entity => entity.Employee)
            .Include(entity => entity.Reports)
            .FirstOrDefaultAsync(entity => entity.Id == request.AdvanceId, cancellationToken)
            ?? throw new InvalidOperationException("Advance not found.");

        var employeeName = advance.Employee?.FullName ?? string.Empty;
        return AdvanceDto.FromEntity(advance, employeeName);
    }
}
