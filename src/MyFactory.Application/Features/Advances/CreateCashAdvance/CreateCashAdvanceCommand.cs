using MediatR;

namespace MyFactory.Application.Features.Advances.CreateCashAdvance;

public sealed record CreateCashAdvanceCommand(
    Guid EmployeeId,
    DateOnly IssueDate,
    decimal Amount,
    string? Description
) : IRequest<Guid>;
