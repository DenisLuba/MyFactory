using MediatR;

namespace MyFactory.Application.Features.RemoveTimesheetEntry;

public sealed record RemoveTimesheetEntryCommand(
    Guid EntryId
) : IRequest;