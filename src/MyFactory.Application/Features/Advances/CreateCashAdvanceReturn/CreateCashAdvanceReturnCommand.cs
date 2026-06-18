using MediatR;

namespace MyFactory.Application.Features.Advances.CreateCashAdvanceReturn;

public sealed record CreateCashAdvanceReturnCommand(
    Guid CashAdvanceId,
    DateOnly ReturnDate,
    decimal Amount,
    string? Description
) : IRequest<Guid>;
