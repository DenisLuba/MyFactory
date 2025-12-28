using MediatR;

namespace MyFactory.Application.Features.UpdateTimesheetEntry;

public sealed record UpdateTimesheetEntryCommand(
    Guid EntryId,
    decimal Hours,
    string? Comment
) : IRequest;