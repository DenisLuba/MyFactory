using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;

namespace MyFactory.Application.Features.RemoveTimesheetEntry;

public sealed class RemoveTimesheetEntryCommandHandler
    : IRequestHandler<RemoveTimesheetEntryCommand>
{
    private readonly IApplicationDbContext _db;

    public RemoveTimesheetEntryCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(
        RemoveTimesheetEntryCommand request,
        CancellationToken cancellationToken)
    {
        var entry = await _db.Timesheets
            .FirstOrDefaultAsync(x => x.Id == request.EntryId, cancellationToken)
            ?? throw new NotFoundException("Timesheet entry not found");

        _db.Timesheets.Remove(entry);

        await _db.SaveChangesAsync(cancellationToken);
    }
}
