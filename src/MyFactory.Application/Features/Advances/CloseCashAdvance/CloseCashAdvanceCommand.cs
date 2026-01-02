using MediatR;

namespace MyFactory.Application.Features.Advances.CloseCashAdvance;

public sealed record CloseCashAdvanceCommand(Guid CashAdvanceId) : IRequest;
