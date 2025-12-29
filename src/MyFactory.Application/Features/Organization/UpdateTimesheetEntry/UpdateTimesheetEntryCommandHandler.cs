using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;

namespace MyFactory.Application.Features.UpdateTimesheetEntry;

public sealed class UpdateTimesheetEntryCommandHandler
    : IRequestHandler<UpdateTimesheetEntryCommand>
{
    private readonly IApplicationDbContext _db;

    public UpdateTimesheetEntryCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(
        UpdateTimesheetEntryCommand request,
        CancellationToken cancellationToken)
    {
        var entry = await _db.Timesheets
            .FirstOrDefaultAsync(x => x.Id == request.EntryId, cancellationToken)
            ?? throw new NotFoundException("Timesheet entry not found");

        entry.Update(request.Hours, request.Comment);

        await _db.SaveChangesAsync(cancellationToken);
    }
}
