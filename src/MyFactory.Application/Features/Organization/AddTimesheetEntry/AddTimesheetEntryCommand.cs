using MediatR;

namespace MyFactory.Application.Features.AddTimesheetEntry;

public sealed record AddTimesheetEntryCommand(
    Guid EmployeeId,
    DateOnly Date,
    decimal Hours,
    string? Comment
) : IRequest<Guid>;