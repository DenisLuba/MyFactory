using MediatR;

namespace MyFactory.Application.Features.Employees.RemoveTimesheetEntry;

public sealed record RemoveTimesheetEntryCommand(
    Guid EntryId
) : IRequest;