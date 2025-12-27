using FluentValidation;

namespace MyFactory.Application.Features.ProductionOrders.RemoveProductionStageEmployee;

public sealed class RemoveProductionStageEmployeeCommandValidator : AbstractValidator<RemoveProductionStageEmployeeCommand>
{
    public RemoveProductionStageEmployeeCommandValidator()
    {
        RuleFor(x => x.OperationId).NotEmpty();
        RuleFor(x => x.Stage).IsInEnum();
    }
}
