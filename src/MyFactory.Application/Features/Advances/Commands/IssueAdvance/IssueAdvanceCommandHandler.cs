using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Advances;
using MyFactory.Domain.Entities.Finance;

namespace MyFactory.Application.Features.Advances.Commands.IssueAdvance;

public sealed class IssueAdvanceCommandHandler : IRequestHandler<IssueAdvanceCommand, AdvanceDto>
{
    private readonly IApplicationDbContext _context;

    public IssueAdvanceCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AdvanceDto> Handle(IssueAdvanceCommand request, CancellationToken cancellationToken)
    {
        var employee = await _context.Employees
            .AsNoTracking()
            .FirstOrDefaultAsync(emp => emp.Id == request.EmployeeId, cancellationToken)
            ?? throw new InvalidOperationException("Employee not found.");

        var advance = new Advance(request.EmployeeId, request.Amount, request.IssuedAt, request.Description);
        await _context.Advances.AddAsync(advance, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return AdvanceDto.FromEntity(advance, employee.FullName);
    }
}
